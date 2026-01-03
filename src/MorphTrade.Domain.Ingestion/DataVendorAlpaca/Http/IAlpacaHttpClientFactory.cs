using OpAn.App.MorphTrade.Abstractions.Http;
using IHttpClientFactory = OpAn.App.MorphTrade.Abstractions.Http.IHttpClientFactory;

namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Http;

/// <summary>
/// Contract for Alpaca HttpClient.
/// </summary>
public interface IAlpacaHttpClientFactory : IHttpClientFactory;
