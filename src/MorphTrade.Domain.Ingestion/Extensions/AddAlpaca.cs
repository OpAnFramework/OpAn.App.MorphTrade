using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Http;
using OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Http.options;
using OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Tools;

namespace OpAn.App.MorphTrade.Domain.Ingestion.Extensions;

/// <summary>
/// Adds the Alpaca services to the host application builder.
/// </summary>
public static class AddAlpaca
{
	/// <summary>
	/// Adds the services needed for Alpaca data vendor.
	/// </summary>
	/// <param name="builder">Host application builder to be configured.</param>
	/// <returns>Extended host application builder.</returns>
	public static HostApplicationBuilder AddAlpacaExtensions(this HostApplicationBuilder builder)
	{
		AlpacaDataVendorOptions? dataVendorOptions = builder
			.Configuration
			.GetSection(nameof(AlpacaDataVendorOptions))
			.Get<AlpacaDataVendorOptions>();

		if (dataVendorOptions is not null)
		{
			builder
				.Services
				.AddSingleton<IAlpacaHttpClientWrapper, AlpacaHttpClientWrapper>()
				.Configure<AlpacaDataVendorOptions>(options =>
					{
						options.BaseAddress = dataVendorOptions.BaseAddress;
						options.ApiKeyId = dataVendorOptions.ApiKeyId;
						options.ApiKeySecret = dataVendorOptions.ApiKeySecret;
					}
				)
				.AddSingleton<IDataVendor, DataVendorAlpaca.DataVendorAlpaca>()
				.AddSingleton<AlpacaTimeframeHelper>();
		}

		return builder;
	}
}
