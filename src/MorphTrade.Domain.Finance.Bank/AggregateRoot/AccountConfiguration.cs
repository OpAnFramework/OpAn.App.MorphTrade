using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.AggregateRoot;

/// <summary>
/// EF Core configuration for Account Entity
/// </summary>
public class AccountConfiguration: IEntityTypeConfiguration<Account>
{
	/// <inheritdoc />
	public void Configure(EntityTypeBuilder<Account> builder)
	{
		builder.HasIndex(e => e.Id).IsUnique();
		builder.Property(e => e.AccountName).IsRequired().HasMaxLength(100);
		builder.Property(e => e.Funds).IsRequired();
	}
}
