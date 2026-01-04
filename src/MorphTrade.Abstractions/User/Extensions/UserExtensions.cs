using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpAn.App.MorphTrade.Abstractions.Core;

namespace OpAn.App.MorphTrade.Abstractions.User.Extensions;

/// <summary>
/// Allows performing user level service extensions.
/// </summary>
public static class UserExtensions
{
	/// <summary>
	/// Allows adding user information to the service container.
	/// </summary>
	/// <param name="services">Service container to be extended.</param>
	/// <param name="userInfo">User information to be added to the service container.</param>
	/// <returns></returns>
	public static IServiceCollection AddUserInfo(
		this IServiceCollection services,
		IUserInfo? userInfo)
	{
		if (userInfo is null)
		{
			throw new ArgumentNullException(nameof(userInfo));
		}
		services.AddSingleton<IUserInfo>(userInfo);
		return services;
	}

	/// <summary>
	/// Ensures that the correct user info is added to the service container.
	/// </summary>
	/// <param name="services">Service container to be extended.</param>
	/// <param name="configuration">Configuration to be followed.</param>
	/// <param name="userInfo">User information to be added to the service container.</param>
	/// <returns></returns>
	public static IServiceCollection EnsureUserInfo(
		this IServiceCollection services,
		IConfiguration configuration,
		IUserInfo? userInfo = null)
	{
		bool? isNoAuth = configuration.GetSection("NoAuth").Get<bool>();

		if (isNoAuth.HasValue && isNoAuth.Value)
		{
			AddUserInfo(
				services,
				new UserInfo()
				{
					Name = "DEFAULT_USER",
					Roles = new List<string>(),
					UserId = "DEFAULT_USERID"
				});
			return services;
		}

		// Default behaviour
		AddUserInfo(services, userInfo);
		return services;
	}
}
