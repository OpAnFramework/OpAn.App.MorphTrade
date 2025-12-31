namespace OpAn.App.MorphTrade.Abstractions.Flows.Enums;

/// <summary>
/// An enumerable for the status of the flow.
/// </summary>
public enum FlowStatus
{
	/// <summary>
	/// Flow is created.
	/// </summary>
	Created,

	/// <summary>
	/// Flow is running.
	/// </summary>
	Running,

	/// <summary>
	/// Flow is Stopped.
	/// </summary>
	Stopped,

	/// <summary>
	/// Flow failed.
	/// </summary>
	Failed,

	/// <summary>
	/// Flow execution canceled.
	/// </summary>
	Canceled,

	/// <summary>
	/// Flow execution closed.
	/// </summary>
	Closed
}
