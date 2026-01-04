using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Abstractions.Flows.Enums;
using OpAn.App.MorphTrade.Domain.Flows.Entities;
using OpAn.App.MorphTrade.Domain.Flows.Repositories;

namespace OpAn.App.MorphTrade.Domain.Flows.Flows;

/// <summary>
/// Manages the flows registered under the hood.
/// </summary>
public class FlowsManager: BackgroundService
{
	private readonly ILogger<FlowsManager> _logger;
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly IHostApplicationLifetime? _hostApplicationLifetime;
	private readonly IUserInfo _userInfo;

	///  <summary>
	/// 	Flow manager constructor.
	///  </summary>
	///  <param name="scopeFactory">Injected service scope factory.</param>
	///  <param name="logger">Injected logger.</param>
	///  <param name="applicationLifetime">Injected application lifetime.</param>
	///  <param name="userInfo">Injected user info.</param>
	public FlowsManager(
		IServiceScopeFactory scopeFactory,
		ILogger<FlowsManager> logger,
		IHostApplicationLifetime applicationLifetime,
		IUserInfo userInfo
		)
	{
		_logger = logger;
		_serviceScopeFactory = scopeFactory;
		_hostApplicationLifetime = applicationLifetime;
		_userInfo = userInfo;
	}

	///  <summary>
	/// 	Flow manager constructor for unit testing.
	///  </summary>
	///  <param name="scopeFactory">Injected service scope factory.</param>
	///  <param name="logger">Injected logger.</param>
	///  <param name="userInfo">Injected user info.</param>
	public FlowsManager(
		IServiceScopeFactory scopeFactory,
		ILogger<FlowsManager> logger,
		IUserInfo userInfo
		)
	{
		_logger = logger;
		_serviceScopeFactory = scopeFactory;
		_userInfo = userInfo;
	}

	/// <summary>
	/// Provides all the registrable from the assembly.
	/// </summary>
	/// <returns></returns>
	public IList<FlowRegistryEntry> GetRegisteredFlows()
	{
		var serviceScope = _serviceScopeFactory.CreateScope();
		return serviceScope
			.ServiceProvider
			.GetService<IFlowRegistry>()!
			.Flows
			.ToList();
	}

	/// <summary>
	/// Manages a flow as a managed object.
	/// </summary>
	/// <param name="flow">Flow to be managed.</param>
	/// <returns></returns>
	public async Task<IFlow> CreateFlow(IFlow flow)
	{
		using var scope = _serviceScopeFactory.CreateScope();
		var flowsRepository = scope.ServiceProvider.GetRequiredService<IFlowsRepository>();
		// Find if the flow already exists on the database or not.
		FlowMeta? existingFlowMetadata = (
				await flowsRepository
				.GetFlowsByNameAsync(flow.GetFlowInstanceName()))!
			.FirstOrDefault();
		FlowMeta targetFlowMetadata = existingFlowMetadata ?? new FlowMeta()
		{
			UserId = _userInfo.UserId,
			Id = Guid.NewGuid().ToString(),
			Name = flow.GetFlowInstanceName(),
			Status = FlowStatus.Created,
			StatusDescription = $"Created with {nameof(FlowsManager)}"
		};

		_logger.LogInformation("Creating new flow {flow}", flow);
		await flowsRepository.CreateUpdateFlowAsync(targetFlowMetadata);
		return flow;
	}

