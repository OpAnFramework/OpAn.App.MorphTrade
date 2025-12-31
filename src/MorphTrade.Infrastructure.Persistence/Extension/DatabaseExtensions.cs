using System.Reflection;
using CmdScale.EntityFrameworkCore.TimescaleDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpAn.App.MorphTrade.Domain.Finance.Bank;
using OpAn.App.MorphTrade.Domain.Flows;

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
	/// <returns>Extended service container.</returns>
	public static IServiceCollection AddPersistenceDb(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		// Adds database from the connection string.
		// Add migrations for the Banking domain.
		services.AddDbContext<BankDbContext>(options =>
			{
				options
					.UseSqlite(
						configuration.GetConnectionString("DefaultDbConnection"),
						sqliteOptions =>
						{
							sqliteOptions.MigrationsAssembly(typeof(IBankingDomainAssemblyMarker)
								.Assembly.GetName().Name!);
						});
			});

		// Add the flow management persistence.
		string? flowManagementConnectionString = configuration.GetConnectionString("DefaultFlowManagementConnection");
		if (!string.IsNullOrEmpty(flowManagementConnectionString))
		{
			services.AddDbContext<FlowsDbContext>(options =>
			{
				options
					.UseNpgsql(
						configuration.GetConnectionString("FlowManagementDbConnection"),
						npgsqlOptions =>
						{
							npgsqlOptions.MigrationsAssembly(
								typeof(IFlowsAssemblyMarker).Assembly.GetName().Name!
							);
						})
					.UseTimescaleDb();
			});
		}
		return services;
	}
}
