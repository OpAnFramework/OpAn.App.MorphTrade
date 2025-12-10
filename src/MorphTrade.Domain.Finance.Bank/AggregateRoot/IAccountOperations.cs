using OpAn.App.MorphTrade.Domain.Finance.Entities.Bank;
using OpAn.App.MorphTrade.Domain.Finance.Enums.Bank;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.AggregateRoot;

/// <summary>
/// An interface for the accounting operations.
/// </summary>
public interface IAccountOperations
{
	/// <summary>
	/// Should provide current balance of the account.
	/// </summary>
	/// <returns></returns>
	public double GetBalance();

	/// <summary>
	/// Provides a crediting mechanism.
	/// </summary>
	/// <param name="transaction">
	///		A transaction strictly associated to <seealso cref="TransactionType.Deposit"/>.</param>
	/// <returns>Returns true if successful. Else returns false.</returns>
	public bool Credit(Transaction transaction);

	/// <summary>
	/// Provides a debiting mechanism.
	/// </summary>
	/// <param name="transaction">
	///		A transaction strictly associated to <seealso cref="TransactionType.Deposit"/>.</param>
	/// <returns>Returns true if successful. Else returns false.</returns>
	public bool Debit(Transaction transaction);
}
