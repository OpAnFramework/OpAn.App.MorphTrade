using System.Text.Json.Serialization;

namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Dto;

/// <summary>
/// An alpaca data point model.
/// </summary>
/// <see href="https://docs.alpaca.markets/reference/stockbars"/>
public class AlpacaBarDataPoint
{
	/// <summary>
	/// Closing value.
	/// </summary>
	[JsonPropertyName("c")]
	public double Close { get; set; }

	/// <summary>
	/// Opening value.
	/// </summary>
	[JsonPropertyName("o")]
	public double Open { get; set; }

	/// <summary>
	/// Low value.
	/// </summary>
	[JsonPropertyName("l")]
	public double Low { get; set; }

	/// <summary>
	/// High value.
	/// </summary>
	[JsonPropertyName("h")]
	public double High { get; set; }

	/// <summary>
	/// Trade count.
	/// </summary>
	[JsonPropertyName("n")]
	public Int64 TradeCount { get; set; }

	/// <summary>
	/// Timestamp in RFC-3339.
	/// </summary>
	[JsonPropertyName("t")]
	public string? Timestamp { get; set; }

	/// <summary>
	/// Bar volume.
	/// </summary>
	[JsonPropertyName("v")]
	public Int64 Volume { get; set; }

	/// <summary>
	/// Volume weighted average price.
	/// </summary>
	[JsonPropertyName("vw")]
	public double WeightedAverageVolume {get; set;}
}
