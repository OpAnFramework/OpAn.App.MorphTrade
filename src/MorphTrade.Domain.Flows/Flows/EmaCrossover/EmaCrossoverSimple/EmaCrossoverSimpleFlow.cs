using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using Skender.Stock.Indicators;

namespace OpAn.App.MorphTrade.Domain.Flows.Flows.EmaCrossover.EmaCrossoverSimple;
/// <summary>
/// A simple EMA crossover flow uses fast moving and slow moving EMA indicators.
///		The crossover provides a change of trend.
/// </summary>
public class EmaCrossoverSimpleFlow: IFlow
{
	private readonly EmaCrossoverSimpleOptions _options;
	private readonly ILogger<EmaCrossoverSimpleFlow> _logger;
	private readonly ITrader _trader;
	private bool? _isHolding, _isBought, _isClosed;
	private EmaResult? _slowEmaResult = null, _fastEmaResult = null;
	private readonly IDataVendor _dataVendor;

	/// <summary>
	/// Constructor for the simple EMA crossover flow.
	/// </summary>
	/// <param name="options">Options to be fed for the flow.</param>
	/// <param name="logger">Logger for targeted distributed logging.</param>
	/// <param name="dataVendor">Provides the live data vendor.</param>
	/// <param name="trader">Trader to call decisions.</param>
	public EmaCrossoverSimpleFlow(
			IOptions<EmaCrossoverSimpleOptions> options,
			ILogger<EmaCrossoverSimpleFlow> logger,
			IDataVendor dataVendor,
			ITrader trader)
	{
		_options = options.Value;
		_logger = logger;
		_dataVendor = dataVendor;
		_trader = trader;
		_isHolding = null;
		_isBought = null;
		_isClosed = null;
	}

	/// <inheritdoc />
	public string Name { get; set; } = nameof(EmaCrossoverSimpleFlow);

	/// <inheritdoc />
	public string GetFlowInstanceName()
	{
		return $"{Name}_{_options.Identifier}";
	}

