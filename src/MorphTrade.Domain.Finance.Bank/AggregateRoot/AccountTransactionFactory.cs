using OpAn.App.MorphTrade.Domain.Finance.Bank.Entities;
using OpAn.App.MorphTrade.Domain.Finance.Bank.Enums;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.AggregateRoot;

/// <summary>
/// Allows creating trnasactions based on an account.
/// </summary>
public static class AccountTransactionFactory
{

	/// <summary>
	/// Allows creating a transaction that is attached to an account.
	/// </summary>
	/// <param name="account">Account entitled for the transaction.</param>
	/// <param name="amount">Amount associated with the transaction.</param>
	/// <param name="transactionType">Transaction type associated.</param>
	/// <param name="reference">Reference string attached to the transaction.</param>
	/// <returns></returns>
	public static Transaction CreateTransaction(
		this Account account,
		double amount,
		TransactionType transactionType,
		string? reference
		)
	{
		Transaction transaction = new Transaction()
		{
			Id = Guid.NewGuid().ToString(),
			// Could be needed for UTC
			Timestamp = DateTime.Now.ToUniversalTime(),
			AccountId = account.Id,
			Amount = amount,
			TransactionType = transactionType,
			Reference = reference,
			Currency = account.Currency
		};

		account.RecordTransaction(transaction);

		return transaction;
	}
}
