namespace OpAn.App.MorphTrade.Abstractions.IngestionDomain;

/// <summary>
/// A datapoint encompassing Open, Low, High, Close and Volume
/// </summary>
public class OlhcvDatapoint: OlhcDatapoint
{
	/// <summary>
	/// Volume information set on the datapoint.
	/// </summary>
	public float Volume { get; set; }
}
