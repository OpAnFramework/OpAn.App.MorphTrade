using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpAn.App.MorphTrade.Abstractions.Finance;
using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Domain.Flows.Entities;

namespace OpAn.App.MorphTrade.Domain.Flows.Repositories;

/// <summary>
/// A flows repository for the Database actions.
/// </summary>
public class FlowsRepository: IFlowsRepository
{
	private readonly ILogger<FlowsRepository> _logger;
	private readonly FlowsDbContext _flowsDbContext;

	/// <summary>
	/// Constructor for the Flows Repository.
	/// </summary>
	/// <param name="logger">Injected logger.</param>
	/// <param name="flowsDbContext">Injected database context.</param>
	public FlowsRepository(
		ILogger<FlowsRepository> logger,
		FlowsDbContext flowsDbContext)
	{
		_logger	= logger;
		_flowsDbContext = flowsDbContext;
	}

	/// <inheritdoc />
	public async Task<IList<FlowMeta>?> GetAllFlowsAsync()
	{
		_logger.LogDebug("Retrieving all flows");
		return await _flowsDbContext.Flows.ToListAsync();
	}

	/// <inheritdoc />
	public async Task<FlowMeta?> GetFlowByIdAsync(string id)
	{
		_logger.LogDebug("Retrieving flow with ID: {ID}", id);
		return await _flowsDbContext.Flows.FirstOrDefaultAsync(f => f.Id == id);
	}

	/// <inheritdoc />
	public async Task<IList<FlowMeta>?> GetFlowsByNameAsync(string name)
	{
		_logger.LogDebug("Retrieving all flows with name: {NAME}", name);
		return await _flowsDbContext.Flows.Where(f => f.Name == name).ToArrayAsync();
	}

	/// <inheritdoc />
	public async Task<FlowMeta?> CreateFlowAsync(FlowMeta flow)
	{
		_logger.LogDebug("Creating new flow with name: {NAME}", flow.Name);
		await _flowsDbContext.Flows.AddAsync(flow);
		await _flowsDbContext.SaveChangesAsync();
		return flow;
	}

	/// <inheritdoc />
	public async Task<FlowMeta?> CreateUpdateFlowAsync(FlowMeta flow)
	{
		_logger.LogDebug("Updating new flow with name: {NAME}", flow.Name);
		FlowMeta? existingFlow = await _flowsDbContext
			.Flows
			.FirstOrDefaultAsync(f => f.Id == flow.Id);

		if (existingFlow == null)
		{
			return await CreateFlowAsync(flow);
		}

		var properties = typeof(FlowMeta).GetProperties()
			.Where(p => p.CanWrite && p.Name != nameof(flow.Id));

		foreach (var property in properties)
		{
			property
				.SetValue(existingFlow, property.GetValue(flow));
		}

		await _flowsDbContext.SaveChangesAsync();
		return existingFlow;
	}

	/// <inheritdoc />
	public async Task<CallResponseEvent?> AddCallResponseAsync(
		FlowMeta flow,
		CallResponse callResponse,
		bool isBacktesting = false)
	{
		CallResponseEvent callResponseEvent = new CallResponseEvent()
		{
			UserId = flow.UserId,
			Id = Guid.NewGuid().ToString(),
			Ticker = callResponse.Ticker,
			FlowId = flow.Id,
			Timestamp = DateTimeOffset.FromUnixTimeSeconds(callResponse.Timestamp).UtcDateTime,
			TradeCall = callResponse.TradeCall,
			TradeCallInfo = (TradeCallInfoEvent) callResponse.TradeCallInfo,
			IsBacktesting = isBacktesting
		};

		_logger.LogDebug(
			"Adding new call response with ID: {ID};" +
			"To the flow with ID: {flowId}",
			callResponseEvent.Id,
			callResponseEvent.FlowId);

		await _flowsDbContext.CallResponseEvents.AddAsync(callResponseEvent);
		await _flowsDbContext.SaveChangesAsync();
		return callResponseEvent;
	}

	/// <inheritdoc />
	public async Task<CallResponseEvent?> CreateUpdateCallResponseEventAsync(FlowMeta flow, CallResponse callResponse, bool isBacktesting = false)
	{
		_logger.LogDebug("Updating the call response for {FlowId}", flow.Id);
		CallResponseEvent? responseEvent = await _flowsDbContext
			.CallResponseEvents
			.FirstOrDefaultAsync(response =>
				response.FlowId == flow.Id
				&& response.Timestamp
					== DateTimeOffset.FromUnixTimeSeconds(callResponse.Timestamp).UtcDateTime
				&& response.IsBacktesting == isBacktesting
				&& response.TradeCall == callResponse.TradeCall
				&& response.Ticker.Symbol == callResponse.Ticker.Symbol);

		// TODO: Update CallResponseEvent logic
		if (responseEvent is null)
		{
			return await AddCallResponseAsync(flow, callResponse, isBacktesting);
		}
		return responseEvent;
	}

	/// <inheritdoc />
	public async Task<IList<CallResponseEvent>?> GetAllCallResponseEventsAsync(FlowMeta flow)
	{
		_logger.LogDebug("Retrieving all call response events from the flow with ID: {ID}", flow.Id);
		return await _flowsDbContext
			.CallResponseEvents
			.Where(e => e.FlowId == flow.Id)
			.ToListAsync();
	}

	/// <inheritdoc />
	public async Task<(FlowMeta? flowInfo, IList<CallResponseEvent>? callResponseEvents)> RemoveFlowAsync(FlowMeta flow)
	{
		FlowMeta? flowInfo = _flowsDbContext
			.Flows
			.FirstOrDefault(f => f.Id == flow.Id);

		IList<CallResponseEvent> callResponseEvents = await _flowsDbContext
			.CallResponseEvents
			.Where(e => e.FlowId == flow.Id)
			.ToListAsync();

		_logger.LogDebug("Deleting all call response events and flows information for flow ID: {ID}", flow.Id);
		_flowsDbContext.CallResponseEvents.RemoveRange(callResponseEvents);
		_flowsDbContext.Flows.Remove(flow);
		await _flowsDbContext.SaveChangesAsync();
		return (flowInfo, callResponseEvents);
	}
}
