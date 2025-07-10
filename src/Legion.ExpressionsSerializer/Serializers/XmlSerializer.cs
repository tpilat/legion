using Legion.ExpressionsSerializer.Interfaces;
using System.Runtime.Serialization;

namespace Legion.ExpressionsSerializer.Serializers;

public class XmlSerializer : TextSerializer, IXmlSerializer
{
	protected override XmlObjectSerializer CreateXmlObjectSerializer(Type type)
	{
		return new DataContractSerializer(type, GetKnownTypes());
	}
}
