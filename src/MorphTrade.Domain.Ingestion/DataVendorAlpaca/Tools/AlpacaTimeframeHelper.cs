namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Tools;
/// <summary>
/// Helps with the timeframe conversions for the platform Alpaca.
/// </summary>
public class AlpacaTimeframeHelper
{
	/// <summary>
	/// Timespan to Alpaca timeframe conversion helper.
	/// </summary>
	/// <param name="timespan">Timespan to be converted.</param>
	/// <returns>String in standard with Alpaca vendor.</returns>
	/// <exception cref="ArgumentException">If timespan fails to be converted.</exception>
	public string ToAlpacaTimeframe(TimeSpan timespan)
	{
		// Minutes (1–59)
		if (timespan.TotalMinutes >= 1 && timespan.TotalMinutes < 60 && timespan.Seconds == 0 && timespan.Hours == 0)
		{
			int m = (int)timespan.TotalMinutes;
			return m + (m <= 59 ? "Min" : "");
		}

		// Hours (1–23)
		if (timespan.TotalHours >= 1 && timespan.TotalHours < 24 && timespan.Minutes == 0)
		{
			int h = (int)timespan.TotalHours;
			return h + "H";
		}

		// Days (must be exactly 1 day)
		if (timespan.TotalDays == 1)
			return "1D";

		// Weeks (7 days, 14 days, ...)
		if (timespan.TotalDays % 7 == 0)
		{
			int weeks = (int)(timespan.TotalDays / 7);
			return weeks + "W";
		}

		// Months (mapped from days, approximate ranges)
		// Only allow: 1,2,3,4,6,12 Month timeframes
		int months = timespan.Days / 30;
		int[] allowed = { 1, 2, 3, 4, 6, 12 };

		if (allowed.Contains(months) && timespan.Days == months * 30)
		{
			return months + "M";
		}

		throw new ArgumentException($"Unsupported timeframe: {timespan}");
	}
}
