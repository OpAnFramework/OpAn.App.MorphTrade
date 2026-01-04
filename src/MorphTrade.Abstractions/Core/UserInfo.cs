using Microsoft.Extensions.Options;

namespace OpAn.App.MorphTrade.Abstractions.Core;

/// <summary>
/// Provides user information for the program operations.
/// </summary>
public class UserInfo: IUserInfo
{
	/// <inheritdoc />
	public required string UserId { get; set; }

	/// <inheritdoc />
	public required string Name { get; set; }

	/// <inheritdoc />
	public required IList<string> Roles { get; set; }
}
