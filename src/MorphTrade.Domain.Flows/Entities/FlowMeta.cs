using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Abstractions.Flows.Enums;

namespace OpAn.App.MorphTrade.Domain.Flows.Entities;

/// <summary>
/// A flow entity that specifies meta information about the flow.
/// </summary>
public class FlowMeta: AuthorizedEntity
{
	/// <summary>
	/// Name of the flow.
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Flow last updated on.
	/// </summary>
	public DateTime? LastUpdated { get; set; }

	/// <summary>
	/// Description about the change of status.
	/// </summary>
	public string? StatusDescription { get; set; }

	/// <summary>
	/// Status of the flow.
	/// </summary>
	public FlowStatus? Status { get; set; }
}
