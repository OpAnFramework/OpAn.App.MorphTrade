# MorphTrade.Abstractions

Core interfaces and contracts for the MorphTrade framework.

## Purpose

Defines the foundational abstractions that enable vendor-agnostic data ingestion and indicator implementation across the framework.

## Key Components

- **`IDataVendor`** - Contract for market data providers
- **`IIndicator`** - Contract for technical indicators  
- **`IAuthenticator`** - Contract for vendor authentication
- **Data Models** - `OlhcDatapoint`, `VolumeDatapoint`, `OlhcvDatapoint`
- **HTTP Abstractions** - Base interfaces for HTTP communication
- **Flow Abstractions** - Interfaces for trading flows and strategies

## Dependencies

- JetBrains.Annotations

## Usage

Reference this project when implementing new data vendors, indicators, or extending the framework with custom components.
