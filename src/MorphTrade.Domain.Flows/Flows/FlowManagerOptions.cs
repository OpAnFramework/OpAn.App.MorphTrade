using OpAn.App.MorphTrade.Abstractions.Flows;

namespace OpAn.App.MorphTrade.Domain.Flows.Flows;

/// <summary>
/// Flow manager options.
/// </summary>
public class FlowManagerOptions
{
	/// <summary>
	/// A default flow manager identifier.
	/// </summary>
	public string Identifier { get; set; } = "DEFAULT_UNIT";

	/// <summary>
	/// Registrable flows-
	/// </summary>
	public IList<ManagedFlow> ManagedFlows { get; set; } = new List<ManagedFlow>();
}
