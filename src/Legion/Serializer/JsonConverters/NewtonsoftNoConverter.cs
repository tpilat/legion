namespace Legion.Serializer.JsonConverters;

/// <summary>
/// Use this converter for any class, if you don't want "inherit" converters from implemented interfaces. The default json converter will be used instead.
/// </summary>
public class NewtonsoftNoConverter : Newtonsoft.Json.JsonConverter
{
	public override bool CanRead => false;
	public override bool CanWrite => false;

	public override bool CanConvert(Type objectType)
		=> throw new NotSupportedException();

	public override object? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		=> throw new NotSupportedException();

	public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
		=> throw new NotSupportedException();
}
