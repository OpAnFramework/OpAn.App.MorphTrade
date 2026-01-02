using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;

namespace OpAn.App.MorphTrade.Domain.Flows.Tests.Fixtures;

/// <summary>
/// Performs the database fixtures.
/// </summary>
public class DatabaseFixtures: IAsyncDisposable
{
	/// <summary>
	/// Database name.
	/// </summary>
	private const string DatabaseName = "MorphTradeDB_FlowsTest";

	/// <summary>
	/// Database user.
	/// </summary>
	private const string DatabaseUser = "postgres";

	/// <summary>
	/// Database password.
	/// </summary>
	private const string DatabasePassword = "postgres";

	private IContainer? _container;
	private PostgreSqlConfiguration? _configuration;
	private int _port;

	private PostgreSqlConfiguration CreateConfiguration()
	{
		return new PostgreSqlConfiguration(
			database: DatabaseName,
			username: DatabaseUser,
			password: DatabasePassword);
	}


	/// <summary>
	/// Initialises the database fixtures.
	/// </summary>
	public async ValueTask InitAsync()
	{
		_configuration = CreateConfiguration();
		_port = StaticNetworkHelper.GetFreePort();
		_container = new PostgreSqlBuilder("timescale/timescaledb:latest-pg17")
			.WithDatabase(_configuration.Database)
			.WithUsername(_configuration.Username)
			.WithPassword(_configuration.Password)
			.WithPortBinding(_port, 5432)
			.Build();
		await _container.StartAsync();
	}

	/// <summary>
	/// Provides the runtime plugs to the test container.
	/// </summary>
	/// <returns></returns>
	public (PostgreSqlConfiguration? configuration, int? port) GetRuntimePlugs()
	{
		return (_configuration, _port);
	}

	/// <summary>
	/// Provides the running test container.
	/// </summary>
	/// <returns></returns>
	public IContainer? GetContainer()
	{
		return _container;
	}

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		if (_container is not null) await _container!.DisposeAsync();
	}
}
