namespace OpAn.App.MorphTrade.Abstractions.IngestionDomain;

/// <summary>
/// A base implmentation of a datapoint.
/// </summary>
public class Datapoint
{
    /// <summary>
    /// Timestamp of the datapoint.
    /// </summary>
    public required DateTime Timestamp { get; set; }

    /// <summary>
    /// Timeframe during which the datapoint is read.
    /// </summary>
    public TimeSpan? Timeframe { get; set; }
}
