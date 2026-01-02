using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// A base class for any object manager.
/// </summary>
public abstract class ObjectManager<TManagedObject> : IManager<TManagedObject>
{
	// A concurrent queue to show the managed objects
	private readonly ConcurrentQueue<TManagedObject> _managedObjects = new ConcurrentQueue<TManagedObject>();

	private readonly ILogger<ObjectManager<TManagedObject>> _logger;

	/// <summary>
	/// Constructor for the object manager.
	/// </summary>
	/// <param name="logger">Injected logger</param>
	protected ObjectManager(
		ILogger<ObjectManager<TManagedObject>> logger)
	{
		_logger = logger;
	}

	/// <inheritdoc />
	public virtual Task<IList<TManagedObject>?> GetManagedObjectsAsync()
	{
		IList<TManagedObject>? managedObjects = null;
		try
		{
			managedObjects = _managedObjects.ToList();
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Failed to get managed objects");
		}
		return Task.FromResult(managedObjects);
	}

	/// <inheritdoc />
	public virtual Task<TManagedObject> ManageObjectAsync(TManagedObject managedObject)
	{
		_managedObjects.Enqueue(managedObject);
		return Task.FromResult(managedObject);
	}
}
