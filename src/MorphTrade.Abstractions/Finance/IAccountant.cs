namespace OpAn.App.MorphTrade.Abstractions.Finance;

/// <summary>
/// An accountant contract that would manage ledge on behalf on MorphTrade.
/// </summary>
public interface IAccountant
{
	/// <summary>
	/// Creates a purchase order for a ticker.
	/// </summary>
	/// <param name="ticker">Instrument to be purchased with specified information.</param>
	/// <returns></returns>
	public Task CreatePurchaseOrder(Ticker ticker);

	/// <summary>
	/// Creates a sell order for a ticker.
	/// This also realises the PnL accounts.
	/// </summary>
	/// <param name="ticker">Instrument to be purchased with specified information.</param>
	/// <returns></returns>
	public Task CreateSellOrder(Ticker ticker);

	/// <summary>
	/// Manages the unrealised PnL statement for the accounts.
	/// </summary>
	/// <param name="tickers">A list of tickers to be tracked.</param>
	/// <returns></returns>
	public Task TrackTickers(IList<Ticker> tickers);
}
