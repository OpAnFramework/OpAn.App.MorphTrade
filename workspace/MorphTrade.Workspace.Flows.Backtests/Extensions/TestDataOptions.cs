namespace OpAn.App.MorphTrade.Workspace.Flows.Backtests.Extensions;

/// <summary>
/// Test data ingestion related options.
/// </summary>
public class TestDataOptions
{
	/// <summary>
	/// Absolute path to the test data json file.
	/// </summary>
	public string TestDataJsonAbsolutePath { get; init; } = "test_data.json";
}
