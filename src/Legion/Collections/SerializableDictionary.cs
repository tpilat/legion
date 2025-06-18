using System.Xml.Serialization;

namespace Legion.Collections;

[XmlRoot("dictionary")]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue?>, IXmlSerializable
	where TKey : notnull
{
	public SerializableDictionary()
		: base()
	{
	}

	public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
		: base(dictionary)
	{
	}

	public SerializableDictionary(IEqualityComparer<TKey> comparer)
		: base(comparer)
	{
	}

	public SerializableDictionary(int capacity)
		: base(capacity)
	{
	}

	public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		: base(dictionary, comparer)
	{
	}

	public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
		: base(capacity, comparer)
	{
	}

	public System.Xml.Schema.XmlSchema? GetSchema()
	{
		return null;
	}

	public void ReadXml(System.Xml.XmlReader reader)
	{
		var keySerializer = new XmlSerializer(typeof(TKey));
		var valueSerializer = new XmlSerializer(typeof(TValue));

		bool wasEmpty = reader.IsEmptyElement;
		reader.Read();

		if (wasEmpty)
			return;

		while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
		{
			reader.ReadStartElement("item");

			reader.ReadStartElement("key");

			if (keySerializer.Deserialize(reader) is not TKey key)
			{
				Throw.InvalidOperationException("Missing key");
				return;
			}

			reader.ReadEndElement();

			reader.ReadStartElement("value");

			if (valueSerializer.Deserialize(reader) is not TValue value)
				value = default!;

			reader.ReadEndElement();

			this.Add(key, value);

			reader.ReadEndElement();
			reader.MoveToContent();
		}
		reader.ReadEndElement();
	}

	public void WriteXml(System.Xml.XmlWriter writer)
	{
		var keySerializer = new XmlSerializer(typeof(TKey));
		var valueSerializer = new XmlSerializer(typeof(TValue));

		foreach (var kvp in this)
		{
			writer.WriteStartElement("item");

			writer.WriteStartElement("key");
			keySerializer.Serialize(writer, kvp.Key);
			writer.WriteEndElement();

			writer.WriteStartElement("value");
			TValue? value = kvp.Value;
			valueSerializer.Serialize(writer, value);
			writer.WriteEndElement();

			writer.WriteEndElement();
		}
	}
}
