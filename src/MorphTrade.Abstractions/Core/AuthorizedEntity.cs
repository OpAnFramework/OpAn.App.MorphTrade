namespace OpAn.App.MorphTrade.Abstractions.Core;

/// <summary>
/// A baseline model for an authorized entity.
/// </summary>
public class AuthorizedEntity: Entity
{
	/// <summary>
	/// UserId for the Authorized entities.
	/// </summary>
	public required string UserId { get; set; } = "DEFAULT_USERID";
}
