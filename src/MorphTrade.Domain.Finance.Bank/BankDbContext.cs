using Microsoft.EntityFrameworkCore;
using OpAn.App.MorphTrade.Domain.Finance.Bank.AggregateRoot;
using OpAn.App.MorphTrade.Domain.Finance.Bank.Entities;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank;

/// <summary>
/// Database context for the Bank Subdomain.
/// </summary>
public class BankDbContext : DbContext
{
	/// <summary>
	/// Database context constructor for the Bank Subdomain.
	/// </summary>
	/// <param name="options"></param>
	public BankDbContext(DbContextOptions<BankDbContext> options)
	: base (options){

	}

	/// <summary>
	/// Accounts added to the Banking database.
	/// </summary>
	public DbSet<Account> Accounts { get; set; }

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(IFinanceDomainAssemblyMarker).Assembly);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(IBankingDomainAssemblyMarker).Assembly);
		base.OnModelCreating(modelBuilder);
	}
}
