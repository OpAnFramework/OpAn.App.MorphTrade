using OpAn.App.MorphTrade.Abstractions.Core;

namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// Registry object for the flows.
/// </summary>
public interface IFlowRegistry: IRegistry<FlowRegistryEntry>
{
	/// <summary>
	/// Returns the flows.
	/// </summary>
	IReadOnlyCollection<FlowRegistryEntry> Flows { get; }
}
