namespace OpAn.App.MorphTrade.Abstractions.Finance;

/// <summary>
/// Trader contract.
/// Allows creating positions on an instrument.
/// </summary>
public interface ITrader
{
	/// <summary>
	/// Buy a trade unit.
	/// </summary>
	/// <param name="ticker">Ticker to trade.</param>
	/// <returns></returns>
	Task Buy(Ticker ticker);

	/// <summary>
	/// Sell a trade unit.
	/// </summary>
	/// <param name="ticker">Ticker to trade.</param>
	/// <returns></returns>
	Task Sell(Ticker ticker);

	/// <summary>
	/// Creates a long entry.
	/// </summary>
	/// <param name="ticker">Ticker to trade.</param>
	/// <param name="tp">Target profit.</param>
	/// <param name="sl">Stop loss.</param>
	/// <returns></returns>
	Task LongEntry(Ticker ticker, decimal tp, decimal sl);

	/// <summary>
	/// Creates a short entry.
	/// </summary>
	/// <param name="ticker">Ticker to trade.</param>
	/// <param name="tp">Target profit.</param>
	/// <param name="sl">Stop loss.</param>
	Task ShortEntry(Ticker ticker, decimal tp, decimal sl);

	// TODO: Extend the contract for trailing Short and traling long
}
