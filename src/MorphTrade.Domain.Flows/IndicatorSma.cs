using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Abstractions.IngestionDomain;

namespace OpAn.App.MorphTrade.Domain.Flows;

/// <summary>
/// Indicator for simple moving average.
/// </summary>
public class IndicatorSma: IIndicator
{
	/// <inheritdoc />
	public (IList<float>?, bool?) Indicate(
		OlhcvDatapoint currentValue,
		IList<OlhcvDatapoint> historicData,
		params (string key, object value)[] kwargs)
	{
		var kwargDict = kwargs
			.ToDictionary(x => x.key, x => x.value);

		if (kwargDict["meanRange"] is null)
		{
			throw new Exception("meanRange required for SMA indicator");
		}

		int meanRange = (int)kwargDict["meanRange"];

		if (historicData.Count < meanRange)
		{
			throw new Exception("The mean range must be greater than total historic data-points");
		}

		int left = 0, right = meanRange;
		List<float> result = new List<float>();

		while (left < right && right < historicData.Count)
		{
			IList<OlhcvDatapoint> targetDPs = historicData
				.Skip(left)
				.Take(right - left)
				.ToList();
			result.Add(SimpleMovingAverage(targetDPs));
			left++;
			right++;
		}

		return (result, false);
	}

	private float SimpleMovingAverage(IList<OlhcvDatapoint> historicData)
	{
		return historicData.Average(dp => dp.Close);
	}
}
