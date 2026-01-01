namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// Manager contract would allow a few predefined aspects of the program.
/// </summary>
public interface IManager<TManagedObject>
{
	/// <summary>
	///	Provides a baseline managed object list.
	/// </summary>
	/// <returns></returns>
	public Task<IList<TManagedObject>?> GetManagedObjectsAsync();

	/// <summary>
	/// Manages an object in the runtime memory.
	/// </summary>
	/// <param name="managedObject"></param>
	/// <returns></returns>
	public Task<TManagedObject> ManageObjectAsync(TManagedObject managedObject);
}
