using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;

namespace OpAn.App.MorphTrade.Domain.Flows.Flows;

/// <summary>
/// Base class for the flows.
/// </summary>
public abstract class FlowBase: IFlow
{
	private readonly IOptions<FlowOptions> _options;
	private readonly ILogger<FlowBase> _logger;

	/// <summary>
	/// Execution context for the flow.
	/// </summary>
	protected virtual FlowExecutionContext? FlowExecutionContext {get; set;}

	/// <summary>
	/// Checks if it backtesting is being performed.
	/// </summary>
	protected bool IsBacktesting { get; set; } = false;

	/// <inheritdoc />
	public abstract string Name { get; set; }

	/// <summary>
	/// Screened datapoints at the time of execution.
	/// </summary>
	protected abstract IDictionary<string, ScreenedDatapoint> ScreenDatapoints { get; }

	/// <summary>
	/// Constructor for the base class.
	/// </summary>
	/// <param name="options">Flow options for the base class.</param>
	/// <param name="logger">Injected logger.</param>
	protected FlowBase(
		IOptions<FlowOptions> options,
		ILogger<FlowBase> logger)
	{
		_options = options;
		_logger = logger;
	}

	/// <inheritdoc />
	public string GetFlowInstanceName()
	{
		return $"{Name}_{_options.Value.Identifier}";
	}

	/// <inheritdoc />
	public virtual async Task ExecuteAsync(CancellationToken cancellationToken = default)
	{
		if (_options.Value.BacktestPrecheck)
		{
			IsBacktesting = true;
			_logger.LogInformation("Backtesting started");
			bool isDataFresh = await GenerateScreenedDatapoints();
			if (isDataFresh)
			{
				Backtest(
					ScreenDatapoints,
					out var _,
					out var _);
			}
			IsBacktesting = false;
		}

		_logger.LogInformation(
			"{Flow} : Starting flow at {Timestamp}",
			GetFlowInstanceName(),
			DateTime.Now);

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
			foreach (KeyValuePair<string, ScreenedDatapoint> historicData in ScreenDatapoints)
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
		}
	}

	/// <inheritdoc />
	public virtual async Task ExecuteAsync(FlowExecutionContext context, CancellationToken cancellationToken = default)
	{
		FlowExecutionContext = context;
		await ExecuteAsync(cancellationToken);
	}

	/// <inheritdoc />
	public abstract Task ExecuteLive(
		IList<OlhcvDatapoint> datapoints,
		OlhcvDatapoint currentDatapoint,
		IList<CallResponse>? callResponses,
		string? symbol);

	/// <inheritdoc />
	public abstract void Backtest(
		IDictionary<string, ScreenedDatapoint> screenedData,
		out IList<CallResponse> decisionList,
		out IList<object> results);

	/// <summary>
	/// Provides an EpochTime converted for the flows.
	/// </summary>
	/// <param name="dateTime">To be converted.</param>
	/// <returns></returns>
	public virtual int GetEpochTime(DateTime dateTime)
		=> (int) (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;

	/// <summary>
	/// Generate screened datapoints.
	/// </summary>
	/// <returns></returns>
	protected abstract Task<bool> GenerateScreenedDatapoints();

	/// <summary>
	/// Update the necessary indicators required the flow.
	/// </summary>
	/// <param name="datapoints">Locally stored datapoints.</param>
	/// <returns></returns>
	protected abstract void UpdateIndicators(IList<OlhcvDatapoint> datapoints);
}
