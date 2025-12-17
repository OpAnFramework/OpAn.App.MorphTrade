using OpAn.App.MorphTrade.Abstractions.IngestionDomain;

namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// Contract for a flow.
/// </summary>
public interface IFlow
{
	/// <summary>
	/// Executes the flow.
	/// </summary>
	void Execute();

	/// <summary>
	/// Executes flow for the live context.
	/// </summary>
	/// <param name="datapoints"></param>
	/// <param name="currentDatapoint"></param>
	void ExecuteLive(IList<OlhcvDatapoint> datapoints, OlhcvDatapoint currentDatapoint);
}
