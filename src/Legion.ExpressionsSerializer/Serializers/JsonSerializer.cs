using Legion.ExpressionsSerializer.Interfaces;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Legion.ExpressionsSerializer.Serializers;

public class JsonSerializer : TextSerializer, IJsonSerializer
{
	protected override XmlObjectSerializer CreateXmlObjectSerializer(Type type)
	{
		return new DataContractJsonSerializer(type, GetKnownTypes());
	}
}
