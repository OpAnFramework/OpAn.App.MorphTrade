# MorphTrade.Domain.Finance.Bank

Banking domain implementation with account management and persistence.

## Purpose

Manages financial accounts, transactions, and banking operations with Entity Framework Core persistence.

## Key Components

- **AggregateRoot/** - Domain aggregate roots (Account, Transaction)
- **Entities/** - Domain entities
- **Enums/** - Banking-related enumerations (AccountType, TransactionType)
- **Repositories/** - Data access repositories
- **BankDbContext** - EF Core database context
- **Migrations/** - EF Core database migrations

## Features

- Account creation and management
- Transaction tracking
- SQLite database persistence
- Repository pattern for data access
- EF Core migrations support

## Usage

Register in DI container:

```csharp
builder.Services
    .AddPersistenceDb(configuration)
    .AddBankingDomain();
```

Inject repositories to manage accounts and transactions in your services.

## Database

Uses SQLite with EF Core. Migrations are automatically applied on application startup.
