using OpAn.App.MorphTrade.Abstractions.Http;

namespace OpAn.App.MorphTrade.Domain.Finance.Ledger.ErpNext.Extensions;

/// <summary>
/// A helper interface for clean dependency injection of ErpHttpClientFactory.
/// </summary>
public interface IErpHttpClientFactory : IHttpClientFactory;
