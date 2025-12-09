namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers.Auth;

/// <summary>
/// Options provided for the fyers Auth API calls.
/// </summary>
public class FyersAuthenticatorOptions
{
	/// <summary>
	/// ClientId for the Fyers authentication
	/// </summary>
	public string? ClientId { get; set; }

	/// <summary>
	/// ClientId for the Fyers authentication
	/// </summary>
	public string? SecretKey { get; set; }

	/// <summary>
	/// ClientId for the Fyers authentication
	/// </summary>
	public string? RedirectUri { get; set; }

	/// <summary>
	/// ClientId for the Fyers authentication
	/// </summary>
	public string? AuthCode { get; set; }

	/// <summary>
	/// Timeout for the authorization routine.
	/// </summary>
	public TimeSpan AuthorizationTimeout { get; set; } = TimeSpan.FromSeconds(10);
}
