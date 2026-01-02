using JetBrains.Annotations;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.Flows;

namespace OpAn.App.MorphTrade.Domain.Flows.Flows.EmaCrossover.EmaCrossoverSimple;

/// <summary>
/// Options for simple EMA Crossover flow.
/// </summary>
public class EmaCrossoverSimpleOptions: FlowOptions
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
	/// Screened symbols from the options.
	/// </summary>
	[PublicAPI]
	public IList<string>? ScreenedSymbols { get; set; }

	/// <summary>
	/// Provides a ticker information.
	/// </summary>
	public TickerInformation? TickerInformation { get; set; }

	/// <summary>
	/// Time at which the observations are done.
	/// </summary>
	public DateTime BacktestObservationTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Observation period of the flow.
	/// </summary>
	public TimeSpan ObservationPeriod { get; set; } = TimeSpan.FromDays(3);

	/// <summary>
	/// Timeframe for the datapoint.
	/// </summary>
	public TimeSpan Timeframe {get; set;} = TimeSpan.FromMinutes(30);
}
