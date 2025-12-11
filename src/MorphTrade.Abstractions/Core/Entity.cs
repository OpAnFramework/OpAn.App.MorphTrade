namespace OpAn.App.MorphTrade.Abstractions.Core;

/// <summary>
/// Baseline entity definition.
/// </summary>
public class Entity
{
	/// <summary>
	/// Every entity should have a string identifier.
	/// </summary>
	public required string Id { get; init; } = Guid.NewGuid().ToString();
}
