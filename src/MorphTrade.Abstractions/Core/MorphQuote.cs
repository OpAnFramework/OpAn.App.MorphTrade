using OpAn.App.MorphTrade.Abstractions.IngestionDomain;
using Skender.Stock.Indicators;

namespace OpAn.App.MorphTrade.Abstractions.Core;

/// <summary>
/// Custom quote for MorphTrade.
/// </summary>
public class MorphQuote: IQuote
{
	/// <inheritdoc />
	public DateTime Date { get; init; }

	/// <inheritdoc />
	public decimal Open { get; init; }

	/// <inheritdoc />
	public decimal High { get; init; }

	/// <inheritdoc />
	public decimal Low { get; init; }

	/// <inheritdoc />
	public decimal Close { get; init; }

	/// <inheritdoc />
	public decimal Volume { get; init; }

	/// <summary>
	/// Allows an internal direct conversion from OlhcvDatapoint to the MorphQuote.
	/// </summary>
	/// <param name="datapoint">Datapoint to be converted.</param>
	/// <returns></returns>
	public static implicit operator MorphQuote(OlhcvDatapoint datapoint)
	{
		return new()
		{
			Date = datapoint.Timestamp,
			Open = (decimal) datapoint.Open,
			High = (decimal) datapoint.High,
			Low = (decimal) datapoint.Low,
			Volume = (decimal) datapoint.Volume,
			Close = (decimal) datapoint.Close
		};
	}
}
