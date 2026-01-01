using OpAn.App.MorphTrade.Abstractions.Flows;

namespace OpAn.App.MorphTrade.Domain.Flows.Registries;

/// <summary>
/// Implementation of the flow registry.
/// </summary>
public class FlowRegistry: IFlowRegistry
{
	private readonly List<FlowRegistryEntry> _flows = new List<FlowRegistryEntry>();
	/// <inheritdoc />
	public void Register(FlowRegistryEntry instance)
	{
		_flows.Add(instance);
	}

	/// <inheritdoc />
	public IReadOnlyCollection<FlowRegistryEntry> Flows => _flows;
}
