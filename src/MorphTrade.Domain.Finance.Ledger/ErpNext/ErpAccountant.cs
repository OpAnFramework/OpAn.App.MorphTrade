using OpAn.App.MorphTrade.Abstractions.Finance;

namespace OpAn.App.MorphTrade.Domain.Finance.Ledger.ErpNext;

/// <summary>
/// An ERP accountant would use ERPNext framework for managing the ledger.
/// </summary>
/// <remarks>
///	We use Frappe Framework as the primary toolchain dependency for the ledger management
///		in this context.
/// <seealso ref="https://frappe.io/framework"/>
/// </remarks>
public class ErpAccountant: IAccountant
{
	/// <inheritdoc />
	public Task CreatePurchaseOrder(Ticker ticker)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task CreateSellOrder(Ticker ticker)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task TrackTickers(IList<Ticker> tickers)
	{
		throw new NotImplementedException();
	}
}
