using OpAn.App.MorphTrade.Domain.Finance.Bank.Entities;
using OpAn.App.MorphTrade.Domain.Finance.Bank.Enums;

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

	///  <summary>
	///  Provides a crediting mechanism.
	///  </summary>
	///  <param name="amount">Amount to be credited.</param>
	///  <param name="description">Description of the Transaction.</param>
	///  <returns>Returns created transaction.</returns>
	public Transaction Credit(double amount, string description);

	/// <summary>
	/// Provides a debiting mechanism.
	/// </summary>
	///  <param name="amount">Amount to be credited.</param>
	///  <param name="description">Description of the Transaction.</param>
	///  <returns>Returns created transaction.</returns>
	public Transaction Debit(double amount, string description);
}
