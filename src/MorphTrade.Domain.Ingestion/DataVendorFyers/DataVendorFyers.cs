using FyersCSharpSDK;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers.Auth;
using OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers.Tools;

namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorFyers;

/// <summary>
/// Data vendor implementation for the vendor 'Fyers'
/// </summary>
public class DataVendorFyers: IDataVendor
{
	private readonly FyersClass _fyersModel;

	/// <summary>
	/// Constructs the data vendor for fyers.
	/// </summary>
	/// <param name="credentials">Fyers credentials.</param>
	/// <param name="options">Options for the Fyers details.</param>
	public DataVendorFyers(
		IFyersCredentials credentials,
		IOptions<FyersAuthenticatorOptions> options)
	{
		_fyersModel = FyersClass.Instance;
		_fyersModel.ClientId = options.Value.ClientId;
		_fyersModel.AccessToken = credentials.GetAccessToken();
	}

    /// <inheritdoc />
    public async Task<IList<VolumeDatapoint>> GetVolume(
	    string index,
	    string stock,
	    DateTime observationTime,
	    TimeSpan timespan,
	    TimeSpan timeframe)
    {
	    Tuple<JArray, JObject> historicData = await GetHistoricalData(
		    index, stock, observationTime, timespan, timeframe
	    );
	    var rawData = historicData.Item1.ToObject<List<List<int>>>();


	    return rawData!.Select(x => new VolumeDatapoint
	    {
		    Timestamp = DateTimeOffset.FromUnixTimeSeconds(x[0]).UtcDateTime,
		    Timeframe = timeframe,
		    Volume = x[5],
	    }).ToList();
    }

    /// <inheritdoc />
    public async Task<IList<OlhcDatapoint>> GetOlhcData(
	    string index,
	    string stock,
	    DateTime observationTime,
	    TimeSpan timespan,
	    TimeSpan timeframe)
    {
	    Tuple<JArray, JObject> historicData = await GetHistoricalData(
			index, stock, observationTime, timespan, timeframe
	    );
	    var rawData = historicData.Item1.ToObject<List<List<int>>>();

	    return rawData!.Select(x => new OlhcDatapoint
	    {
		    Timestamp = DateTimeOffset.FromUnixTimeSeconds(x[0]).UtcDateTime,
		    Timeframe = timeframe,
		    Open = x[1],
		    High = x[2],
		    Low = x[3],
		    Close = x[4]
	    }).ToList();
    }

    /// <inheritdoc />
    public async Task<IList<OlhcvDatapoint>> GetOlhcvData(
	    string index,
	    string stock,
	    DateTime observationTime,
	    TimeSpan timespan,
	    TimeSpan timeframe)
    {
	    Tuple<JArray, JObject> historicData = await GetHistoricalData(
		    index, stock, observationTime, timespan, timeframe
	    );
	    var rawData = historicData.Item1.ToObject<List<List<int>>>();

	    return rawData!.Select(x => new OlhcvDatapoint
	    {
		    Timestamp = DateTimeOffset.FromUnixTimeSeconds(x[0]).UtcDateTime,
		    Timeframe = timeframe,
		    Open = x[1],
		    High = x[2],
		    Low = x[3],
		    Close = x[4],
		    Volume = x[5],
	    }).ToList();

    }

    private async Task<Tuple<JArray, JObject>> GetHistoricalData(
	    string index,
	    string stock,
	    DateTime observationTime,
	    TimeSpan timespan,
	    TimeSpan timeframe)
    {
	    StockHistoryModel model = new()
	    {
		    Symbol = $"{index}:{stock}",
		    DateFormat = "1",
		    ContFlag = 1,
		    RangeFrom = (observationTime-timespan).ToString("yyyy-MM-dd"),
		    RangeTo = observationTime.ToString("yyyy-MM-dd"),
		    Resolution = $"{CandleResolutionHelper.ToResolutionString(timeframe)}S"
	    };

	    return await _fyersModel.GetStockHistory(model);
    }
}
