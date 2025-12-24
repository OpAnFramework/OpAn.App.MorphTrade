using JetBrains.Annotations;

namespace OpAn.App.MorphTrade.Abstractions.Finance;

/// <summary>
/// Provides a base ticker information.
/// </summary>
[PublicAPI]
public class TickerInformation
{
	/// <summary>
	/// Exchange from which ticker is to be configured for.
	/// </summary>
	public string? Exchange { get; set; }

	/// <summary>
	/// Symbol indicating the instrument.
	/// </summary>
	public string? Symbol { get; set; }

	/// <summary>
	/// Default purchasing unit size.
	/// </summary>
	public decimal? DefaultUnitSize { get; set; }

	/// <summary>
	/// Minimum trading unit size.
	/// </summary>
	public decimal? MinimumUnitSize { get; set; }

	/// <summary>
	/// Maximum trading unit size.
	/// </summary>
	public decimal? MaximumUnitSize { get; set; }
}
