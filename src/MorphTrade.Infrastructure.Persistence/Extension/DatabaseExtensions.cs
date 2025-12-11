using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpAn.App.MorphTrade.Domain.Finance.Bank;

namespace OpAn.App.MorphTrade.Infrastructure.Persistence.Extension;

/// <summary>
/// Database extension methods.
/// </summary>
public static class DatabaseExtensions
{
	/// <summary>
	/// Allows adding database to the application builder.
	/// </summary>
	/// <param name="services">Service collection to be extended.</param>
	/// <param name="configuration">Configuration to be taken account for.</param>
	/// <param name="assemblies">Assemblies to be added for migrations.</param>
	/// <returns>Extended service container.</returns>
	public static IServiceCollection AddPersistenceDb(
		this IServiceCollection services,
		IConfiguration configuration,
		IEnumerable<Assembly>? assemblies = null)
	{
		// Adds database from the connection string.
		services.AddDbContext<BankDbContext>(options =>
			{
				if (assemblies != null)
				{
					options
						.UseSqlite(
							configuration.GetConnectionString("DefaultDbConnection"),
							sqliteOptions =>
							{
								foreach (Assembly assembly in assemblies.ToList())
								{
									sqliteOptions.MigrationsAssembly(assembly);
								}
							});
				}
				else
				{
					options.UseSqlite(
						configuration.GetConnectionString("DefaultDbConnection"));
				}
			});
		return services;
	}
}
