# Domain Flows - FlowBase & Strategy Implementation

The `FlowBase` class provides the core foundation for implementing trading strategies (Flows) within the MorphTrade ecosystem. It handles the lifecycle of data ingestion, indicator updates, and execution for both live trading and backtesting.

## Key Properties

| Property | Description |
| :--- | :--- |
| `IsBacktesting` | A boolean flag indicating if the flow is currently running in a backtest environment. Instances do **not** share this state. |
| `FlowExecutionContext` | Contains contextual metadata (e.g., `FlowMeta`) required for the flow's execution. Specific to each instance. |
| `ScreenDatapoints` | A dictionary of screened technical data (OHLCV) keyed by symbol, used by the flow during execution. |

## Lifecycle of a Flow

The `ExecuteAsync` method (implemented in `FlowBase`) manages the continuous loop of a flow:

1. **Backtest Precheck**: If configured, it performs a dry-run backtest.
2. **Data Generation**: Calls `GenerateScreenedDatapoints()` to fetch fresh market data.
3. **Indicator Update**: Calls `UpdateIndicators()` to recalculate technical metrics.
4. **Execution**: Iterates through symbols and calls `ExecuteLive()` for symbols with fresh data.

## Creating a New Flow

To implement a new trading strategy, inherit from `FlowBase` and implement the following abstract members:

### 1. Define the Flow Name
```csharp
public override string Name { get; set; } = "MyStrategyFlow";
```

### 2. Market Data Ingestion
Implement `GenerateScreenedDatapoints()` to fetch data using an `IDataVendor`.
```csharp
protected override async Task<bool> GenerateScreenedDatapoints() {
    // Fetch data and populate ScreenDatapoints
}
```

### 3. Indicator Calculation
Implement `UpdateIndicators()` to transform raw OHLCV data into technical indicators (e.g., EMA, RSI).
```csharp
protected override void UpdateIndicators(IList<OlhcvDatapoint> datapoints) {
    // Use Skender.Stock.Indicators or custom logic
}
```

### 4. Strategy Logic (Live & Backtest)
Implement `ExecuteLive()` for the core decision-making logic and `Backtest()` to simulate historical performance.

```csharp
public override async Task ExecuteLive(
    IList<OlhcvDatapoint> datapoints, 
    OlhcvDatapoint currentDatapoint, 
    IList<CallResponse>? callResponses, 
    string? symbol) 
{
    // Make Buy/Sell decisions using current indicators
}
```

## Usage Example

Flows are typically instantiated via Dependency Injection and managed by the `FlowsManager`. When executing, you can pass a `FlowExecutionContext`:

```csharp
var context = new FlowExecutionContext().Add(new FlowMeta { ... });
await myFlow.ExecuteAsync(context, cancellationToken);
```

> [!IMPORTANT]
> Always use `FlowExecutionContext` to pass metadata like Flow IDs to ensure results are correctly associated in the repository.
