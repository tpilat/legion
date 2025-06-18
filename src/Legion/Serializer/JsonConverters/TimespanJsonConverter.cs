#if NET5_0

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Legion.Serializer;

/// <summary>
/// The new Json.NET doesn't support Timespan at this time
/// https://github.com/dotnet/corefx/issues/38641
/// </summary>
public class TimespanJsonConverter : System.Text.Json.Serialization.JsonConverter<TimeSpan>
{
	/// <summary>
	/// Format: Days.Hours:Minutes:Seconds:Milliseconds
	/// </summary>
	public const string TimeSpanFormatString = @"d\.hh\:mm\:ss\.FFFFFFF";

	public override TimeSpan Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
	{
		var s = reader.GetString();
		if (string.IsNullOrWhiteSpace(s))
			return TimeSpan.Zero;

		if (!TimeSpan.TryParseExact(s, TimeSpanFormatString, null, out var parsedTimeSpan))
			throw new FormatException($"Input timespan is not in an expected format : expected {System.Text.RegularExpressions.Regex.Unescape(TimeSpanFormatString)}. Please retrieve this key as a string and parse manually.");

		return parsedTimeSpan;
	}

	public override void Write(System.Text.Json.Utf8JsonWriter writer, TimeSpan value, System.Text.Json.JsonSerializerOptions options)
	{
		var timespanFormatted = $"{value.ToString(TimeSpanFormatString)}";

		if (timespanFormatted.EndsWith("."))
			timespanFormatted = $"{timespanFormatted}0000000";
		else if (0 < value.Milliseconds)
			timespanFormatted = $"{timespanFormatted}0000";

		writer.WriteStringValue(timespanFormatted);
	}
}
#endif
