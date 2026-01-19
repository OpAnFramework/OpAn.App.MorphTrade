using JetBrains.Annotations;

namespace OpAn.App.MorphTrade.Abstractions.Finance;

/// <summary>
/// Trade call info allows us to add additional information regarding the trade call being made.
/// </summary>
[PublicAPI]
public class TradeCallInfo
{
	/// <summary>
	/// A constructor for the trade call info.
	/// </summary>
	/// <param name="tradeCall">Trade call.</param>
	/// <param name="entry">Entry point.</param>
	/// <param name="stopLoss">Exit point to stop loss.</param>
	/// <param name="takeProfit">Exit point to take profit.</param>
	public TradeCallInfo(
		TradeCall tradeCall,
		decimal? entry,
		decimal? stopLoss,
		decimal? takeProfit)
	{
		TradeCall = tradeCall;
		if (IsAValidCall(tradeCall, entry, stopLoss, takeProfit))
		{
			Entry = entry;
			StopLoss = stopLoss;
			TakeProfit = takeProfit;
		}
	}

	private bool IsAValidCall(
			TradeCall tradeCall,
			decimal? entry,
			decimal? stopLoss,
			decimal? takeProfit)
	{
		if (tradeCall is TradeCall.Long or TradeCall.Short)
		{
			// Ensure that the valid Entry, SL and TP are set
			bool valuePointsAreSet =
				entry.HasValue && stopLoss.HasValue && takeProfit.HasValue;

			if (valuePointsAreSet)
			{
				switch (tradeCall)
				{
					case TradeCall.Long:
						return stopLoss < entry && entry < takeProfit;
					case TradeCall.Short:
						return takeProfit < entry && entry < stopLoss;
				}
			}
			return false;
		}
		return true;
	}

	/// <summary>
	/// Allows casting a trade call info into a TradeCallInfoEvent.
	/// </summary>
	/// <param name="tradeCallInfo"></param>
	/// <returns></returns>
	public static explicit operator TradeCallInfoEvent(TradeCallInfo tradeCallInfo)
	{
		return new TradeCallInfoEvent()
		{
			TradeCall = tradeCallInfo.TradeCall,
			Entry = tradeCallInfo.Entry,
			StopLoss = tradeCallInfo.StopLoss,
			TakeProfit = tradeCallInfo.TakeProfit
		};
	}

	/// <summary>
	/// Trade call type.
	/// </summary>
	public TradeCall TradeCall { get; init; }

	/// <summary>
	/// Entry price of the trade call.
	/// </summary>
	public decimal? Entry {get; init;}

	/// <summary>
	/// Stop loss value of the trade call. (for Long and Short positions.)
	/// </summary>
	public decimal? StopLoss {get; init;}

	/// <summary>
	/// A profitable exit value of the trade call. (for Long and Short positions.)
	/// </summary>
	public decimal? TakeProfit {get; init;}
}
