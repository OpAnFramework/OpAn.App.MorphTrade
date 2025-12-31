using Microsoft.EntityFrameworkCore;
using OpAn.App.MorphTrade.Domain.Flows.Entities;
using CmdScale.EntityFrameworkCore.TimescaleDB;

namespace OpAn.App.MorphTrade.Domain.Flows;

/// <summary>
/// Database context for the flow management.
/// </summary>
public class FlowsDbContext : DbContext
{
	/// <summary>
	/// Constructor for the database context
	/// </summary>
	/// <param name="options"></param>
	public FlowsDbContext(
		DbContextOptions<FlowsDbContext> options
		): base(options)
	{

	}

	/// <summary>
	/// Flows information for the flow management.
	/// </summary>
	public DbSet<FlowMeta> Flows { get; set; }

	/// <summary>
	/// Call responses registered on the flow.
	/// </summary>
	public DbSet<CallResponseEvent> CallResponseEvents { get; set; }

	// TODO: Register a model creation override
	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(IFlowsAssemblyMarker).Assembly);
		modelBuilder.HasPostgresExtension("timescaledb");
		base.OnModelCreating(modelBuilder);
	}
}
