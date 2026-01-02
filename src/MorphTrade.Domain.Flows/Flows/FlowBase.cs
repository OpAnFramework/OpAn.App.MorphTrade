using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;

namespace OpAn.App.MorphTrade.Domain.Flows.Flows;

/// <summary>
/// Base class for the flows.
/// </summary>
public abstract class FlowBase: IFlow
{
	/// <summary>
	/// Checks if it backtesting is being performed.
	/// </summary>
	public virtual bool IsBacktesting { get; set; } = false;

	/// <inheritdoc />
	public abstract string Name { get; set; }

	/// <summary>
	/// Screened datapoints at the time of execution.
	/// </summary>
	public abstract IDictionary<string, ScreenedDatapoint> ScreenDatapoints { get; }

	/// <inheritdoc />
	public string GetFlowInstanceName() => Name;

	/// <inheritdoc />
	public Task ExecuteAsync(CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task ExecuteAsync(FlowExecutionContext context, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task ExecuteLive(IList<OlhcvDatapoint> datapoints, OlhcvDatapoint currentDatapoint, IList<CallResponse>? callResponses, string? symbol)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public void Backtest(IDictionary<string, ScreenedDatapoint> screenedData, out IList<CallResponse> decisionList, out IList<object> results)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task BacktestLive(CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}
