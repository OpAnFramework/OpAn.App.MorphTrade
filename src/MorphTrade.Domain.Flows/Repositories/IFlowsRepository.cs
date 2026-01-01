using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Domain.Flows.Entities;

namespace OpAn.App.MorphTrade.Domain.Flows.Repositories;
/// <summary>
/// A flow management repository for the database features.
/// </summary>
public interface IFlowsRepository
{
	/// <summary>
	/// Provides the flow with an identifier.
	/// </summary>
	/// <param name="id">Identifier to be matched.</param>
	/// <returns></returns>
	public Task<FlowMeta?> GetFlowByIdAsync(string id);

	/// <summary>
	/// Provides a list of flows with matching names.
	/// </summary>
	/// <param name="name">Name to be matched.</param>
	/// <returns></returns>
	public Task<IList<FlowMeta>?> GetFlowsByNameAsync(string name);

	/// <summary>
	/// Creates a new flow.
	/// </summary>
	/// <returns></returns>
	public Task<FlowMeta?> CreateFlowAsync(
		FlowMeta flow);

	/// <summary>
	/// Create or update the flow in the database.
	/// </summary>
	/// <param name="flow"></param>
	/// <returns></returns>
	public Task<FlowMeta?> CreateUpdateFlowAsync(FlowMeta flow);

	/// <summary>
	/// Adds a call response to the flow.
	/// </summary>
	/// <param name="flow">Flow to be registered for the call response.</param>
	/// <param name="callResponse">Call response to be added to the flow.</param>
	/// <param name="isBacktesting">Adds a flag to make sure that it is registered as backtesting option.</param>
	/// <returns></returns>
	public Task<CallResponseEvent?> AddCallResponseAsync(
		FlowMeta flow,
		CallResponse callResponse,
		bool isBacktesting = false
		);

	/// <summary>
	/// Provides all Call response events.
	/// </summary>
	/// <param name="flow">Flow on which the responses are called.</param>
	/// <returns></returns>
	public Task<IList<CallResponseEvent>?> GetAllCallResponseEventsAsync(FlowMeta flow);

	/// <summary>
	/// Remove a flow information from the database entries.
	/// </summary>
	/// <param name="flow">Flows to be flushed.</param>
	/// <returns></returns>
	public Task<(FlowMeta? flowInfo, IList<CallResponseEvent>? callResponseEvents)> RemoveFlowAsync(FlowMeta flow);
}
