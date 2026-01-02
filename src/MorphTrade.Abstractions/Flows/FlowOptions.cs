namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// Basic flow options.
/// </summary>
public class FlowOptions
{
	/// <summary>
	/// A default identifier for a flow.
	/// </summary>
	public string Identifier { get; set; } = "DEFAULT";

	/// <summary>
	/// Interval at which you perform the flow execution.
	/// </summary>
	/// <remarks>
	///	Specify it as ISO format. i.e. "hh:mm:ss"
	/// </remarks>
	public TimeSpan Interval {get; set;} = TimeSpan.FromSeconds(20);

	/// <summary>
	/// Performs backtests automatically when the flow is initialized.
	/// </summary>
	public bool BacktestPrecheck {get; set;} = false;
}
