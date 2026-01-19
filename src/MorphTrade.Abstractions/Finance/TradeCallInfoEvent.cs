using JetBrains.Annotations;

namespace OpAn.App.MorphTrade.Abstractions.Finance;

/// <summary>
/// Event spec for the trade call info.
/// </summary>
[PublicAPI]
public class TradeCallInfoEvent
{
	/// <summary>
	/// Trade call type.
	/// </summary>
	public required TradeCall TradeCall { get; set; }

	/// <summary>
	/// Entry price of the trade call.
	/// </summary>
	public decimal? Entry {get; set;}

	/// <summary>
	/// Stop loss value of the trade call. (for Long and Short positions.)
	/// </summary>
	public decimal? StopLoss {get; set;}

	/// <summary>
	/// A profitable exit value of the trade call. (for Long and Short positions.)
	/// </summary>
	public decimal? TakeProfit {get; set;}

	/// <summary>
	/// An implicit operator that allows conversion of entities from
	///		TradeCallInfoEvent to a TradeCallInfo.
	/// </summary>
	/// <param name="tradeCallInfoEvent"></param>
	/// <returns></returns>
	public static implicit operator TradeCallInfo(TradeCallInfoEvent tradeCallInfoEvent)
	{
		return new TradeCallInfo(
				tradeCallInfoEvent.TradeCall,
				tradeCallInfoEvent.Entry,
				tradeCallInfoEvent.StopLoss,
				tradeCallInfoEvent.TakeProfit
			);
	}
}
