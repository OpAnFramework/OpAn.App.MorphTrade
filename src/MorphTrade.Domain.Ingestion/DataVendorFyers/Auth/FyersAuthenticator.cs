using System.Text.Json;
using FyersCSharpSDK;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers.Auth;

/// <summary>
/// Authenticator for Fyers API.
/// </summary>
public class FyersAuthenticator: IFyersAuthenticator
{
	private readonly FyersAuthenticatorOptions _options;

	/// <summary>
	/// Constructor for the Fyers Authenticator.
	/// </summary>
	/// <param name="options">Options provided with configurations.</param>
	public FyersAuthenticator(IOptions<FyersAuthenticatorOptions> options)
	{
		_options = options.Value;
	}

	/// <inheritdoc />
	public void GenerateAuthCode()
	{
		try
		{
			FyersClass
				.Instance
				.GetGenerateCode(
					_options.ClientId,
					_options.SecretKey,
					_options.RedirectUri
				);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
		Task.Delay(_options.AuthorizationTimeout).Wait();
	}

	/// <inheritdoc />
	public async Task<FyersAuthenticatorTokens?> GenerateAccessToken()
	{
		JObject intermediateJobject = await FyersClass
			.Instance
			.GenerateToken(
				_options.SecretKey,
				_options.RedirectUri,
				_options.AuthCode,
				Utility.GenerateAppHashID(
					_options.ClientId, _options.SecretKey
				)
			);
		return
			JsonSerializer.Deserialize<FyersAuthenticatorTokens>(intermediateJobject.ToString());
	}

	/// <inheritdoc />
	public void Dispose()
	{
		// FyersClass.Instance is self-managed.
	}
}
