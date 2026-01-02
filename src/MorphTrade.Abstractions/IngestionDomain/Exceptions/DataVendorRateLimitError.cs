namespace OpAn.App.MorphTrade.Abstractions.IngestionDomain.Exceptions;

/// <summary>
/// Data vendor rate limiting error.
/// </summary>
public class DataVendorRateLimitError: DataVendorError
{
	private const string ErrorPrefix = "DataVendorRateLimitError: ";

	/// <inheritdoc />
	public DataVendorRateLimitError()
	{

	}

	/// <inheritdoc />
	public DataVendorRateLimitError(string message)
		: base(ErrorPrefix + message)
	{

	}

	/// <inheritdoc />
	public DataVendorRateLimitError(string message, Exception? innerException)
		: base(ErrorPrefix + message, innerException)
	{

	}
}
