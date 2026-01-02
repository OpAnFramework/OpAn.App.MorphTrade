using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Console.Extensions;
using OpAn.App.MorphTrade.Domain.Finance.Trader;
using OpAn.App.MorphTrade.Domain.Flows.Flows.EmaCrossover.EmaCrossoverSimple;
using OpAn.App.MorphTrade.Workspace.Flows.Backtests.Extensions;

namespace OpAn.App.MorphTrade.Workspace.Flows.Backtests;

[PublicAPI]
internal class Program
{
	private static void Main(string[] args)
	{
		HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

		// Add Configurations
		builder.AddConfigurations(Assembly.GetExecutingAssembly());
		// Add Logging
		builder.Services.AddLogging();
		// Add test data json into the service container
		builder.Services.AddTestData(builder.Configuration);
		// Add trader unit for the Flow design.
		builder.Services.AddSingleton<ITrader, NullTrader>();


		builder
			.Services
			.AddScoped<IFlow, EmaCrossoverSimpleFlow>();
		// Configure service
		builder
			.Services
			.AddOptions<EmaCrossoverSimpleOptions>()
			.Configure(options =>
			{
				options.FasterIndicatorEmaCount = 5;
				options.SlowerIndicatorEmaCount = 20;
				options.TickerInformation = new TickerInformation
				{
					DefaultUnitSize = 10
				};
			});


		var services = builder.Services.BuildServiceProvider();
		Dictionary<string, IList<OlhcvDatapoint>> screenedDatapoints = new ()
		{
			{"DEFAULT_EXCHANGE:SAMPLE_DATA", services.GetService<IList<OlhcvDatapoint>>()!}
		};

		//services.GetService<IFlow>()!.Backtest(
		//	screenedDatapoints,
		//	out IList<CallResponse> decisionList,
		//	out IList<object> backtestResults);

		var jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			Converters =
			{
				new JsonStringEnumConverter()
			}
		};

		//string decisionListJson = JsonSerializer.Serialize(decisionList, jsonOptions);
		//string backtestResultsJson = JsonSerializer.Serialize(backtestResults, jsonOptions);

		string resultsDir = builder
			.Configuration
			.GetValue<string>("ResultsDirectory") ??
		                    Path.GetTempPath();
		var resultsPrefix = Path
			.Combine(resultsDir, DateTime.Now.ToString("yyyy-MM-ddTHH-mm"));
		Directory.CreateDirectory(resultsPrefix);

		//File.WriteAllText(Path.Combine(resultsPrefix, "decisionList.json"), decisionListJson);
		//File.WriteAllText(Path.Combine(resultsPrefix, "backtestResults.json"), backtestResultsJson);
	}
}
