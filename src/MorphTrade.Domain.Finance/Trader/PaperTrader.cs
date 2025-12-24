using OpAn.App.MorphTrade.Abstractions.Finance;

namespace OpAn.App.MorphTrade.Domain.Finance.Trader;

/// <summary>
/// Creates a paper trader.
/// </summary>
public class PaperTrader: ITrader
{
	/// <inheritdoc />
	public Task Buy(Ticker ticker)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task Sell(Ticker ticker)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task LongEntry(Ticker ticker, decimal tp, decimal sl)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task ShortEntry(Ticker ticker, decimal tp, decimal sl)
	{
		throw new NotImplementedException();
	}
}
