using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Domain.Flows.Entities;
using OpAn.App.MorphTrade.Domain.Flows.Extensions;
using OpAn.App.MorphTrade.Domain.Flows.Repositories;
using OpAn.App.MorphTrade.Domain.Flows.Tests.Fixtures;
using OpAn.App.MorphTrade.Infrastructure.Persistence.Extension;

namespace OpAn.App.MorphTrade.Domain.Flows.Tests;

/// <summary>
/// Tests for flows repository behavioural consistencies.
/// </summary>
[TestClass]
public class TestFlowsRepositoryConsistencies
{
	private readonly IServiceProvider _globalServices;
	private const string TestFlowName = "TestFlow";
	private const string TestFlowId = "TestFlowId";
	private const string TestUserId = "TestUserId";

	private const string TestTickerSymbol = "TickerSymbol";
	private const decimal TestTickerPrice = 100;
	private const decimal TestTickerVolume = 100;

	/// <summary>
	/// Constructor for the persistent database tests.
	/// </summary>
	public TestFlowsRepositoryConsistencies()
	{
		IServiceCollection services = new ServiceCollection();
		services.AddSingleton<DatabaseFixtures>();
		_globalServices = services.BuildServiceProvider();
	}

	private IServiceCollection CreateDependencies()
	{
		var fixture = _globalServices.GetRequiredService<DatabaseFixtures>();
		var (config, port) = fixture.GetRuntimePlugs();

		Assert.IsNotNull(config);
		Assert.IsNotNull(port);

		string connectionString
			= $"Host=localhost;Port={port};Database={config.Database};" +
			  $"Username={config.Username};Password={config.Password};Pooling=false";

		var services = new ServiceCollection();
		services.AddLogging();

		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>
			{
				["ConnectionStrings:FlowManagementDbConnection"] = connectionString
			})
			.Build();

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
	/// Tests for following contracted methods:
	///		- CreateFlowAsync
	///		- GetFlowByIdAsync
	///		- GetFlowsByNameAsync
	/// </summary>
	/// <returns></returns>
	[TestMethod]
	public async Task Test_IfFlowCanBeCreated()
	{
		var services = CreateDependencies();
		var serviceContainer = services.BuildServiceProvider();
		IFlowsRepository flowsRepository = serviceContainer.GetRequiredService<IFlowsRepository>();
		Assert.IsNotNull(flowsRepository);

		// Create a new Flow and assert its existence
		FlowMeta testFlowMetadata = new FlowMeta
		{
			UserId = TestUserId,
			Name = TestFlowName,
			Id = TestFlowId
		};
		await flowsRepository.CreateFlowAsync(testFlowMetadata);
		Assert.AreEqual(
			testFlowMetadata,
			await flowsRepository.GetFlowByIdAsync(testFlowMetadata.Id));
		Assert.AreEqual(
			testFlowMetadata,
			(await flowsRepository.GetFlowsByNameAsync(testFlowMetadata.Name))!.FirstOrDefault());
	}

	/// <summary>
	/// Checks if after a flow creation, could one add the Call Response Events.
	/// </summary>
	[TestMethod]
	public async Task Test_IfCallResponseEventsCanBeAdded()
	{
		await Test_IfFlowCanBeCreated();

		var serviceContainer = CreateDependencies().BuildServiceProvider();
		IFlowsRepository flowsRepository = serviceContainer.GetRequiredService<IFlowsRepository>();

		FlowMeta? registeredFlow = await flowsRepository.GetFlowByIdAsync(TestFlowId);
		Assert.IsNotNull(registeredFlow);

		CallResponse testCallResponse = new CallResponse
		{
			Timestamp = 0,
			Ticker = new Ticker
			{
				Symbol = TestTickerSymbol,
				Price = TestTickerPrice,
				Quantity = 0,
				Volume = TestTickerVolume,
				Timestamp = DateTime.UtcNow
			},
			TradeCall = TradeCall.Long,
			TradeCallInfo = new TradeCallInfo(TradeCall.Long, 5, 2, 10)
		};

		CallResponseEvent? crEvent = await flowsRepository.CreateUpdateCallResponseEventAsync(
			registeredFlow, testCallResponse);
		Assert.IsNotNull(crEvent);
		IList<CallResponseEvent>? crEvents = await flowsRepository.GetAllCallResponseEventsAsync(registeredFlow);
		Assert.IsNotNull(crEvents);
		Assert.AreEqual(1, crEvents.Count);
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