	/// <inheritdoc />
	public async Task ExecuteAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("EmaCrossoverSimpleFlow started");
		while (!cancellationToken.IsCancellationRequested)
		{
			_logger.LogInformation("EmaCrossoverSimpleFlow running ...");
			await Task.Delay(1000, cancellationToken);
		}
	}

	/// <inheritdoc />
	public Task ExecuteLive(
		IList<OlhcvDatapoint> datapoints,
		OlhcvDatapoint currentDatapoint,
		in IList<CallResponse>? callResponses,
		string? symbol)
	{
		// Convert datapoints to Skender quotes
		/*
		IEnumerable<MorphQuote> quotes = datapoints.Select(x => (MorphQuote)x);
		var morphQuotes = quotes.ToList();
		IEnumerable<EmaResult> emaSlow = morphQuotes.GetEma(_options.SlowerIndicatorEmaCount).ToList();
		IEnumerable<EmaResult> emaFast = morphQuotes.GetEma(_options.FasterIndicatorEmaCount).ToList();
		*/

		var executionTime = currentDatapoint.Timestamp;

		// Downtrend detected
		if (_slowEmaResult!.Ema > _fastEmaResult!.Ema)
		{
			// Make a selling decision
			bool isBoughtPreviously = _isBought is not null && _isBought.Value;
			bool isHoldingPreviously = _isHolding is not null && _isHolding.Value;
			bool isClosed = _isClosed is not null && _isClosed.Value;

			if (isBoughtPreviously && isHoldingPreviously && !isClosed)
			{
				var ticker = new Ticker
				{
					Price = (decimal)currentDatapoint.Close,
					Quantity = (long)(_options.TickerInformation!.DefaultUnitSize ?? 1),
					Timestamp = executionTime,
					Symbol = symbol!
				};
				CallResponse callResponse = new CallResponse()
				{
					Timestamp = GetEpochTime(executionTime),
					TradeCall = TradeCall.Sell,
					Ticker = ticker
				};

				// TODO: Perform Purchase action, replace the always true condition to the purchase action.
				var task = _trader.Sell(ticker);
				bool isPurchased = task.IsCompleted;

				if (isPurchased && callResponses is not null) callResponses.Add(callResponse);

				// Set conditions:
				_isBought = false;
				_isHolding = false;
				_isClosed = true;
			}
		}

		// Uptrend detected
		if (_slowEmaResult!.Ema < _fastEmaResult!.Ema)
		{
			// Make a buying decision
			bool isBoughtPreviously = _isBought is not null && _isBought.Value;
			bool isHoldingPreviously = _isHolding is not null && _isHolding.Value;

			if (!isHoldingPreviously && !isBoughtPreviously)
			{
				var ticker = new Ticker
				{
					Price = (decimal)currentDatapoint.Close,
					Quantity = (long)(_options.TickerInformation!.DefaultUnitSize ?? 1),
					Timestamp = executionTime,
					Symbol = symbol!
				};
				CallResponse callResponse = new CallResponse()
				{
					Timestamp = GetEpochTime(executionTime),
					TradeCall = TradeCall.Buy,
					Ticker = ticker
				};

				// TODO: Perform Purchase action, replace the always true condition to the purchase action.
				var task = _trader.Buy(ticker);
				bool isPurchased = task.IsCompleted;

				if (isPurchased && callResponses is not null) callResponses.Add(callResponse);

				// Set conditions:
				_isBought = true;
				_isHolding = true;
				_isClosed = false;
			}
		}
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	/// <remarks>
	/// We are using a baseline batching system for the Backtests.
	/// This batching schema is inspired from the Quantization mechanisms of Nyquist Criteria.
	/// N_batch = 5*min(N_ema_fast, N_ema_slow)
	/// </remarks>
	public void Backtest(
		IDictionary<string, IList<OlhcvDatapoint>> screenedData,
		out IList<CallResponse> decisionList,
		out IList<object> results)
	{
		_logger.LogInformation($"Starting backtest: {nameof(EmaCrossoverSimpleFlow)}");
		// Batching strategy
		var batchSize = 5 * Math.Min(_options.FasterIndicatorEmaCount, _options.SlowerIndicatorEmaCount);

		decisionList = new List<CallResponse>();
		results = new List<object>();

		foreach (var (instrumentSymbol, historicData) in screenedData)
		{
			// Create additional EMA results for analytics
			// Convert datapoints to Skender quotes
			IEnumerable<MorphQuote> quotes = historicData.Select(x => (MorphQuote)x);
			var morphQuotes = quotes.ToList();
			IEnumerable<EmaResult> emaSlow = morphQuotes.GetEma(_options.SlowerIndicatorEmaCount).ToList();
			IEnumerable<EmaResult> emaFast = morphQuotes.GetEma(_options.FasterIndicatorEmaCount).ToList();


			if (historicData.Count < batchSize + 1)
			{
				results.Add(new
				{
					InstrumentSymbol = instrumentSymbol,
					Status = "SKIPPED: Not enough data points"
				});
				_logger.LogInformation($"Skipping backtest: {nameof(EmaCrossoverSimpleFlow)}");
				continue;
			}

			int left = 0;
			int right = batchSize - 1;
			int currentBarIndex = right + 1;

			while (left <= right && right < historicData.Count && currentBarIndex < historicData.Count)
			{
				IList<OlhcvDatapoint> batchData = historicData.Take(left..right).ToList();
				OlhcvDatapoint currentDatapoint = historicData[currentBarIndex];
				_slowEmaResult = emaSlow.ElementAt(currentBarIndex);
				_fastEmaResult = emaFast.ElementAt(currentBarIndex);

				ExecuteLive(
					batchData,
					currentDatapoint,
					decisionList,
					instrumentSymbol).Wait();

				// index value reassignment
				left++;
				right++;
				currentBarIndex = right + 1;
			}
			results.Add(new
			{
				InstrumentSymbol = instrumentSymbol,
				HistoricData = historicData,
				Analytics = new
				{
					EmaSlow = emaSlow,
					EmaFast = emaFast
				},
				Decisions = decisionList,
				Status = "SUCCEEDED"
			});
		}
		_logger.LogInformation($"Finished backtest: {nameof(EmaCrossoverSimpleFlow)}");
	}

	/// <inheritdoc />
	public async Task BacktestLive(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Starting backtest: {NAME}", Name);
		while (!cancellationToken.IsCancellationRequested)
		{
			_logger.LogInformation("Running the backtest");
			await Task.Delay(1000, cancellationToken);
		}
	}

	private int GetEpochTime(DateTime dateTime) => (int) (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
}
