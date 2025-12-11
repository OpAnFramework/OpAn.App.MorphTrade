# MorphTrade.Domain.Finance.Bank.Tests

Unit and integration tests for the banking domain.

## Purpose

Validates banking domain functionality including account management, transaction processing, and database persistence.

## Test Coverage

- **TestPersistentAccountDatabase** - Database integration tests
- **TestPersistentAccountStorage** - Account storage and retrieval tests
- Account creation and validation
- Transaction processing
- Repository operations

## Running Tests

```bash
dotnet test tests/MorphTrade.Domain.Finance.Bank.Tests
```

## Framework

Uses MSTest for test execution with EF Core in-memory database for integration testing.
