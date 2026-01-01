using JetBrains.Annotations;

namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// An abstraction for the managed flow.
/// </summary>
[PublicAPI]
public class ManagedFlow
{
	/// <summary>
	/// Type of the flow to be registered.
	/// </summary>
	public string? Type { get; set; }

	/// <summary>
	/// Options to be passed to the manager.
	/// </summary>
	public dynamic? Options { get; set; }
}
