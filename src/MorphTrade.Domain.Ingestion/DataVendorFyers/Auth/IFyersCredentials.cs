using OpAn.App.MorphTrade.Abstractions.IngestionDomain;

namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers.Auth;

/// <summary>
/// Credentials provided by the Fyers auth api.
/// </summary>
public interface IFyersCredentials: ICredentials
{
	/// <summary>
	/// Provides access token created by the application.
	/// </summary>
	/// <returns>Access token in string.</returns>
	public string GetAccessToken();

	/// <summary>
	/// Provides refresh token created by the application.
	/// </summary>
	/// <returns>Refresh token in string.</returns>
	public string GetRefreshToken();
}
