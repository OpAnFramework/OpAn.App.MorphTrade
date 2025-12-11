using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.Entities;

/// <summary>
/// A configuration for the transaction entity
/// </summary>
public class TransactionConfiguration: IEntityTypeConfiguration<Transaction>
{
	/// <inheritdoc />
	public void Configure(EntityTypeBuilder<Transaction> builder)
	{
		builder.HasIndex(p => p.Id).IsUnique();
		builder.Property(p => p.Timestamp).IsRequired();
		builder.Property(p => p.TransactionType).IsRequired();
		builder.Property(p => p.Amount).IsRequired();
		builder.Property(p => p.AccountId).IsRequired().HasMaxLength(100);
		builder.Property(p => p.Reference).HasMaxLength(100);
		builder.Property(p => p.Currency).IsRequired().HasConversion<string>();
	}
}
