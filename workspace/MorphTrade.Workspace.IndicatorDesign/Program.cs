// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Abstractions.Tools.JsonConverters;
using OpAn.App.MorphTrade.Workspace.IndicatorDesign.Abstractions;

namespace OpAn.App.IndicatorDesign;

internal class Program
{
	public static void Main(string[] args)
	{
		var data = File.ReadAllText("data.json");

		var options = new JsonSerializerOptions
		{
			Converters = { new UnixSecondsToDateTimeConverter() }
		};
		var olhcvDatapoints = JsonSerializer.Deserialize<IList<OlhcvDatapoint>>(data, options);

		IList<MorphQuote> quotes = olhcvDatapoints!.Select(x => new MorphQuote
		{
			Date = x.Timestamp,
			Open = (decimal) x.Open,
			High = (decimal) x.High,
			Low = (decimal) x.Low,
			Volume = (decimal) x.Volume,
			Close = (decimal) x.Close
		}).ToList();

		Console.WriteLine("Hello World!");
	}
}
