using Skender.Stock.Indicators;

namespace OpAn.App.MorphTrade.Workspace.IndicatorDesign.Abstractions;

/// <summary>
/// Custom quote for MorphTrade.
/// </summary>
public class MorphQuote: IQuote
{
	/// <inheritdoc />
	public DateTime Date { get; set; }

	/// <inheritdoc />
	public decimal Open { get; set; }

	/// <inheritdoc />
	public decimal High { get; set; }

	/// <inheritdoc />
	public decimal Low { get; set; }

	/// <inheritdoc />
	public decimal Close { get; set; }

	/// <inheritdoc />
	public decimal Volume { get; set; }
}
