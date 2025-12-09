namespace OpAn.App.MorphTrade.Abstractions.IngestionDomain;

/// <summary>
/// Formulates a contract for different data vendors.
/// </summary>
public interface IDataVendor
{
    /// <summary>
    /// Provides market volume information on specified index and stock during a particular
    ///     time and timeframe.
    /// </summary>
    /// <param name="index">Specified Index.</param>
    /// <param name="stock">Specified stock.</param>
    /// <param name="observationTime">Specified observation time.</param>
    /// <param name="timespan">Specified time between which data-points are subjected to analysis.</param>
    /// <param name="timeframe">Specified timeframe for each data-point's duration.</param>
    /// <returns></returns>
    public Task<IList<VolumeDatapoint>> GetVolume(
        string index,
        string stock,
        DateTime observationTime,
        TimeSpan timespan,
        TimeSpan timeframe
    );

    /// <summary>
    /// Provides Open Low High Close information on specified index and stock during a particular
    ///     time and timeframe.
    /// </summary>
    /// <param name="index">Specified Index.</param>
    /// <param name="stock">Specified stock.</param>
    /// <param name="observationTime">Specified observation time.</param>
    /// <param name="timespan">Specified time between which data-points are subjected to analysis.</param>
    /// <param name="timeframe">Specified timeframe for each datapoint's duration.</param>
    /// <returns></returns>
    public Task<IList<OlhcDatapoint>> GetOlhcData(
        string index,
        string stock,
        DateTime observationTime,
        TimeSpan timespan,
        TimeSpan timeframe
    );

    /// <summary>
    /// Provides Open Low High Close information on specified index and stock during a particular
    ///     time and timeframe.
    /// </summary>
    /// <param name="index">Specified Index.</param>
    /// <param name="stock">Specified stock.</param>
    /// <param name="observationTime">Specified observation time.</param>
    /// <param name="timespan">Specified time between which data-points are subjected to analysis.</param>
    /// <param name="timeframe">Specified timeframe for each datapoint's duration.</param>
    /// <returns></returns>
    public Task<IList<OlhcvDatapoint>> GetOlhcvData(
	    string index,
	    string stock,
	    DateTime observationTime,
	    TimeSpan timespan,
	    TimeSpan timeframe
	);
}
