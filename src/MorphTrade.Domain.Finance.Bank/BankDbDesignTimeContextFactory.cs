using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank;

/// <summary>
/// Provides a design time database context for migrations.
///
/// NOTE: This is important, as the program exits early if the design time configurations are not provided correctly.
///		This is a failsafe design time component, as this will run when the migrations are performed with EF Core.
/// </summary>
public class BankDbDesignTimeContextFactory: IDesignTimeDbContextFactory<BankDbContext>
{
	/// <inheritdoc />
	public BankDbContext CreateDbContext(string[] args)
	{
		const string temporaryDbConnectionString = "Data Source=opan-morph-trade_migration_temp.db";
		var optionsBuilder = new DbContextOptionsBuilder<BankDbContext>();

		optionsBuilder.UseSqlite(temporaryDbConnectionString);
		return new BankDbContext(optionsBuilder.Options);
	}
}
