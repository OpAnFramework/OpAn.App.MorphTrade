using OpAn.App.MorphTrade.Abstractions.Finance;

namespace OpAn.App.MorphTrade.Domain.Flows.EmaCrossover.EmaCrossoverSimple;

/// <summary>
/// Options for simple EMA Crossover flow.
/// </summary>
public class EmaCrossoverSimpleOptions
{
	/// <summary>
	/// A faster EMA indicator count would provide,
	///		how many datapoints are seen for the fast moving indicator.
	/// </summary>
	public int FasterIndicatorEmaCount { get; set; }

	/// <summary>
	/// A slower EMA indicator count would provide,
	///		how many data points are seen for the slow moving indicator.
	/// </summary>
	public int SlowerIndicatorEmaCount { get; set; }

	/// <summary>
	/// Provides a ticker information.
	/// </summary>
	public TickerInformation? TickerInformation { get; set; }
}
