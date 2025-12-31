using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpAn.App.MorphTrade.Domain.Flows.Entities;

/// <summary>
/// Configures the flow meta information onto the entity type.
/// </summary>
public class FlowMetaConfiguration: IEntityTypeConfiguration<FlowMeta>
{
	/// <inheritdoc />
	public void Configure(EntityTypeBuilder<FlowMeta> builder)
	{
		builder.HasIndex(e => e.Id).IsUnique();
		builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
		builder.Property(e => e.StatusDescription).HasMaxLength(200);
		builder.Property(e => e.Status).HasConversion<string>();
	}
}
