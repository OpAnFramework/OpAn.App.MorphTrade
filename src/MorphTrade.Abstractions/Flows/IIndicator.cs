using OpAn.App.MorphTrade.Abstractions.IngestionDomain;

namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// Defines a default contract for an indictor.
/// </summary>
public interface IIndicator
{
	/// <summary>
	/// Generates an Indicator for entry signals based on provided parameters.
	/// </summary>
	/// <param name="currentValue">Provides current OLHCV bar value.</param>
	/// <param name="historicData">Historic data-points for the subjected instrument unit.</param>
	/// <param name="kwargs">Provided additional keyword arguments.</param>
	/// <returns></returns>
	public (IList<float>?, bool?) Indicate(
			OlhcvDatapoint currentValue,
			IList<OlhcvDatapoint> historicData,
			params (string key, object value)[] kwargs
		);
}
