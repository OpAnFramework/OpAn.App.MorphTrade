using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Dto;
using OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Http;
using OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca.Tools;

namespace OpAn.App.MorphTrade.Domain.Ingestion.DataVendorAlpaca;

/// <summary>
/// A data vendor implementation for the vendor Alpaca.
/// </summary>
public class DataVendorAlpaca: IDataVendor
{
	private readonly HttpClient _client;
	private readonly AlpacaTimeframeHelper _timeframeHelper;

	/// <summary>
	/// Constructor for the Alpaca Data Vendor.
	/// </summary>
	/// <param name="clientWrapper">HttpClientWrapper for the alpaca.</param>
	/// <param name="timeframeHelper">Timeframe helper for the Alpaca vendor.</param>
	public DataVendorAlpaca(
		IAlpacaHttpClientWrapper clientWrapper,
		AlpacaTimeframeHelper timeframeHelper)
	{
		_client = clientWrapper.GetClient;
		_timeframeHelper = timeframeHelper;
	}
	/// <inheritdoc />
	public async Task<IList<VolumeDatapoint>> GetVolume(
		string index,
		string stock,
		DateTime observationTime,
		TimeSpan timespan,
		TimeSpan timeframe)
	{
		Dictionary<string, string?> queries = BuildCallQuery(
			index,
			stock,
			observationTime,
			timespan,
			timeframe);

		AlpacaHistoricBarStockResponseDto? responseDto = await CallAlpaca(queries);

		// Convert to natively supported OLHCV data point.
		return responseDto!
			.Bars![stock]
			.Select(dataPoint => new VolumeDatapoint
			{
				Timestamp = ToDateTime(dataPoint.Timestamp!),
				Timeframe = timeframe,
				Volume = dataPoint.Volume
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
		Dictionary<string, string?> queries = BuildCallQuery(
			index,
			stock,
			observationTime,
			timespan,
			timeframe);

		AlpacaHistoricBarStockResponseDto? responseDto = await CallAlpaca(queries);

		// Convert to natively supported OLHCV data point.
		return responseDto!
			.Bars![stock]
			.Select(dataPoint => new OlhcDatapoint
			{
				Timestamp = ToDateTime(dataPoint.Timestamp!),
				Timeframe = timeframe,
				Close = (float)dataPoint.Close,
				Open = (float)dataPoint.Open,
				High = (float)dataPoint.High,
				Low = (float)dataPoint.Low,
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
		Dictionary<string, string?> queries = BuildCallQuery(
			index,
			stock,
			observationTime,
			timespan,
			timeframe);

		AlpacaHistoricBarStockResponseDto? responseDto = await CallAlpaca(queries);

		// Convert to natively supported OLHCV data point.
		return responseDto!
			.Bars![stock]
			.Select(dataPoint => new OlhcvDatapoint
			{
				Timestamp = ToDateTime(dataPoint.Timestamp!),
				Timeframe = timeframe,
				Close = (float)dataPoint.Close,
				Open = (float)dataPoint.Open,
				High = (float)dataPoint.High,
				Low = (float)dataPoint.Low,
				Volume = dataPoint.Volume
			}).ToList();
	}

	private Dictionary<string, string?> BuildCallQuery(
		string index,
		string stock,
		DateTime observationTime,
		TimeSpan timespan,
		TimeSpan timeframe)
	{
		return new Dictionary<string, string?>
		{
			["feed"] = index,
			["symbols"] = stock,
			["timeframe"] = _timeframeHelper.ToAlpacaTimeframe(timeframe),
			["start"] = ToRfc3339((observationTime - timespan)),
			["end"] = ToRfc3339((observationTime))
		};
	}

	private async Task<AlpacaHistoricBarStockResponseDto?> CallAlpaca(Dictionary<string, string?> queries)
	{
		// Create a query encoded endpoint
		string endpoint = QueryHelpers.AddQueryString("stocks/bars", queries);

		HttpResponseMessage request = await _client.GetAsync(endpoint);
		object? response = await request.Content.ReadFromJsonAsync<object>();
		return JsonSerializer
			.Deserialize<AlpacaHistoricBarStockResponseDto>(response?.ToString()!);
	}

	private string ToRfc3339(DateTime dateTime)
	{
		return dateTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");
	}

	private DateTime ToDateTime(string dateTime)
	{
		return DateTimeOffset.Parse(dateTime).UtcDateTime;
	}
}
