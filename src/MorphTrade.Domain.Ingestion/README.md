# MorphTrade.Domain.Ingestion

Data vendor implementations for market data ingestion.

## Purpose

Implements concrete data vendor integrations for fetching historical and real-time market data from multiple sources.

## Supported Vendors

### Fyers (Indian Markets)
- OAuth-based authentication flow
- Historical OLHCV data retrieval
- NSE/BSE market support

### Alpaca (US Markets)  
- API key authentication
- Historical stock data
- US equity markets

## Key Components

- **DataVendorFyers/** - Fyers API implementation
- **DataVendorAlpaca/** - Alpaca API implementation  
- **Extensions/** - DI registration helpers

## Usage

Register vendors in your DI container:

```csharp
builder.AddFyersDataVendor();
// or
builder.AddAlpacaExtensions();
```

Inject `IDataVendor` to fetch market data in your services.
