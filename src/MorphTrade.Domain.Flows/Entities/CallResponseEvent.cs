using CmdScale.EntityFrameworkCore.TimescaleDB.Configuration.Hypertable;
using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.Flows;

namespace OpAn.App.MorphTrade.Domain.Flows.Entities;

/// <summary>
/// Records Call Responses as an event in the timeseries database.
/// </summary>
[Hypertable(nameof(Timestamp), ChunkTimeInterval = "86400000")] // 1 day in milliseconds
public class CallResponseEvent: AuthorizedEntity
{
	/// <summary>
	/// Timestamp of the Call response.
	/// </summary>
	public DateTime Timestamp { get; set; }

	/// <summary>
	/// Identifier of the flow that needs to be tracked with.
	/// </summary>
	public required string FlowId { get; set; }

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
	/// Provides additional information for the trade call.
	/// </summary>
	public TradeCallInfoEvent? TradeCallInfo { get; set; }

	/// <summary>
	/// The instrument that will be handled with the trade call.
	/// </summary>
	public required Ticker Ticker { get; set; }


	/// <summary>
	/// Allows implicit conversions from call response events to Call Responses.
	/// </summary>
	/// <param name="callResponseEvent"></param>
	/// <returns></returns>
	public static implicit operator CallResponse(CallResponseEvent callResponseEvent)
	{
		if (callResponseEvent.TradeCallInfo == null)
		{
			throw new NullReferenceException("Call response event has null trade call info.");
		}
		return new CallResponse
		{
			Timestamp = (int) new DateTimeOffset(DateTime.SpecifyKind(callResponseEvent.Timestamp, DateTimeKind.Utc))
				.ToUnixTimeMilliseconds(),
			TradeCall = callResponseEvent.TradeCall,
			TradeCallInfo = callResponseEvent.TradeCallInfo,
			Ticker = callResponseEvent.Ticker,
		};
	}
}
