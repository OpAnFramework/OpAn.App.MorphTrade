using JetBrains.Annotations;

namespace OpAn.App.MorphTrade.Abstractions.IngestionDomain;

/// <summary>
/// Datapoint structure providing Open, Low, High, Close information.
/// </summary>
[PublicAPI]
public class OlhcDatapoint: Datapoint
{
    /// <summary>
    /// Opening value of a stock on given timestamp.
    /// </summary>
    public float Open { get; set; }

    /// <summary>
    /// High value of a stock on given timestamp.
    /// </summary>
    public float High { get; set; }

    /// <summary>
    /// Low value of a stock on given timestamp.
    /// </summary>
    public float Low { get; set; }

    /// <summary>
    /// Closing value of a stock on given timestamp.
    /// </summary>
    public float Close { get; set; }
}
