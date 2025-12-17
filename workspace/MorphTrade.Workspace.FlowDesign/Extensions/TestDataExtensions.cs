using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Abstractions.Tools.JsonConverters;

namespace OpAn.App.MorphTrade.Workspace.FlowDesign.Extensions;

/// <summary>
/// Allows adding test data in the service container.
/// </summary>
public static class TestDataExtensions
{
	/// <summary>
	/// Adds test data.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="configuration"></param>
	/// <returns></returns>
	public static IServiceCollection AddTestData(this IServiceCollection services, IConfiguration configuration)
	{
		TestDataOptions options = configuration
			.GetSection(nameof(TestDataOptions))
			.Get<TestDataOptions>() ?? new TestDataOptions();

		// Add mock test data to the service container
		var data = File.ReadAllText(options.TestDataJsonAbsolutePath);
		var jsonSerializerOptions = new JsonSerializerOptions
		{
			Converters = { new UnixSecondsToDateTimeConverter() }
		};
		IList<OlhcvDatapoint>? olhcvDatapoints = JsonSerializer
			.Deserialize<IList<OlhcvDatapoint>>(data, jsonSerializerOptions);

		services.AddSingleton<IList<OlhcvDatapoint>>(_ => olhcvDatapoints!);
		return services;
	}
}
