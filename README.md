# MorphTrade

**MorphTrade** is a flexible, vendor-agnostic algorithmic trading framework built in C# (.NET 9.0) that provides a robust foundation for ingesting financial market data and applying technical indicators for trading analysis.

## Overview

MorphTrade abstracts away the complexity of working with multiple financial data vendors, providing a unified interface for accessing market data and building trading strategies. The framework follows clean architecture principles, making it easy to extend with new data sources, indicators, and trading strategies.

## Features

- **Multi-Vendor Data Ingestion**: Seamlessly switch between different market data providers
  - Fyers (Indian markets)
  - Alpaca (US markets)
- **Technical Indicators**: Extensible indicator system for technical analysis
- **Type-Safe Data Models**: Strongly-typed data points (OLHC, Volume, OLHCV)
- **Authentication Management**: Built-in authentication handling for data vendors
- **Dependency Injection**: Modern .NET hosting with DI throughout
- **Centralized Package Management**: Consistent versioning across all projects

## Architecture

The project is organized into several layers:

```
MorphTrade/
├── src/
│   ├── MorphTrade.Abstractions/      # Core interfaces and contracts
│   ├── MorphTrade.Domain.Ingestion/  # Data vendor implementations
│   ├── MorphTrade.Domain.Flows/      # Trading indicators and strategies
│   └── MorphTrade.Console/           # Console application entry point
└── workspace/
    └── IndicatorDesign/              # Experimental indicator testing
```

### Core Abstractions

- **`IDataVendor`**: Contract for market data providers
- **`IIndicator`**: Contract for technical indicators
- **`IAuthenticator`**: Contract for vendor authentication
- **Data Models**: `OlhcDatapoint`, `VolumeDatapoint`, `OlhcvDatapoint`

## Requirements

- **.NET 9.0 SDK** or later
- **Data Vendor Credentials** (Fyers or Alpaca)
  - Fyers: Client ID, Secret Key, Redirect URI
  - Alpaca: API Key ID, API Key Secret

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd MorphTrade
```

### 2. Configure Data Vendor

Edit `src/MorphTrade.Console/appsettings.json` to configure your preferred data vendor:

**For Alpaca:**
```json
{
  "DataVendor": "Alpaca",
  "AlpacaDataVendorOptions": {
    "BaseAddress": "https://data.alpaca.markets/v2/",
    "ApiKeyId": "YOUR_ALPACA_KEY_ID",
    "ApiKeySecret": "YOUR_ALPACA_SECRET"
  }
}
```

**For Fyers:**
```json
{
  "DataVendor": "Fyers",
  "FyersAuthenticatorOptions": {
    "ClientId": "YOUR_CLIENT_ID",
    "SecretKey": "YOUR_SECRET_KEY",
    "RedirectUri": "https://trade.fyers.in/api-login/redirect-uri/index.html"
  }
}
```

### 3. Fyers Authentication (If Using Fyers)

If you're using Fyers as your data vendor, the authentication process requires an additional step due to OAuth flow:

**First Run - Generate Auth Code:**
```bash
dotnet run --project src/MorphTrade.Console/MorphTrade.Console.csproj
```

This will:
- Open your browser for Fyers authentication
- Prompt you to log in and authorize the application
- Display an auth code in the redirect URL

**Second Run - Provide Auth Code:**
```bash
dotnet run --project src/MorphTrade.Console/MorphTrade.Console.csproj --auth YOUR_AUTH_CODE
```

Replace `YOUR_AUTH_CODE` with the code obtained from the first run. The application will then:
- Exchange the auth code for an access token
- Proceed with normal operation

> [!NOTE]
> The `--auth` flag is **only required for Fyers**. Alpaca uses API keys directly from `appsettings.json` and doesn't require this additional authentication step.

### 4. Build the Solution

```bash
dotnet build MorphTrade.sln
```

### 5. Run the Console Application

**For Alpaca:**
```bash
dotnet run --project src/MorphTrade.Console/MorphTrade.Console.csproj
```

**For Fyers (after initial auth):**
```bash
dotnet run --project src/MorphTrade.Console/MorphTrade.Console.csproj --auth YOUR_AUTH_CODE
```

## Usage Examples

### Fetching Market Data

```csharp
// Inject IDataVendor in your service
public class TradingService
{
    private readonly IDataVendor _dataVendor;
    
    public TradingService(IDataVendor dataVendor)
    {
        _dataVendor = dataVendor;
    }
    
    public async Task<IList<OlhcvDatapoint>> GetHistoricalData()
    {
        return await _dataVendor.GetOlhcvData(
            index: "NSE",           // or "sip" for Alpaca
            stock: "RELIANCE",      // or "AAPL" for Alpaca
            observationTime: DateTime.UtcNow,
            timespan: TimeSpan.FromDays(30),
            timeframe: TimeSpan.FromMinutes(5)
        );
    }
}
```

### Using Technical Indicators

```csharp
var indicator = new YourIndicator();
var historicData = await _dataVendor.GetOlhcvData(...);
var currentValue = historicData.Last();

var (values, signal) = indicator.Indicate(
    currentValue,
    historicData,
    ("parameterName", parameterValue)
);
```

## Extending the Framework

### Adding a New Data Vendor

1. Create a new implementation of `IDataVendor` in `MorphTrade.Domain.Ingestion`
2. Implement authentication if needed (`IAuthenticator`)
3. Add configuration options
4. Register in the DI container via extension method

### Adding a New Indicator

1. Create a new class implementing `IIndicator` in `MorphTrade.Domain.Flows`
2. Implement the `Indicate` method with your indicator logic
3. Register in the DI container if needed

## Project Structure

- **MorphTrade.Abstractions**: Core interfaces and domain models
- **MorphTrade.Domain.Ingestion**: Data vendor implementations and authentication
- **MorphTrade.Domain.Flows**: Technical indicators and trading strategies
- **MorphTrade.Console**: Console application for running trading operations
- **IndicatorDesign**: Workspace for testing and designing new indicators

## Dependencies

Key packages used:
- `Microsoft.Extensions.Hosting` - Hosting and DI
- `Skender.Stock.Indicators` - Technical analysis library
- `fyers-api-v3` - Fyers SDK
- `JetBrains.Annotations` - Code annotations

## Contribution

Contributions are welcome! Please follow these guidelines:

1. **Fork the repository** and create a feature branch
2. **Follow the existing architecture** patterns and naming conventions
3. **Add tests** for new functionality
4. **Update documentation** as needed
5. **Submit a pull request** with a clear description of changes

### Code Standards

- Use C# 12.0 language features
- Enable implicit usings
- Treat warnings as errors
- Generate XML documentation for public APIs
- Follow the existing namespace pattern: `OpAn.App.MorphTrade.*`

## License

See `LICENSE.rst` for details.

## Roadmap

Future enhancements planned:
- [ ] Additional technical indicators
- [ ] Backtesting framework
- [ ] Strategy composition and execution
- [ ] Real-time data streaming
- [ ] Order execution capabilities
- [ ] Risk management modules
- [ ] Performance analytics and reporting

## Support

For issues, questions, or contributions, please open an issue on the repository.
