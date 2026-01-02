namespace OpAn.App.MorphTrade.Abstractions.IngestionDomain.Exceptions;

/// <summary>
/// A base exception for the data vendors.
/// </summary>
public class DataVendorError: Exception
{
	private const string ErrorPrefix = $"{nameof(DataVendorError)}: ";
	/// <inheritdoc />
	protected DataVendorError()
	{

	}

	/// <inheritdoc />
	protected DataVendorError(string message)
		: base(ErrorPrefix + message)
	{

	}


	/// <inheritdoc />
	protected DataVendorError(string message, Exception? inner)
		: base(
			ErrorPrefix + message,
			inner)
	{

	}

}
