namespace OpAn.App.MorphTrade.Abstractions.IngestionDomain;

/// <summary>
/// Volume datapoint.
/// </summary>
public class VolumeDatapoint: Datapoint
{
	/// <summary>
	/// Volume information set on the datapoint.
	/// </summary>
	public float Volume { get; set; }
}
