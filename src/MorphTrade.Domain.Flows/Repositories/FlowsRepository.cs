using OpAn.App.MorphTrade.Abstractions.Flows;
using OpAn.App.MorphTrade.Domain.Flows.Entities;

namespace OpAn.App.MorphTrade.Domain.Flows.Repositories;

/// <summary>
/// A flows repository for the Database actions.
/// </summary>
public class FlowsRepository: IFlowsRepository
{
	/// <inheritdoc />
	public Task<FlowMeta?> GetFlowById(string id)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<FlowMeta[]?> GetFlowByName(string name)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<FlowMeta?> CreateFlow(string name, string description)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<CallResponseEvent?> AddCallResponse(FlowMeta flow, CallResponse callResponse)
	{
		throw new NotImplementedException();
	}
}
