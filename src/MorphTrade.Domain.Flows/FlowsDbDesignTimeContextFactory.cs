using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OpAn.App.MorphTrade.Domain.Flows;

/// <summary>
/// A database context factory for design time Migrations.
/// </summary>
public class FlowsDbDesignTimeContextFactory: IDesignTimeDbContextFactory<FlowsDbContext>
{
	/// <inheritdoc />
	public FlowsDbContext CreateDbContext(string[] args)
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();

		var optionsBuilder = new DbContextOptionsBuilder<FlowsDbContext>();

		// Read connection string from configuration
		var connectionString = configuration.GetConnectionString("FlowManagementDbConnection");

		optionsBuilder.UseNpgsql(connectionString);

		return new FlowsDbContext(optionsBuilder.Options);
	}
}
