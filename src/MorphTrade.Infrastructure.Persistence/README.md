# MorphTrade.Infrastructure.Persistence

Infrastructure layer for database persistence configuration.

## Purpose

Provides database configuration, connection management, and persistence infrastructure setup for the MorphTrade framework.

## Key Components

- **Extension/DatabaseExtensions** - DI registration for database services
- Database connection string management
- EF Core configuration helpers

## Features

- SQLite database configuration
- Connection string management from configuration
- Centralized persistence setup

## Usage

Register persistence services:

```csharp
builder.Services.AddPersistenceDb(configuration);
```

This configures the database context and connection for use across domain projects.

## Dependencies

- Entity Framework Core
- SQLite provider
