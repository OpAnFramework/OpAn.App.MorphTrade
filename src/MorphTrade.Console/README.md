# MorphTrade.Console

Console application entry point for the MorphTrade framework.

## Purpose

Provides a runnable console application that orchestrates data vendor configuration, dependency injection, database migrations, and hosted service execution.

## Features

- Multi-vendor data ingestion setup (Fyers, Alpaca)
- Configuration management via `appsettings.json`
- Database migration execution on startup
- Dependency injection container setup
- Logging infrastructure

## Running

```bash
dotnet run --project src/MorphTrade.Console/MorphTrade.Console.csproj
```

For Fyers authentication:
```bash
dotnet run --project src/MorphTrade.Console/MorphTrade.Console.csproj --auth YOUR_AUTH_CODE
```

## Configuration

Edit `appsettings.json` to configure your data vendor (Fyers or Alpaca) with appropriate credentials.
