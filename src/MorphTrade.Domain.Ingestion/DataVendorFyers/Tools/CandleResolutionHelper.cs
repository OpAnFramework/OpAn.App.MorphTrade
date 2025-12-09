namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers.Tools;

/// <summary>
/// Candle resolution helper tool.
/// </summary>
public static class CandleResolutionHelper
{
	private static readonly Dictionary<TimeSpan, string?> _map = new()
	{
		{ TimeSpan.FromDays(1), "1D" },

		{ TimeSpan.FromSeconds(5),  "5S" },
		{ TimeSpan.FromSeconds(10), "10S" },
		{ TimeSpan.FromSeconds(15), "15S" },
		{ TimeSpan.FromSeconds(30), "30S" },
		{ TimeSpan.FromSeconds(45), "45S" },

		{ TimeSpan.FromMinutes(1),   "1" },
		{ TimeSpan.FromMinutes(2),   "2" },
		{ TimeSpan.FromMinutes(3),   "3" },
		{ TimeSpan.FromMinutes(5),   "5" },
		{ TimeSpan.FromMinutes(10),  "10" },
		{ TimeSpan.FromMinutes(15),  "15" },
		{ TimeSpan.FromMinutes(20),  "20" },
		{ TimeSpan.FromMinutes(30),  "30" },
		{ TimeSpan.FromMinutes(60),  "60" },
		{ TimeSpan.FromMinutes(120), "120" },
		{ TimeSpan.FromMinutes(240), "240" },
	};

	/// <summary>
	/// Provides closes candle resolution.
	/// </summary>
	/// <param name="input">Timespan to be converted.</param>
	/// <returns>String containing candle resolution candidate.</returns>
	/// <exception cref="ArgumentException"></exception>
	public static string? ToResolutionString(TimeSpan input)
	{
		if (_map.TryGetValue(input, out string? result))
			return result;

		throw new ArgumentException(
			$"Unsupported TimeSpan: {input}. No matching candle resolution found.");
	}
}
