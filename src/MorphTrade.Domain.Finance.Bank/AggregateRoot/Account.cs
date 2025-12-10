using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Domain.Finance.Entities.Bank;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.AggregateRoot;

/// <summary>
/// Aggregate root for the account model.
/// </summary>
public class Account: Entity, IAccountOperations
{
	/// <summary>
	/// An account name for the account.
	/// </summary>
	public required string AccountName { get; set; }

	/// <summary>
	/// Funds associated with the account.
	/// </summary>
	public required double Funds { get; set; }

	/// <inheritdoc />
	public double GetBalance()
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public bool Credit(Transaction transaction)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public bool Debit(Transaction transaction)
	{
		throw new NotImplementedException();
	}
}
