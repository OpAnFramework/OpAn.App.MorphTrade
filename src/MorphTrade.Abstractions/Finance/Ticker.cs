namespace OpAn.App.MorphTrade.Abstractions.Finance;

/// <summary>
/// Ticker provides an information about the instrument.
/// </summary>
public class Ticker
{
	/// <summary>
	/// Provides symbol for the ticker.
	/// </summary>
	public required string Symbol { get; set; }

	/// <summary>
	/// Price per ticker unit.
	/// </summary>
	public required decimal Price { get; set; }

	/// <summary>
	/// Volume of the ticker.
	/// </summary>
	public decimal? Volume { get; set; }

	/// <summary>
	/// Quantity to trade.
	/// </summary>
	public required Int64 Quantity { get; set; }

	/// <summary>
	/// Timestamp at which you create the ticker object.
	/// </summary>
	public required DateTime Timestamp { get; set; }
}