	/// <summary>
	/// Starts the flow.
	/// </summary>
	/// <param name="flow">Flow to start.</param>
	/// <param name="stoppingToken">Stopping token.</param>
	/// <returns></returns>
	private async Task StartFlow(IFlow flow, CancellationToken stoppingToken)
	{
		using var serviceScope = _serviceScopeFactory.CreateScope();
		var flowsRepository = serviceScope.ServiceProvider.GetRequiredService<IFlowsRepository>();
		// Get flow from the database
		FlowMeta flowMetadata = (await flowsRepository
				.GetFlowsByNameAsync(flow.GetFlowInstanceName()))!
			.FirstOrDefault()
			?? new FlowMeta
			{
				UserId = _userInfo.UserId,
				Id = Guid.NewGuid().ToString(),
				Name = flow.GetFlowInstanceName(),
				LastUpdated = DateTime.Now,
				StatusDescription = "New flow created",
				Status = FlowStatus.Created
			};

		// Get the registry
		IFlowRegistry flowRegistry = serviceScope.ServiceProvider
			.GetService<IFlowRegistry>()!;

		FlowRegistryEntry entry = flowRegistry
			.Flows
			.FirstOrDefault(fre => flow.GetFlowInstanceName() == fre.Flow.GetFlowInstanceName())!;

		entry.Status = FlowStatus.Running;
		flowMetadata.Status = entry.Status;
		flowMetadata.StatusDescription = "Operation started";
		flowMetadata.LastUpdated = DateTime.UtcNow;

		// Set the execution context information to be injected into the internal services.
		SetExecutionContext(entry, flowMetadata);

		await flowsRepository.CreateUpdateFlowAsync(flowMetadata);

		// Start the flow
		try
		{
			_logger.LogInformation("Starting flow {flow}", entry.Flow.GetFlowInstanceName());
			if (entry.Context is not null)
			{
				await entry.Flow.ExecuteAsync(entry.Context, stoppingToken);
			}
			else
			{
				await entry.Flow.ExecuteAsync(stoppingToken);
			}
		}
		catch (OperationCanceledException)
		{
			_logger.LogInformation("Flow {flow} cancelled", entry.Flow.GetFlowInstanceName());
			entry.Status = FlowStatus.Canceled;
			flowMetadata.Status = entry.Status;
			flowMetadata.StatusDescription = "Operation cancelled";
			flowMetadata.LastUpdated = DateTime.UtcNow;

			// Set the execution context information to be injected into the internal services.
			SetExecutionContext(entry, flowMetadata);

			await flowsRepository.CreateUpdateFlowAsync(flowMetadata);
		}
		catch (Exception ex)
		{
			_logger.LogError("Flow {Flow} failed due to {Error}", entry.Flow.GetFlowInstanceName(), ex.Message);
			entry.Status = FlowStatus.Failed;
			flowMetadata.Status = entry.Status;
			flowMetadata.StatusDescription = ex.Message;
			flowMetadata.LastUpdated = DateTime.UtcNow;

			// Set the execution context information to be injected into the internal services.
			SetExecutionContext(entry, flowMetadata);

			await flowsRepository.CreateUpdateFlowAsync(flowMetadata);
			await base.StopAsync(stoppingToken);
		}
	}

	/// <summary>
	/// Stops a specific flow.
	/// </summary>
	/// <param name="flow">Flow to be stopped.</param>
	private async Task StopFlow(IFlow flow)
	{
		using var serviceScope = _serviceScopeFactory.CreateScope();
		var flowsRepository = serviceScope.ServiceProvider.GetRequiredService<IFlowsRepository>();
		// Get flow from the database
		FlowMeta flowMetadata = (await flowsRepository
				.GetFlowsByNameAsync(flow.GetFlowInstanceName()))!
			.FirstOrDefault()!;

		// Get the registry
		IFlowRegistry flowRegistry = serviceScope.ServiceProvider
			.GetService<IFlowRegistry>()!;

		FlowRegistryEntry entry = flowRegistry
			.Flows
			.FirstOrDefault(fre => flow.GetFlowInstanceName() == fre.Flow.GetFlowInstanceName())!;

		entry.Status = FlowStatus.Stopped;

		flowMetadata.Status = entry.Status;
		flowMetadata.StatusDescription = "Operation stopped";
		flowMetadata.LastUpdated = DateTime.UtcNow;

		// Set the execution context information to be injected into the internal services.
		SetExecutionContext(entry, flowMetadata);

		await flowsRepository.CreateUpdateFlowAsync(flowMetadata);
	}

	/// <inheritdoc />
	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Executing all the flows with {FlowManager}", nameof(FlowsManager));
		var serviceScope = _serviceScopeFactory.CreateScope();
		var flowRegistry = serviceScope.ServiceProvider.GetService<IFlowRegistry>()!;

		foreach (FlowRegistryEntry registryEntry in flowRegistry.Flows)
		{
			Task.Run(() => StartFlow(registryEntry.Flow, registryEntry.TokenSource.Token), stoppingToken);
		}

		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public override async Task StopAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Stopping flows");
		var serviceScope = _serviceScopeFactory.CreateScope();
		var flowRegistry = serviceScope.ServiceProvider.GetService<IFlowRegistry>()!;

		foreach (FlowRegistryEntry registryFlow in flowRegistry.Flows)
		{
			_logger.LogInformation("Stopping flow {flow}", registryFlow.Flow.GetFlowInstanceName());
			await StopFlow(registryFlow.Flow);
		}

		_logger.LogInformation("FlowsManager stopped.");
		await base.StopAsync(stoppingToken);
		_hostApplicationLifetime!.StopApplication();
	}

	private void SetExecutionContext(
		FlowRegistryEntry entry,
		FlowMeta flowMetadata)
	{
		FlowExecutionContext executionContext = entry.Context ?? new FlowExecutionContext()
		{
			CancellationToken = entry.TokenSource.Token,
		};

		// Add the metadata as an execution context.
		executionContext.Set(flowMetadata);

		entry.Context = executionContext;
	}
}
