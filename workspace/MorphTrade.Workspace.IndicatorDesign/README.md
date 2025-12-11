# MorphTrade.Workspace.IndicatorDesign

Experimental workspace for designing and testing technical indicators.

## Purpose

Provides a sandbox environment for prototyping and testing new technical indicators before integrating them into the main framework.

## Features

- Load historical market data from JSON files
- Test indicator calculations on real data
- Prototype new indicator logic
- Convert OLHCV data to indicator-compatible formats

## Usage

1. Place market data in `data.json`
2. Run the workspace project:
   ```bash
   dotnet run --project workspace/MorphTrade.Workspace.IndicatorDesign
   ```
3. Experiment with indicator implementations

## Workflow

Use this workspace to:
- Test indicator algorithms with historical data
- Validate calculations before production use
- Iterate on indicator design quickly
- Debug indicator behavior with real market data

Once validated, move indicators to `MorphTrade.Domain.Flows` for production use.
