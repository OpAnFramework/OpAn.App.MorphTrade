using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpAn.App.MorphTrade.Domain.Finance.Bank.AggregateRoot;
using OpAn.App.MorphTrade.Domain.Finance.Bank.Enums;
using OpAn.App.MorphTrade.Domain.Finance.Bank.Repositories;
using OpAn.App.MorphTrade.Infrastructure.Persistence.Extension;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.Tests;

/// <summary>
/// Performs database action tests.
/// </summary>
[TestClass]
public class TestPersistentAccountDatabase
{
	private string? _tempPath;
	private const string DbName = "test_database.db";
	private readonly string _configName = "appsettings.test.json";

	/// <summary>
	/// Test initializations.
	/// </summary>
	[TestInitialize]
	public void Setup()
	{
		_tempPath = Path
			.Combine(Path.GetTempPath(), $"MorphTrade_{Guid.NewGuid()}");
		// create a temp directory
		Directory.CreateDirectory(_tempPath);

		string combinedPath = Path.Combine(_tempPath, DbName);
		// setup default application configuration
		var config = new
		{
			ConnectionStrings = new
			{
				DefaultDbConnection = $"Data Source={combinedPath}"
			}
		};

		var appSettingsJson = JsonSerializer.Serialize(config);
		File.WriteAllText(Path.Combine(_tempPath, _configName), appSettingsJson);
	}

	/// <summary>
	/// Database entry creation unit tests with Account Repository.
	/// </summary>
	[TestMethod]
	public async Task Test_IfDatabaseEntriesAreCreatedWithAccountRepository()
	{
		ConfigurationManager configuration = new ConfigurationManager();
		configuration.Sources.Clear();
		configuration.AddJsonFile(Path.Combine(_tempPath!, _configName));

		ServiceCollection servicesContainer = new ServiceCollection();
		servicesContainer.AddLogging();
		servicesContainer
			.AddPersistenceDb(configuration)
			.AddBankingDomain();

		var services = servicesContainer.BuildServiceProvider();
		await services.GetRequiredService<BankDbContext>()
			.Database.EnsureCreatedAsync();

		IAccountRepository? accountRepository = services.GetService<IAccountRepository>();

		Assert.IsNotNull(accountRepository);

		string trackingId = Guid.NewGuid().ToString();
		Account testAccount = new Account()
		{
			Id = trackingId,
			AccountName = $"AccountName_{trackingId}",
			Funds = 50000,
			Currency = CurrencyType.Euro
		};

		await accountRepository.Add(testAccount);

		Account? retrievedAccount = await accountRepository.GetByIdAsync(trackingId);
		Assert.IsNotNull(retrievedAccount);
		Assert.AreEqual(trackingId, retrievedAccount.Id);

		// Perform transactions and check if transactions are updated.
		retrievedAccount.Credit(2345, "TestTransaction");
		await accountRepository.Update(retrievedAccount);
		var richRetrievedAccount = await accountRepository.GetByIdAsync(trackingId);
		Assert.IsNotNull(richRetrievedAccount);
		Assert.AreEqual(52345, richRetrievedAccount.Funds);

		// Ensures that the account creation process does not add faulty auto-generations.
		Assert.AreEqual(CurrencyType.Euro, richRetrievedAccount.Currency);

		// Ensures that the same currency transactions are maintained.
		Assert.IsTrue(richRetrievedAccount.Transactions.All(t => t.Currency == CurrencyType.Euro));
	}

	/// <summary>
	/// Cleanup routine for the tests.
	/// </summary>
	[TestCleanup]
	public void Cleanup()
	{
		Directory.Delete(_tempPath!, true);
	}
}
