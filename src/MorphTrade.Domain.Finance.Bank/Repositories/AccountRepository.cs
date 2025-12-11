using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpAn.App.MorphTrade.Domain.Finance.Bank.AggregateRoot;

namespace OpAn.App.MorphTrade.Domain.Finance.Bank.Repositories;

/// <summary>
/// Account repository for the database.
/// </summary>
public class AccountRepository: IAccountRepository
{
	private readonly BankDbContext _dbContext;
	private readonly ILogger<AccountRepository> _logger;

	/// <summary>
	/// Accounts repository constructor.
	/// </summary>
	/// <param name="context">Database context.</param>
	/// <param name="logger">Logger for the Accounts repository.</param>
	public AccountRepository(
		BankDbContext context,
		ILogger<AccountRepository> logger)
	{
		_dbContext = context;
		_logger = logger;
	}

	/// <inheritdoc />
	public async Task<Account?> GetByIdAsync(string id)
	{
		_logger.LogDebug("Retrieving account with ID: {ID}", id);
		return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == id);
	}

	/// <inheritdoc />
	public async Task<IEnumerable<Account>> GetAllAsync()
	{
		_logger.LogDebug("Retrieving all accounts.");
		return await _dbContext.Accounts.ToListAsync();
	}

	/// <inheritdoc />
	public async Task<IEnumerable<Account>> GetAccountsByNameAsync(string name)
	{
		_logger.LogDebug("Retrieving all accounts by name: {NAME}", name);
		return await _dbContext.Accounts
			.Where(a => a.AccountName == name).ToListAsync();
	}

	/// <inheritdoc />
	public async Task Add(Account account)
	{
		_logger.LogDebug("Adding account with ID: {ID}", account.Id);
		await _dbContext.Accounts.AddAsync(account);
		await _dbContext.SaveChangesAsync();
	}

	/// <inheritdoc />
	public async Task Update(Account account)
	{
		_logger.LogDebug("Updating account with ID: {ID}", account.Id);
		_dbContext.Accounts.Update(account);
		await _dbContext.SaveChangesAsync();
	}

	/// <inheritdoc />
	public async Task Remove(Account account)
	{
		_logger.LogDebug("Removing account with ID: {ID}", account.Id);
		_dbContext.Accounts.Remove(account);
		await _dbContext.SaveChangesAsync();
	}
}
