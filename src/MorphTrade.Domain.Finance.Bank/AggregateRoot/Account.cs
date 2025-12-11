using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Domain.Finance.Bank.Entities;
using OpAn.App.MorphTrade.Domain.Finance.Bank.Enums;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.AggregateRoot;

/// <summary>
/// Aggregate root for the account model.
/// </summary>
[Table("Accounts")]
public class Account: Entity, IAccountOperations
{
	/// <summary>
	/// An account name for the account.
	/// </summary>
	public required string AccountName { get; init; }

	/// <summary>
	/// Specifies currency associated with the account.
	/// </summary>
	public required CurrencyType Currency { get; init; }

	/// <summary>
	/// Funds associated with the account.
	/// </summary>
	[PublicAPI]
	public required double Funds { get; set; }

	private readonly List<Transaction> _transactions = new ();

	/// <summary>
	/// Publicly accessible Transactions list.
	/// </summary>
	public IReadOnlyList<Transaction> Transactions => _transactions;

	/// <inheritdoc />
	public double GetBalance() => Funds;

	/// <inheritdoc />
	public Transaction RecordTransaction(Transaction transaction)
	{
		_transactions.Add(transaction);
		return transaction;
	}

	/// <inheritdoc />
	public Transaction Credit(double amount, string description)
	{
		if (amount <= 0)
			throw new ArgumentException("Deposit amount must be positive.");

		// Update the Aggregate Root's state (Balance)
		Funds += amount;
		return this.CreateTransaction(amount, TransactionType.Deposit, description);
	}

	/// <inheritdoc />
	public Transaction Debit(double amount, string description)
	{
		if (amount <= 0)
			throw new ArgumentException("Debit amount must be positive.");
		if (amount > Funds)
			throw new ArgumentException("Debit amount must be less than Funds.");

		// 1. Update the Aggregate Root's state (Balance)
		Funds -= amount;

		return this.CreateTransaction(amount, TransactionType.Withdraw, description);
	}
}
