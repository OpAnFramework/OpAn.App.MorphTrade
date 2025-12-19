namespace OpAn.App.MorphTrade.Domain.Finance.Ledger.ErpNext.Extensions;

/// <summary>
/// This would provide us options to be read from the ERP company.
/// </summary>
public class ErpOptions
{
	/// <summary>
	/// Backend protocol over which the ERP operates. Defaults to 'HTTP'.
	/// </summary>
	public required string BackendProtocol { get; set; } = "http";

	/// <summary>
	/// A backend host for the ERP backend.
	/// </summary>
	public required string BackendHost {get;set;}

	/// <summary>
	/// A backend port for the ERP backend.
	/// </summary>
	public required int BackendPort {get;set;}

	/// <summary>
	/// A backend base endpoint to assemble the backend calls.
	/// </summary>
	public required string BackendBaseEndpoint {get;set;}

	/// <summary>
	/// Company name that we are subjecting when handling ERP Transactions.
	/// </summary>
	public required string CompanyName { get; set; }

	/// <summary>
	/// Information set for the cash account. This would be used to handle trades (credit and debit).
	/// </summary>
	public required string CashAccount { get; set; }

	/// <summary>
	/// Information set for the broker account. This represents the cash held by the broker.
	/// </summary>
	public required string BrokerAccount { get; set; }

	/// <summary>
	/// Information set for the realised profit and loss accounts.
	/// </summary>
	public string? RealisedPnLAccount { get; set; }

	/// <summary>
	/// Information set for the unrealised profit and loss accounts.
	/// </summary>
	public string? UnrealisedPnLAccount { get; set; }
}
