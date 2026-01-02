using JetBrains.Annotations;

namespace OpAn.App.MorphTrade.Abstractions.Flows;

/// <summary>
/// Flow execution context provides information regarding the runtime.
/// </summary>
[PublicAPI]
public class FlowExecutionContext
{
	/// <summary>
	/// Context Identifier.
	/// </summary>
	public readonly string Id = Guid.NewGuid().ToString();

	/// <summary>
	/// Cancellation token could also be attached to the context.
	/// </summary>
	public CancellationToken? CancellationToken { get; set; }

	/// <summary>
	/// An extension bag to get extra information from the context.
	/// </summary>
	private readonly IDictionary<Type, object> _extensions = new Dictionary<Type, object>();

	/// <summary>
	/// Sets an extension in the extension bag.
	/// </summary>
	/// <param name="extension">To be added as a context extension.</param>
	/// <typeparam name="TExt">Type of the extension.</typeparam>
	public void Set<TExt>(
		TExt extension)
		where TExt : notnull
	=> _extensions[typeof(TExt)] = extension;

	/// <summary>
	/// Trys to get an extension based on the Type based out variable.
	/// </summary>
	/// <param name="value">Outwards extension from the method.</param>
	/// <typeparam name="TExt">Type of the extension to get.</typeparam>
	/// <returns></returns>
	public bool TryGet<TExt>(
		out TExt value)
		where TExt : notnull
	{
		if (_extensions.TryGetValue(typeof(TExt), out var @object))
		{
			value = (TExt)@object;
			return true;
		}
		value = default!;
		return false;
	}

	/// <summary>
	/// Provides the required extension.
	/// </summary>
	/// <typeparam name="TExt">Type of the extension.</typeparam>
	/// <returns></returns>
	public TExt GetRequired<TExt>()
		where TExt : notnull
	=> (TExt)_extensions[typeof(TExt)];
}
