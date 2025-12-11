using Microsoft.Extensions.DependencyInjection;
using OpAn.App.MorphTrade.Domain.Finance.Bank.Repositories;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank;

/// <summary>
/// Extension class for banking domain.
/// </summary>
public static class BankDomainExtension
{
	/// <summary>
	/// Allows adding banking domain to the service container.
	/// </summary>
	/// <param name="services">Service container to be extended.</param>
	/// <returns>Extended service container.</returns>
	public static IServiceCollection AddBankingDomain(this IServiceCollection services)
	{
		services.AddScoped<IAccountRepository, AccountRepository>();
		return services;
	}
}
