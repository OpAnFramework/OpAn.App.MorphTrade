namespace OpAn.App.MorphTrade.Abstractions.Core;

/// <summary>
/// Registry for object types.
/// </summary>
/// <typeparam name="TObject">Type of object to be managed.</typeparam>
public interface IRegistry<in TObject>
{
	/// <summary>
	/// Registers an instance of the given TObject to the registry.
	/// </summary>
	/// <param name="instance">Instance to be registered.</param>
	void Register(TObject instance);
}
