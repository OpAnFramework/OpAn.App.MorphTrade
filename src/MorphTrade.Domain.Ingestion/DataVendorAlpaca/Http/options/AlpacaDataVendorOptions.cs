namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Http.options;

/// <summary>
/// Options set for Alpaca on the appsettings.json
/// </summary>
public class AlpacaDataVendorOptions
{
	/// <summary>
	/// Base address for the Alpaca API.
	/// </summary>
	public required string BaseAddress { get; set; }

	/// <summary>
	/// API key ID for the Alpaca data API.
	/// </summary>
	public required string ApiKeyId { get; set; }

	/// <summary>
	/// API key secret for the Alpaca data API.
	/// </summary>
	public required string ApiKeySecret { get; set; }
}
