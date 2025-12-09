namespace OpAn.App.MorphTrade.Abstractions.Http;

/// <summary>
/// An http client wrapper for precise dependency injection.
/// </summary>
public interface IHttpClientWrapper
{
	/// <summary>
	/// Provides default http client for the wrapped entity.
	/// </summary>
	public HttpClient GetClient { get; }
}
