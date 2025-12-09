namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers.Auth;

/// <summary>
/// Fyers credentials for runtime.
/// </summary>
public class FyersCredentials: IFyersCredentials
{
	private string? _accessToken;
	private string? _refreshToken;
	private readonly IFyersAuthenticator _fyersAuthenticator;

	/// <summary>
	/// Credentials constructor for fyers.
	/// </summary>
	/// <param name="fyersAuthenticator">Authenticator for Fyers api.</param>
	public FyersCredentials(
			IFyersAuthenticator fyersAuthenticator
		)
	{
		_fyersAuthenticator = fyersAuthenticator;
		SetAccessToken().Wait();
	}

	private async Task SetAccessToken()
	{
		FyersAuthenticatorTokens? tokensSet = await _fyersAuthenticator.GenerateAccessToken();
		if (tokensSet is null)
		{
			throw new NullReferenceException("The authenticator tokens were null. " +
			                                 "Please verify your appsettings.json file." +
			                                 "And also make sure the CLI command consists of --auth code.");
		}

		_accessToken = tokensSet?.AccessToken;
		_refreshToken = tokensSet?.RefreshToken;
	}

	/// <inheritdoc />
	public string GetAccessToken() => _accessToken!;

	/// <inheritdoc />
	public string GetRefreshToken() => _refreshToken!;
}
