using Microsoft.Extensions.Options;
using OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Http.options;

namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Http;

/// <summary>
/// Wrapper of HttpClient for Alpaca.
/// </summary>
public class AlpacaHttpClientFactory: IAlpacaHttpClientFactory
{
	/// <summary>
	/// Constructor for Alpaca HttpClient wrapper.
	/// </summary>
	/// <param name="options">Options for the Alpaca data vendor.</param>
	public AlpacaHttpClientFactory(
			IOptions<AlpacaDataVendorOptions> options
		)
	{
		var client = new HttpClient
		{
			BaseAddress = new Uri(options.Value.BaseAddress)
		};

		client.DefaultRequestHeaders.Add("APCA-API-KEY-ID", options.Value.ApiKeyId);
		client.DefaultRequestHeaders.Add("APCA-API-SECRET-KEY", options.Value.ApiKeySecret);
		GetClient = client;
	}

	/// <inheritdoc />
	public HttpClient GetClient { get; }
}
