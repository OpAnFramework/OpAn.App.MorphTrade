using JetBrains.Annotations;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;

namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers.Auth;

/// <summary>
/// Authenticates with the Fyers API.
/// </summary>
[PublicAPI]
public interface IFyersAuthenticator: IAuthenticator
{
	/// <summary>
	/// Adds auth code to the fyers class instance.
	/// </summary>
	public void GenerateAuthCode();

	/// <summary>
	/// Generates Access Token.
	/// </summary>
	/// <returns>Task of the Token objects consisting of access token, refresh token and status code.</returns>
	public Task<FyersAuthenticatorTokens?> GenerateAccessToken();
}
