using Microsoft.Extensions.DependencyInjection;

namespace OpAn.App.MorphTrade.Domain.Finance.Ledger.ErpNext.Extensions;

/// <summary>
/// Allows adding ERPNext platform to the builder sdk.
/// </summary>
public static class ErpNextExtensions
{
	/// <summary>
	/// Allows extending the service container
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection AddErpNextPlatform(this IServiceCollection services)
	{
		return services;
	}
}
