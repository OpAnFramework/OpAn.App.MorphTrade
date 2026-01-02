using Microsoft.Extensions.DependencyInjection;
using OpAn.App.MorphTrade.Domain.Flows.Tests.Fixtures;
using Microsoft.Extensions.Configuration;
using OpAn.App.MorphTrade.Infrastructure.Persistence.Extension;

namespace OpAn.App.MorphTrade.Domain.Flows.Tests;

/// <summary>
/// Persistent database tests.
/// </summary>
[TestClass]
public class TestPersistentDatabase
{
	private readonly IServiceProvider _globalServices;

	/// <summary>
	/// Constructor for the persistent database tests.
	/// </summary>
	public TestPersistentDatabase()
	{
		IServiceCollection services = new ServiceCollection();
		services.AddSingleton<DatabaseFixtures>();
		_globalServices = services.BuildServiceProvider();
	}

	/// <summary>
	/// Initializes the tests.
	/// </summary>
	[TestInitialize]
	public async Task Initialize()
	{
		await _globalServices.GetRequiredService<DatabaseFixtures>()
			.InitAsync();
	}

	/// <summary>
	/// Checks if the database connection is settable.
	/// </summary>
	[TestMethod]
	public async Task Test_IfDatabaseConnects()
	{
		var fixture = _globalServices.GetRequiredService<DatabaseFixtures>();
		var (config, port) = fixture.GetRuntimePlugs();

		Assert.IsNotNull(config);
		Assert.IsNotNull(port);

		string connectionString
			= $"Host=localhost;Port={port};Database={config.Database};" +
			  $"Username={config.Username};Password={config.Password};Pooling=false";

		var services = new ServiceCollection();

		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>
			{
				["ConnectionStrings:FlowManagementDbConnection"] = connectionString
			})
			.Build();

		services.AddPersistenceDb(configuration);

		var serviceProvider = services.BuildServiceProvider();
		var dbContext = serviceProvider.GetRequiredService<FlowsDbContext>();

		bool canConnect = await dbContext.Database.CanConnectAsync();
		Assert.IsTrue(canConnect, "Should be able to connect to the database container.");
	}

	/// <summary>
	/// Cleans up the database.
	/// </summary>
	[TestCleanup]
	public async Task Cleanup()
	{
		await _globalServices.GetRequiredService<DatabaseFixtures>()
			.DisposeAsync();
	}
}
