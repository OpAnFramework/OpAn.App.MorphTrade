# MorphTrade.Domain.Ingestion.Tests

Unit and integration tests for data vendor implementations.

## Purpose

Validates data ingestion functionality, vendor contract compliance, and data transformation logic.

## Test Coverage

- **IngestionDomainDataVendorContractTests** - Contract compliance tests for data vendors
- Vendor authentication flows
- Data retrieval operations
- OLHCV data parsing and validation
- Vendor-specific implementations

## Running Tests

```bash
dotnet test tests/MorphTrade.Domain.Ingestion.Tests
```

## Framework

Uses MSTest for test execution. Tests validate both Fyers and Alpaca vendor implementations against the `IDataVendor` contract.
