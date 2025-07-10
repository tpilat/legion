using Legion.ExpressionsSerializer.Interfaces;
using Legion.ExpressionsSerializer.Nodes;
using System.Runtime.Serialization;

namespace Legion.ExpressionsSerializer.Serializers;

public abstract class DataSerializer : SerializerBase, ISerializer
{
	public virtual void Serialize<T>(Stream stream, T obj) where T : Node
	{
		if (stream == null)
			throw new ArgumentNullException(nameof(stream));

		var serializer = CreateXmlObjectSerializer(typeof(T));
		serializer.WriteObject(stream, obj);
	}

	public virtual T Deserialize<T>(Stream stream) where T : Node
	{
		if (stream == null)
			throw new ArgumentNullException(nameof(stream));

		var serializer = CreateXmlObjectSerializer(typeof(T));
		return (T)serializer.ReadObject(stream);
	}

	protected abstract XmlObjectSerializer CreateXmlObjectSerializer(Type type);
}
