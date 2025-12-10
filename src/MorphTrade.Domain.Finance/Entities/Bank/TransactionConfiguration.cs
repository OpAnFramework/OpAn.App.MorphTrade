using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpAn.App.MorphTrade.Domain.Finance.Entities.Bank;

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
	}
}
