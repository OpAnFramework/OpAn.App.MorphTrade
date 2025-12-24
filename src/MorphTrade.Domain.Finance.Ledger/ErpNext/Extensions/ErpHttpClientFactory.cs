using Microsoft.Extensions.Options;

namespace OpAn.App.MorphTrade.Domain.Finance.Ledger.ErpNext.Extensions;

/// <summary>
/// A factory class to produce ErpHttpClient
/// </summary>
public class ErpHttpClientFactory: IErpHttpClientFactory
{
	/// <summary>
	/// Constructor for the ErpHttpClientFactory.
	/// </summary>
	/// <param name="options">Options for the ERP.</param>
	public ErpHttpClientFactory(
			IOptions<ErpOptions> options
		)
	{
		var backendProtocol = options.Value.BackendProtocol;
		var backendHost = options.Value.BackendHost;
		var backendPort = options.Value.BackendPort;
		var backendBaseEndpoint = options.Value.BackendBaseEndpoint;
		var httpClient = new HttpClient
		{
			BaseAddress = new Uri($"{backendProtocol}://{backendHost}:{backendPort}/{backendBaseEndpoint}")
		};

		// TODO: Perform Auth level options here (if needed).

		GetClient = httpClient;
	}

	/// <inheritdoc/>
	public HttpClient GetClient { get; }
}
