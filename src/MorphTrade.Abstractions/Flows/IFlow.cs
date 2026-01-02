using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;

namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// Contract for a flow.
/// </summary>
public interface IFlow
{
	/// <summary>
	/// Provides a name for the flow.
	/// </summary>
	string Name { get; set; }

	/// <summary>
	/// Returns the flow instance name.
	/// </summary>
	/// <returns></returns>
	string GetFlowInstanceName();

	/// <summary>
	/// Executes the flow.
	/// </summary>
	Task ExecuteAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Executes the flow with the provided FlowExecutionContext.
	/// </summary>
	/// <remarks>
	/// <seealso cref="FlowExecutionContext"/>
	/// </remarks>
	Task ExecuteAsync(
		FlowExecutionContext context,
		CancellationToken cancellationToken = default
		);

	/// <summary>
	/// Executes flow for the live context.
	/// </summary>
	/// <param name="datapoints">Data points for the live execution.</param>
	/// <param name="currentDatapoint">Data point for the evaluation.</param>
	/// <param name="callResponses">Optional trade call recorder.</param>
	/// <param name="symbol">Symbol to be checked for.</param>
	Task ExecuteLive(
		IList<OlhcvDatapoint> datapoints,
		OlhcvDatapoint currentDatapoint,
		IList<CallResponse>? callResponses,
		string? symbol);

	/// <summary>
	/// Performs backtest for the live execution flow.
	/// </summary>
	/// <param name="screenedData"></param>
	/// <param name="decisionList"></param>
	/// <param name="results"></param>
	void Backtest(
		IDictionary<string, ScreenedDatapoint> screenedData,
		out IList<CallResponse> decisionList,
		out IList<object> results);

	// TODO: Add a backtest parameter overload for the live data tenant
	/// <summary>
	/// Adds a live backtesting mechanism to the flow.
	/// </summary>
	Task BacktestLive(CancellationToken cancellationToken = default);
}
