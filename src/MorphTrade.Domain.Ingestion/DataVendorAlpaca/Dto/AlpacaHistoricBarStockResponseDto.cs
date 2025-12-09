using System.Text.Json.Serialization;

namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Dto;

/// <summary>
/// Response DTO for the Alpaca historic data for stocks/bar endpoint.
/// </summary>
public class AlpacaHistoricBarStockResponseDto
{
	/// <summary>
	/// Bars values.
	/// </summary>
	[JsonPropertyName("bars")]
	public Dictionary<string, IList<AlpacaBarDataPoint>>? Bars { get; set; }

	/// <summary>
	/// Next page token.
	/// </summary>
	[JsonPropertyName("next_page_token")]
	public string? NextPageToken { get; set; }
}
