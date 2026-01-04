namespace OpAn.App.MorphTrade.Abstractions.Core;

/// <summary>
/// Interface for UserInfo injection.
/// </summary>
public interface IUserInfo
{
	/// <summary>
	/// User identifier.
	/// </summary>
	public string UserId { get; set; }

	/// <summary>
	/// Name of the user.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Roles the user can target to.
	/// </summary>
	public IList<string> Roles { get; set; }
}
