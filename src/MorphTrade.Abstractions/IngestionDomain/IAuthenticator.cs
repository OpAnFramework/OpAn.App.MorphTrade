using JetBrains.Annotations;

namespace OpAn.App.MorphTrade.Abstractions.IngestionDomain;

/// <summary>
/// Baseline authenticator contract.
/// </summary>
[PublicAPI]
public interface IAuthenticator: IDisposable;
