using Microsoft.Extensions.Logging;
using OpAn.App.MorphTrade.Abstractions.Finance;

namespace OpAn.App.MorphTrade.Domain.Finance.Trader;

/// <summary>
/// A null trader which does not perfrom any action.
/// </summary>
public class NullTrader(
		ILogger<NullTrader> logger
	): ITrader
{
	private readonly ILogger<NullTrader> _logger = logger;

	/// <inheritdoc />
	public Task Buy(Ticker ticker)
	{
		_logger.LogInformation("Buying Order Created: {@Ticker}", ticker.Symbol);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public Task Sell(Ticker ticker)
	{
		_logger.LogInformation("Sell Order Created: {@Ticker}", ticker.Symbol);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public Task LongEntry(Ticker ticker, decimal tp, decimal sl)
	{
		_logger
			.LogInformation(
				"Long Entry Created: {@Ticker} with TP {TP} and SL {SL}",
				ticker.Symbol,
				tp,
				sl);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public Task ShortEntry(Ticker ticker, decimal tp, decimal sl)
	{
		_logger
			.LogInformation(
				"Long Entry Created: {@Ticker} with TP {TP} and SL {SL}",
				ticker.Symbol,
				tp,
				sl);
		return Task.CompletedTask;
	}
}
