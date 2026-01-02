using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Domain.Flows.Entities;
using OpAn.App.MorphTrade.Domain.Flows.Repositories;
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
	private readonly IFlowsRepository _flowsRepository;
	private bool? _isHolding, _isBought, _isClosed;
	private IEnumerable<EmaResult>? _slowEmaResult, _fastEmaResult;
	private readonly IDataVendor _dataVendor;
	private FlowExecutionContext? _executionContext;
	private readonly IDictionary<string, ScreenedDatapoint> _screenedDatapoints
		= new Dictionary<string, ScreenedDatapoint>();

	private readonly IDictionary<string, DateTime> _lastProcessedTimestamps = new Dictionary<string, DateTime>();

	// Backtesting parameters
	private bool _isBacktesting;
	private int _backtestingCurrentIndex;

	/// <summary>
	/// Constructor for the simple EMA crossover flow.
	/// </summary>
	/// <param name="options">Options to be fed for the flow.</param>
	/// <param name="logger">Logger for targeted distributed logging.</param>
	/// <param name="dataVendor">Provides the live data vendor.</param>
	/// <param name="trader">Trader to call decisions.</param>
	/// <param name="flowsRepository">Injected flows repository.</param>
	public EmaCrossoverSimpleFlow(
			IOptions<EmaCrossoverSimpleOptions> options,
			ILogger<EmaCrossoverSimpleFlow> logger,
			IDataVendor dataVendor,
			ITrader trader,
			IFlowsRepository flowsRepository)
	{
		_options = options.Value;
		_logger = logger;
		_dataVendor = dataVendor;
		_flowsRepository = flowsRepository;
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
		if (_options.BacktestPrecheck)
		{
			_isBacktesting = true;
			_logger.LogInformation("Backtesting started");
			bool isDataFresh = await GenerateScreenedDatapoints();
			if (isDataFresh)
			{
				Backtest(
					_screenedDatapoints,
					out var _,
					out var _);
			}
			_isBacktesting = false;
		}

		while (!cancellationToken.IsCancellationRequested)
		{
			// Get the historic data and current value.
			bool isDataFresh = await GenerateScreenedDatapoints();
			if (isDataFresh)
			{
				_logger.LogInformation(
					"{Flow} : Generated new datapoints at {Timestamp}",
					GetFlowInstanceName(),
					DateTime.Now);
			}

			// Execute each flow in the given time.
			foreach (KeyValuePair<string, ScreenedDatapoint> historicData in _screenedDatapoints)
			{
				if (historicData.Value.IsFresh)
				{
					_logger.LogInformation(
						"{Flow} : Executing the decision strategy {Timestamp}",
						GetFlowInstanceName(),
						DateTime.Now);
					UpdateIndicators(historicData.Value.Datapoints);
					// Execute the live data
					await ExecuteLive(
						historicData.Value.Datapoints,
						historicData.Value.Datapoints.Last(),
						null,
						historicData.Key
					);
				}
			}
			await Task.Delay(_options.Interval, cancellationToken);
		}
	}

	/// <inheritdoc />
	public async Task ExecuteAsync(
		FlowExecutionContext context,
		CancellationToken cancellationToken = default)
	{
		_executionContext = context;
		await ExecuteAsync(cancellationToken);
	}

	/// <inheritdoc />
	public async Task ExecuteLive(
		IList<OlhcvDatapoint> datapoints,
		OlhcvDatapoint currentDatapoint,
		IList<CallResponse>? callResponses,
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
		EmaResult slowEmaResult = _isBacktesting
			? _slowEmaResult!.ElementAt(_backtestingCurrentIndex)
			: _slowEmaResult!.Last();
		EmaResult fastEmaResult = _isBacktesting
			? _fastEmaResult!.ElementAt(_backtestingCurrentIndex)
			: _fastEmaResult!.Last();

		// Downtrend detected
		if (slowEmaResult.Ema > fastEmaResult.Ema)
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
					Quantity = (long)(_options.TickerInformation is not null
						? _options.TickerInformation.DefaultUnitSize ?? 1
						: 1),
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

				if (isPurchased)
				{
					if (callResponses is not null) callResponses.Add(callResponse);
					if (_executionContext is not null)
					{
						FlowMeta metadata = _executionContext.GetRequired<FlowMeta>();
						await _flowsRepository.CreateUpdateCallResponseEventAsync(
							metadata,
							callResponse,
							_isBacktesting);
					}
				}

				// Set conditions:
				_isBought = false;
				_isHolding = false;
				_isClosed = true;
			}
		}

		// Uptrend detected
		if (slowEmaResult.Ema < fastEmaResult.Ema)
		{
			// Make a buying decision
			bool isBoughtPreviously = _isBought is not null && _isBought.Value;
			bool isHoldingPreviously = _isHolding is not null && _isHolding.Value;

			if (!isHoldingPreviously && !isBoughtPreviously)
			{
				var ticker = new Ticker
				{
					Price = (decimal)currentDatapoint.Close,
					Quantity = (long)(_options.TickerInformation is not null
						? _options.TickerInformation.DefaultUnitSize ?? 1
						: 1),
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

				if (isPurchased)
				{
					if (callResponses is not null) callResponses.Add(callResponse);
					if (_executionContext is not null)
					{
						FlowMeta metadata = _executionContext.GetRequired<FlowMeta>();
						await _flowsRepository.CreateUpdateCallResponseEventAsync(
							metadata,
							callResponse,
							_isBacktesting);
					}
				}

				// Set conditions:
				_isBought = true;
				_isHolding = true;
				_isClosed = false;
			}
		}
	}

	/// <inheritdoc />
	/// <remarks>
	/// We are using a baseline batching system for the Backtests.
	/// This batching schema is inspired from the Quantization mechanisms of Nyquist Criteria.
	/// N_batch = 5*min(N_ema_fast, N_ema_slow)
	/// </remarks>
	public void Backtest(
		IDictionary<string, ScreenedDatapoint> screenedData,
		out IList<CallResponse> decisionList,
		out IList<object> results)
	{
		_logger.LogInformation(
			"Starting backtest: {FlowName} for Symbols: {Symbols}",
			GetFlowInstanceName(),
			screenedData.Keys);
		// Batching strategy
		var batchSize = 5 * Math.Min(_options.FasterIndicatorEmaCount, _options.SlowerIndicatorEmaCount);

		decisionList = new List<CallResponse>();
		results = new List<object>();

		foreach (var (instrumentSymbol, historicData) in screenedData)
		{
			// Create additional EMA results for analytics
			// Update the indicators
			_logger.LogInformation("Updating indicators for {Symbol}", instrumentSymbol);
			UpdateIndicators(historicData.Datapoints);

			if (historicData.Datapoints.Count < batchSize + 1)
			{
				results.Add(new
				{
					InstrumentSymbol = instrumentSymbol,
					Status = "SKIPPED: Not enough data points"
				});
				_logger.LogInformation(
					"Skipping backtest: {Flow} for {Symbol}",
					GetFlowInstanceName(),
					instrumentSymbol);
				continue;
			}

			int left = 0;
			int right = batchSize - 1;
			int currentBarIndex = right + 1;

			_logger.LogInformation(
				"Backtesting with {Flow} and {Symbol}",
				GetFlowInstanceName(), instrumentSymbol);

			while (left <= right && right < historicData.Datapoints.Count && currentBarIndex < historicData.Datapoints.Count)
			{
				IList<OlhcvDatapoint> batchData = historicData.Datapoints.Take(left..right).ToList();
				OlhcvDatapoint currentDatapoint = historicData.Datapoints[currentBarIndex];

				// filter
				_backtestingCurrentIndex = currentBarIndex;

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
					EmaSlow = _slowEmaResult,
					EmaFast = _fastEmaResult
				},
				Decisions = decisionList,
				Status = "SUCCEEDED"
			});
			_logger.LogInformation(
				"Backtesting Finished! {Flow} and {Symbol}",
				GetFlowInstanceName(), instrumentSymbol);
		}
		_logger.LogInformation($"Finished backtest: {nameof(EmaCrossoverSimpleFlow)}");
	}

	private async Task<bool> GenerateScreenedDatapoints()
	{
		bool isFresh = false;
		foreach (string symbolString in _options.ScreenedSymbols!)
		{
			// Symbol string should be formatted as 'index:TickerSymbol'
			var (index, tickerSymbol) = symbolString.Split(':', 2) switch
			{
				[var a, var b] =>  (a, b),
				[var a] => (a, string.Empty),
				_ => (string.Empty, string.Empty)
			};

			var olhcvData = await _dataVendor.GetOlhcvData(
				index,
				tickerSymbol,
				_isBacktesting ? _options.BacktestObservationTime : DateTime.UtcNow,
				_options.ObservationPeriod,
				_options.Timeframe);

			bool isSymbolDataFresh = false;
			if (
				!_lastProcessedTimestamps.ContainsKey(symbolString)
				|| _lastProcessedTimestamps[symbolString] < olhcvData.Last().Timestamp)
			{
				isSymbolDataFresh = true;
				_lastProcessedTimestamps[symbolString] = olhcvData.Last().Timestamp;
			}

			if (isSymbolDataFresh)
			{
				_screenedDatapoints.TryGetValue(symbolString, out ScreenedDatapoint? screenedDatapoint);
				// Prevents memory overflow.
				screenedDatapoint ??= new ScreenedDatapoint
				{
					Datapoints = olhcvData,
					IsFresh = isSymbolDataFresh
				};

				screenedDatapoint.Datapoints = olhcvData;
				_screenedDatapoints[symbolString] = screenedDatapoint;

			}
			else
			{
				_screenedDatapoints.TryGetValue(symbolString, out ScreenedDatapoint? screenedDatapoint);
				screenedDatapoint!.IsFresh = isSymbolDataFresh;
			}

			isFresh = isFresh || isSymbolDataFresh;
		}

		return isFresh;
	}

	private void UpdateIndicators(IList<OlhcvDatapoint> datapoints)
	{
		// Convert datapoints to Skender quotes
		IEnumerable<MorphQuote> quotes = datapoints.Select(x => (MorphQuote)x);
		var morphQuotes = quotes.ToList();
		_slowEmaResult = morphQuotes.GetEma(_options.SlowerIndicatorEmaCount).ToList();
		_fastEmaResult = morphQuotes.GetEma(_options.FasterIndicatorEmaCount).ToList();

	}

	private int GetEpochTime(DateTime dateTime) => (int) (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
}
