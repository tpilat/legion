namespace Legion.Serializer.JsonConverters;

public class NewtonsoftPolymorphicConverter<TClass, TInterface> : Newtonsoft.Json.JsonConverter<TInterface>
	where TClass : class, TInterface
{
	public override bool CanWrite => false;

	public override TInterface? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, TInterface? existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
		=> serializer.Deserialize<TClass>(reader);

	public override void WriteJson(Newtonsoft.Json.JsonWriter writer, TInterface? value, Newtonsoft.Json.JsonSerializer serializer)
		=> serializer.Serialize(writer, value);
}
