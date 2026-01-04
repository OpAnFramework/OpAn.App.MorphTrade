using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Abstractions.Flows.Enums;
using OpAn.App.MorphTrade.Domain.Flows.Flows;
using OpAn.App.MorphTrade.Domain.Flows.Flows.EmaCrossover.EmaCrossoverSimple;
using OpAn.App.MorphTrade.Domain.Flows.Registries;
using OpAn.App.MorphTrade.Domain.Flows.Repositories;

namespace OpAn.App.MorphTrade.Domain.Flows.Extensions;

/// <summary>
/// Allows adding the flows domain to the service container.
/// </summary>
public static class FlowsDomainExtension
{
	private static readonly IFlowRegistry FlowRegistry = new FlowRegistry();
	/// <summary>
	/// Allows adding the flows domain to the service container.
	/// </summary>
	/// <param name="services">Service container to be extended.</param>
	/// <param name="configuration">Configuration to be followed.</param>
	/// <returns></returns>
	public static IServiceCollection AddFlowsDomain(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IFlowsRepository, FlowsRepository>();

		// Read the manager configurations.
		IConfigurationSection? managerOptions = configuration
			.GetSection(nameof(FlowManagerOptions));

		FlowManagerOptions? flowManagerOptions = managerOptions.Get<FlowManagerOptions>();
		if (flowManagerOptions is not null)
		{
			services.Configure<FlowManagerOptions>(options =>
			{
				options.Identifier = flowManagerOptions.Identifier;
				options.ManagedFlows = flowManagerOptions.ManagedFlows;
			});
		}

		services.AddHostedService<FlowsManager>();

		// Add the underlying flows to the service container.
		return services
			.AddFlows(managerOptions ?? null);
	}

	private static IServiceCollection AddFlows(
		this IServiceCollection services,
		IConfigurationSection? flowManagerOptions)
	{
		// Inject Simple Ema Crossover flow
		EmaCrossoverSimpleOptions? emaCrossoverSimpleOptions =
			flowManagerOptions!.GetFlowOptions<EmaCrossoverSimpleFlow, EmaCrossoverSimpleOptions>();
		if (emaCrossoverSimpleOptions is not null)
		{
			var cts = new CancellationTokenSource();
			services.AddScoped<EmaCrossoverSimpleFlow>();
			services.AddSingleton(Options.Create(emaCrossoverSimpleOptions));

			// Register the service to the service registry
			ServiceProvider serviceProvider = services.BuildServiceProvider();
			FlowRegistry.Register(
				new FlowRegistryEntry()
				{
					TokenSource = cts,
					Flow = serviceProvider.GetService<EmaCrossoverSimpleFlow>()!,
					Status = FlowStatus.Created
				});
		}

		services.AddSingleton(FlowRegistry);
		return services;
	}

	private static TFlowOptions? GetFlowOptions<TFlow, TFlowOptions>(
		this IConfigurationSection? flowManagerOptions)
	where TFlow : class, IFlow where TFlowOptions : FlowOptions
	{
		if (flowManagerOptions is null) return null;

		var flowType = typeof(TFlow).Name;

		var flowSection = flowManagerOptions
			.GetSection("ManagedFlows").GetChildren()
			.FirstOrDefault(fs => fs.GetValue<string>("Type") == flowType);
		if (flowSection is not null)
		{
			return flowSection.GetSection("Options").Get<TFlowOptions>();
		}

		return null;

	}

}
