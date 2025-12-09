using System.Text.Json.Serialization;

namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers.Auth;

/// <summary>
/// Tokens object containing access and refresh tokens.
/// </summary>
public class FyersAuthenticatorTokens
{
	/// <summary>
	/// Access token provided by Fyers auth api.
	/// </summary>
	[JsonPropertyName("TOKEN")]
	public string? AccessToken { get; set; }

	/// <summary>
	/// Refresh token provided by Fyers auth api.
	/// </summary>
	[JsonPropertyName("refresh_token")]
	public string? RefreshToken { get; set; }

	/// <summary>
	/// Response message from Fyers auth api.
	/// </summary>
	[JsonPropertyName("RESPONSE_MESSAGE")]
	public string? ResponseMessage { get; set; }
}
