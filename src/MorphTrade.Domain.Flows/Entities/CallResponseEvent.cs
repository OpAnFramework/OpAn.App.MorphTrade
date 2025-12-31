using CmdScale.EntityFrameworkCore.TimescaleDB.Configuration.Hypertable;
using Microsoft.EntityFrameworkCore;
using OpAn.App.MorphTrade.Abstractions.Finance;

namespace OpAn.App.MorphTrade.Domain.Flows.Entities;

/// <summary>
/// Records Call Responses as an event in the timeseries database.
/// </summary>
[Hypertable(nameof(Timestamp), ChunkTimeInterval = "86400000")] // 1 day in milliseconds
[PrimaryKey(nameof(Id), nameof(Timestamp))]
public class CallResponseEvent
{
	/// <summary>
	/// Identifier of the Call Response Event.
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// Timestamp of the Call response.
	/// </summary>
	public DateTime Timestamp { get; set; }

	/// <summary>
	/// Identifier of the flow that needs to be tracked with.
	/// </summary>
	public Guid FlowId { get; set; }

	/// <summary>
	/// Identifier of the flow that needs to be tracked with.
	/// </summary>
	public FlowMeta? Flow { get; set; }

	/// <summary>
	/// Provides a flag for backtest data.
	/// </summary>
	public bool IsBacktesting { get; set; }

	/// <summary>
	/// Signifies what kind of trade call would be handled.
	/// </summary>
	public TradeCall TradeCall { get; set; }

	/// <summary>
	/// The instrument that will be handled with the trade call.
	/// </summary>
	public required Ticker Ticker { get; set; }
}
