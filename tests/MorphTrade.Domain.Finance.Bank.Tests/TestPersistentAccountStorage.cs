using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.Tests;

/// <summary>
/// Test class for Account storage on a persistent database.
/// </summary>
[TestClass]
public class TestPersistentAccountStorage
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
	///	Checks if the appsettings are correctly built.
	/// </summary>
	[TestMethod]
	public void Test_IfAppsettingsAreCorrectlyCreated()
	{
		Assert.IsNotNull(_tempPath);
		Assert.IsTrue(File.Exists(Path.Combine(_tempPath, _configName)));
	}

	/// <summary>
	/// Tests if database connection strings are correctly created.
	/// </summary>
	[TestMethod]
	public void Test_IfDatabaseConnectionStringsAreCorrectlyCreated()
	{
		Assert.IsNotNull(_tempPath);

		ConfigurationManager configuration = new ConfigurationManager();
		configuration.Sources.Clear();

		configuration.AddJsonFile(Path.Combine(_tempPath, _configName));
		string combinedPath = Path.Combine(_tempPath, DbName);
		Assert.AreEqual(
			$"Data Source={combinedPath}",
			configuration.GetConnectionString("DefaultDbConnection"));
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
