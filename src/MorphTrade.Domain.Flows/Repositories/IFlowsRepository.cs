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
	public Task<FlowMeta?> GetFlowById(string id);

	/// <summary>
	/// Provides a list of flows with matching names.
	/// </summary>
	/// <param name="name">Name to be matched.</param>
	/// <returns></returns>
	public Task<FlowMeta[]?> GetFlowByName(string name);

	/// <summary>
	/// Creates a new flow.
	/// </summary>
	/// <returns></returns>
	public Task<FlowMeta?> CreateFlow(
		string name,
		string description);

	/// <summary>
	/// Adds a call response to the flow.
	/// </summary>
	/// <param name="flow">Flow to be registered for the call response.</param>
	/// <param name="callResponse">Call response to be added to the flow.</param>
	/// <returns></returns>
	public Task<CallResponseEvent?> AddCallResponse(
		FlowMeta flow,
		CallResponse callResponse
		);
}
