using JetBrains.Annotations;
using OpAn.App.MorphTrade.Abstractions.Flows.Enums;

namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// Registry entry for the flows.
/// </summary>
[PublicAPI]
public class FlowRegistryEntry
{
	/// <summary>
	/// Cancellation toke for the flow registry entry.
	/// </summary>
	public required CancellationTokenSource TokenSource { get; set; }

	/// <summary>
	/// Flow object for the registry entry.
	/// </summary>
	public required IFlow Flow { get; set; }

	/// <summary>
	/// Status of the flow running.
	/// </summary>
	public FlowStatus? Status { get; set; }

	/// <summary>
	/// An attached execution context.
	/// </summary>
	public FlowExecutionContext? Context { get; set; }
}
