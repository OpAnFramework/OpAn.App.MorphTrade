using Microsoft.Extensions.DependencyInjection;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Domain.Flows.Entities;
using OpAn.App.MorphTrade.Domain.Flows.Flows;
using OpAn.App.MorphTrade.Domain.Flows.Repositories;
using OpAn.App.MorphTrade.Domain.Flows.Tests.Fixtures;
using Testcontainers.PostgreSql;

namespace OpAn.App.MorphTrade.Domain.Flows.Tests;

/// <summary>
/// Tests the FlowsManager behaviour.
/// </summary>
[TestClass]
public class TestFlowsManagerBehavior
{
	private readonly IServiceCollection _commonServiceContainer;
	private PostgreSqlConfiguration? _postgresConfiguration;
	private int _postgresPort;

	/// <summary>
	/// Constructor for the persistent database tests.
	/// </summary>
	public TestFlowsManagerBehavior()
	{
		_commonServiceContainer = new ServiceCollection();
		_commonServiceContainer.AddSingleton<DatabaseFixtures>();
	}

	/// <summary>
	/// Initializes the tests.
	/// </summary>
	[TestInitialize]
	public async Task Initialize()
	{
		var serviceProvider = _commonServiceContainer.BuildServiceProvider();
		await serviceProvider.GetRequiredService<DatabaseFixtures>().InitAsync();
		(PostgreSqlConfiguration? config, int? port) = serviceProvider
			.GetRequiredService<DatabaseFixtures>()
			.GetRuntimePlugs();
		Assert.IsNotNull(config);
		Assert.IsNotNull(port);
		_postgresConfiguration = config;
		_postgresPort = (int)port;
	}

	/// <summary>
	/// Checks if the flows manager can create a flow.
	/// </summary>
	[TestMethod]
	public async Task Test_FlowManagerCanCreateFlow()
	{
		var serviceCollection = _commonServiceContainer.AddFlowDomainDependencies(
			_postgresConfiguration!,
			_postgresPort);
		var commonServices = serviceCollection.BuildServiceProvider();
		Assert.IsNotNull(commonServices);
		FlowsManager flowsManager = commonServices.GetService<FlowsManager>()!;

		FlowRegistryEntry? flowEntry = flowsManager.GetRegisteredFlows().FirstOrDefault();
		Assert.IsNotNull(flowEntry);

		await flowsManager.CreateFlow(flowEntry.Flow);

		IFlowsRepository flowsRepository = commonServices.GetService<IFlowsRepository>()!;
		FlowMeta? flowMetadata = (await flowsRepository
			.GetFlowsByNameAsync(flowEntry.Flow.GetFlowInstanceName()))!
			.FirstOrDefault();
		Assert.IsNotNull(flowMetadata);
		Assert.AreEqual(flowEntry.Flow.GetFlowInstanceName(), flowMetadata.Name);
	}

	/// <summary>
	/// Cleans up the database.
	/// </summary>
	[TestCleanup]
	public async Task Cleanup()
	{
		await _commonServiceContainer.BuildServiceProvider().GetRequiredService<DatabaseFixtures>()
			.DisposeAsync();
	}
}
