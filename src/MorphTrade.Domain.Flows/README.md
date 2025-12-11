# MorphTrade.Domain.Flows

Technical indicators and trading flow implementations.

## Purpose

Provides technical analysis indicators and trading strategy flows for algorithmic trading decisions.

## Current Indicators

- **IndicatorSma** - Simple Moving Average indicator

## Usage

Implement `IIndicator` interface to create custom indicators:

```csharp
var indicator = new IndicatorSma();
var (values, signal) = indicator.Indicate(
    currentValue,
    historicData,
    ("period", 20)
);
```

## Extending

Add new indicators by:
1. Creating a class implementing `IIndicator`
2. Implementing the `Indicate` method with your logic
3. Registering in DI if needed

This domain is designed for extensibility with additional technical indicators and trading strategies.
