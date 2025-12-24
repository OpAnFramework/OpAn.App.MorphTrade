namespace OpAn.App.MorphTrade.Abstractions.Finance;

/// <summary>
/// Different type of trade calls.
/// </summary>
public enum TradeCall
{
	/// <summary>
	/// Trade call for buy.
	/// </summary>
	Buy,

	/// <summary>
	/// Trade call for sale.
	/// </summary>
	Sell,

	/// <summary>
	/// Hold the position.
	/// </summary>
	Hold,

	/// <summary>
	/// Trade call for long position.
	/// </summary>
	Long,

	/// <summary>
	/// Trade call for short position.
	/// </summary>
	Short
}
