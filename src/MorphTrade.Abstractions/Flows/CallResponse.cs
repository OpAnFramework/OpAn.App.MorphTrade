using JetBrains.Annotations;
using OpAn.App.MorphTrade.Abstractions.Finance;

namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// A call response is the decision to make a trade call.
/// </summary>
[PublicAPI]
public class CallResponse
{
	/// <summary>
	/// Timestamp for the call response.
	/// </summary>
	public required int Timestamp {get; set;}

	/// <summary>
	/// Signifies what kind of trade call would be handled.
	/// </summary>
	public TradeCall TradeCall { get; set; }

	/// <summary>
	/// The instrument that will be handled with the trade call.
	/// </summary>
	public required Ticker Ticker { get; set; }
}
