using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Domain.Finance.Trader;
using OpAn.App.MorphTrade.Domain.Flows.Flows;
using OpAn.App.MorphTrade.Infrastructure.Persistence.Extension;
using Testcontainers.PostgreSql;

namespace OpAn.App.MorphTrade.Domain.Flows.Tests.Fixtures;

/// <summary>
/// Allows adding dependencies as a fixture.
/// </summary>
public static class StaticDependencyCreator
{
	/// <summary>
	/// Allows adding flow domain dependencies.
	/// </summary>
	/// <param name="serviceContainer"></param>
	/// <param name="pgsqlConfig"></param>
	/// <param name="pgsqlPort"></param>
	/// <returns></returns>
	public static IServiceCollection AddFlowDomainDependencies(
		this IServiceCollection serviceContainer,
		PostgreSqlConfiguration pgsqlConfig,
		int pgsqlPort)
	{
		string connectionString
			= $"Host=localhost;Port={pgsqlPort};Database={pgsqlConfig.Database};" +
			  $"Username={pgsqlConfig.Username};Password={pgsqlConfig.Password};Pooling=false";

		var services = new ServiceCollection();
		services.AddLogging();

		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>
			{
				["ConnectionStrings:FlowManagementDbConnection"] = connectionString
			})
			.AddJsonFile("flowsmanageroptions.json")
			.Build();

		// Add IDataVendor and ITrader mocks
		//		Mock for the ITrader is the NullTrader
		services.AddSingleton<ITrader, NullTrader>();
		var dataVendorMock = new Mock<IDataVendor>();

		// TODO: Mock DataVendor behaviour
		services.AddSingleton(dataVendorMock.Object);
		services.AddSingleton<FlowsManager>();

		services
			.AddPersistenceDb(configuration)
			// Add Flows domain
			.AddFlowsDomain(configuration);

		services.BuildServiceProvider()
			.GetRequiredService<FlowsDbContext>()
			.Database
			.EnsureCreated();
		return services;
	}
}
