using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers.Auth;

namespace OpAn.App.MorphTrade.Domain.Ingestion.Extensions;

/// <summary>
/// Adds extension methods for Data ingestion domain.
/// </summary>
public static class AddFyers
{

	/// <summary>
	/// Adds and extends the service container for the correct data ingestion routines.
	/// </summary>
	/// <param name="builder">Application builder to be extended.</param>
	/// <returns>The extended Application Builder.</returns>
	/// <exception cref="InvalidOperationException"></exception>
	private static HostApplicationBuilder AddFyersExtensions(this HostApplicationBuilder builder)
	{
		IConfiguration config = builder.Configuration;

		FyersAuthenticatorOptions? authenticatorOptions = config
			.GetSection("FyersAuthenticatorOptions")
			.Get<FyersAuthenticatorOptions>();

		if (authenticatorOptions is null)
		{
			throw new InvalidOperationException("Fyers authenticator options not found");
		}

		if (authenticatorOptions.AuthCode is not null)
		{
			return builder.AddFyersExtensions(authenticatorOptions.AuthCode);
		}

		builder.Services
			.AddSingleton<IFyersAuthenticator, FyersAuthenticator>()
			.Configure<FyersAuthenticatorOptions>(
				options =>
				{
					options.RedirectUri = authenticatorOptions.RedirectUri;
					options.ClientId = authenticatorOptions.ClientId;
					options.SecretKey = authenticatorOptions.SecretKey;
				});

		return builder;
	}

	/// <summary>
	/// Adds and extends the service container for the correct data ingestion routines.
	/// </summary>
	/// <param name="builder">Application builder to be extended.</param>
	/// <param name="authCode">Auth code provided by the builder extension caller.</param>
	/// <returns>The extended Application Builder.</returns>
	/// <exception cref="InvalidOperationException"></exception>
	private static HostApplicationBuilder AddFyersExtensions(
		this HostApplicationBuilder builder,
		string authCode)
	{
		IConfiguration config = builder.Configuration;

		FyersAuthenticatorOptions? authenticatorOptions = config
			.GetSection("FyersAuthenticatorOptions")
			.Get<FyersAuthenticatorOptions>();

		if (authenticatorOptions == null)
		{
			throw new InvalidOperationException("Fyers authenticator options not found");
		}

		builder.Services
			.AddSingleton<IFyersAuthenticator, FyersAuthenticator>()
			.Configure<FyersAuthenticatorOptions>(
					options =>
					{
						options.AuthCode = authCode;
						options.RedirectUri = authenticatorOptions.RedirectUri;
						options.ClientId = authenticatorOptions.ClientId;
						options.SecretKey = authenticatorOptions.SecretKey;
					});

		builder.Services.AddSingleton<IFyersCredentials, FyersCredentials>();

		builder.Services.AddSingleton<IDataVendor, DataVendorFyers.DataVendorFyers>();

		return builder;
	}

	/// <summary>
	/// Adds Fyers as the data vendor.
	/// </summary>
	/// <param name="builder">Host application builder to add the data vendor to.</param>
	/// <returns></returns>
	public static HostApplicationBuilder? AddFyersDataVendor(this HostApplicationBuilder builder)
	{
		string? authCode = builder.Configuration["auth"];

		// Add Data Ingestion dependencies
		if (authCode is null)
		{
			builder
				.AddFyersExtensions()
				.Services
				.BuildServiceProvider()
				.GetRequiredService<IFyersAuthenticator>()
				.GenerateAuthCode();
			return null;
		}
		builder.AddFyersExtensions(authCode);
		return builder;
	}
}
