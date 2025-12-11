using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using OpAn.App.MorphTrade.Abstractions.Core;
using OpAn.App.MorphTrade.Domain.Finance.Bank.Enums;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.Entities;

/// <summary>
/// Base transaction entity that to be handled by the domain
///		MorphTrade.Domain.Finance.Bank.
/// </summary>
[PublicAPI]
[Table("Transactions")]
public class Transaction: Entity
{
	/// <summary>
	/// Timestamp associated with the operation.
	/// </summary>
	public required DateTime Timestamp { get; init; } = DateTime.UtcNow;

	/// <summary>
	///	Transaction type specified for the transaction.
	/// </summary>
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public required TransactionType TransactionType {get; init;}

	/// <summary>
	/// A source account needs to be specified for the
	///		TransactionType.FundTransfer,
	///		TransactionType.Deposit,
	///		TransactionType.Withdraw
	/// </summary>
	public required string AccountId { get; set; }

	/// <summary>
	/// Amount associated with the transaction
	/// </summary>
	public required double Amount { get; set; }

	/// <summary>
	/// A reference string associated to the transaction
	/// </summary>
	public string? Reference { get; set; }

	/// <summary>
	/// Currency associated to the transaction.
	/// </summary>
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CurrencyType Currency { get; set; } = CurrencyType.Rupee;
}
