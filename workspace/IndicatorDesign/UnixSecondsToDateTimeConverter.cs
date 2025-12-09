using System.Text.Json;
using System.Text.Json.Serialization;

namespace SKB.Workspace.IndicatorDesign;

/// <summary>
/// Converts Unix seconds to datetime.
/// </summary>
public class UnixSecondsToDateTimeConverter : JsonConverter<DateTime>
{
	/// <inheritdoc />
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		long seconds = reader.GetInt64();
		return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
	}

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		long seconds = ((DateTimeOffset)value).ToUnixTimeSeconds();
		writer.WriteNumberValue(seconds);
	}
}
