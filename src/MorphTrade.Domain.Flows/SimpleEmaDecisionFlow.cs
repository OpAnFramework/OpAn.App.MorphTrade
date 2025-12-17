using Microsoft.Extensions.Logging;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;

namespace OpAn.App.MorphTrade.Domain.Flows;

/// <summary>
/// Simple EMA decision Flow.
/// </summary>
public class SimpleEmaDecisionFlow : IFlow
{
	private readonly IList<OlhcvDatapoint>? _historicDatapoints;
	private readonly ILogger<SimpleEmaDecisionFlow> _logger;
	private readonly ITrader _trader;

	/// <summary>
	/// Constructor for the flow.
	/// </summary>
	/// <param name="historicData">Injected historic data.</param>
	/// <param name="logger">Injected logger.</param>
	/// <param name="trader">Injected trader.</param>
	public SimpleEmaDecisionFlow(
			IList<OlhcvDatapoint>? historicData,
			ILogger<SimpleEmaDecisionFlow> logger,
			ITrader trader
		)
	{
		_historicDatapoints = historicData;
		_logger = logger;
		_trader = trader;
	}

	/// <inheritdoc />
	public void Execute()
	{
		var currentDatapoint = _historicDatapoints!.Last();
		ExecuteLive(_historicDatapoints!, currentDatapoint);
	}

	/// <inheritdoc />
	public void ExecuteLive(IList<OlhcvDatapoint> datapoints, OlhcvDatapoint currentDatapoint)
	{
		// TODO: @Predator0901 Design your flow here
		_logger.LogInformation("Simple ema decision flow");
		_logger.LogInformation("----------------------------");
		_logger.LogInformation("Ingestion Data Count: {Count}", datapoints.Count);
		_logger.LogInformation("Current Value: {CurrentValue}", currentDatapoint.Open);

		// TODO: Check indicators, check conditions

		// TODO: Place an order
		// For example, buy a sample instrument called apple (IEX:APL) at current price.
		var ticker = new Ticker()
		{
			Timestamp = DateTime.Now,
			Price = (decimal) currentDatapoint.Close,
			Quantity = 10,
			Symbol = "IEX:APL",
			Volume = (decimal) currentDatapoint.Volume
		};

		_trader.Buy(ticker);
	}
}
