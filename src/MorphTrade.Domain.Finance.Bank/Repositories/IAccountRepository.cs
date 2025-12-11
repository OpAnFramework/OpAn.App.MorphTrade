using OpAn.App.MorphTrade.Domain.Finance.Bank.AggregateRoot;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.Repositories;

/// <summary>
/// An account repository for the database operations.
/// </summary>
public interface IAccountRepository
{
	/// <summary>
	/// Retrieves an account based on the ID.
	/// </summary>
	/// <param name="id">Account identifier.</param>
	/// <returns>Retrieved account.</returns>
	Task<Account?> GetByIdAsync(string id);

	/// <summary>
	/// Returns all the accounts in the database.
	/// </summary>
	/// <returns>Provides all accounts.</returns>
	Task<IEnumerable<Account>> GetAllAsync();

	/// <summary>
	/// Provides an account based on the name of the account.
	/// </summary>
	/// <param name="name">Account name to search for.</param>
	/// <returns>Accounts with the holding name.</returns>
	Task<IEnumerable<Account>> GetAccountsByNameAsync(string name);

	/// <summary>
	/// Allows adding an Account.
	/// </summary>
	/// <param name="account">Account to be added.</param>
	Task Add(Account account);

	/// <summary>
	/// Allows updating an account.
	/// </summary>
	/// <param name="account">Account to be updated.</param>
	Task Update(Account account);

	/// <summary>
	/// Allows removing an account.
	/// </summary>
	/// <param name="account">Account to be removed.</param>
	Task Remove(Account account);
}
