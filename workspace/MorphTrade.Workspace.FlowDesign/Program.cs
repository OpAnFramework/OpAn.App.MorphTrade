
using System.Text.Json;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Abstractions.Tools.JsonConverters;
using OpAn.App.MorphTrade.Console.Extensions;
using OpAn.App.MorphTrade.Domain.Finance.Trader;
using OpAn.App.MorphTrade.Domain.Flows;
using OpAn.App.MorphTrade.Workspace.FlowDesign.Extensions;

namespace OpAn.App.MorphTrade.Workspace.FlowDesign;

[PublicAPI]
internal class Program
{
	private static void Main(string[] args)
	{
		HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

		// Add Configurations
		builder.AddConfigurations();
		// Add Logging
		builder.Services.AddLogging();
		// Add test data json into the service container
		builder.Services.AddTestData(builder.Configuration);
		// Add trader unit for the Flow design.
		builder.Services.AddSingleton<ITrader, NullTrader>();

		builder.Services.AddScoped<IFlow, SimpleEmaDecisionFlow>();

		var services = builder.Services.BuildServiceProvider();

		services.GetService<IFlow>()!.Execute();
	}
}
