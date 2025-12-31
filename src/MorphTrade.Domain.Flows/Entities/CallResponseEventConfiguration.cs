using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpAn.App.MorphTrade.Domain.Flows.Entities;

/// <summary>
/// Configuration for call response events.
/// </summary>
public class CallResponseEventConfiguration : IEntityTypeConfiguration<CallResponseEvent>
{
	/// <inheritdoc />
	public void Configure(EntityTypeBuilder<CallResponseEvent> builder)
	{
		builder.HasOne(e => e.Flow)
			.WithMany()
			.HasForeignKey(e => e.FlowId)
			.OnDelete(DeleteBehavior.Restrict);

		// Ticker entity configurations.
		builder.OwnsOne(
			e => e.Ticker,
			ticker =>
			{
				ticker.Property(t => t.Symbol)
					.HasMaxLength(50).IsRequired();
			});

		// Trade call entity configurations.
		builder.Property(e => e.TradeCall).HasConversion<string>();
	}
}
