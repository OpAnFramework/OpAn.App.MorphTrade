using JetBrains.Annotations;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;

namespace OpAn.App.MorphTrade.Abstractions.Core;

/// <summary>
/// Screened datapoints class.
/// </summary>
[PublicAPI]
public class ScreenedDatapoint
{
	/// <summary>
	/// Provides information if a datapoint is fresh or not.
	/// </summary>
	public required bool IsFresh { get; set; } = false;

	/// <summary>
	/// Datapoints screened.
	/// </summary>
	public required IList<OlhcvDatapoint> Datapoints { get; set; }
}
