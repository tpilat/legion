using Legion.Extensions;
using Legion.Serializer;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Legion.Configuration;

public static class ConfigurationToJsonConverter
{
	public const string JSON_DICT_EXTENSION = ".json_dict";

	public static Dictionary<string, ValueWithType> ReadJsonDictionary(string dataFilePath)
	{
		Throw.IfArgumentNullOrWhiteSpace(dataFilePath);

		var dataJson = File.ReadAllText(dataFilePath, new UTF8Encoding(false));
		var dictObj = JsonSerializerHelper.Deserialize<Dictionary<string, ValueWithType>>(dataJson);
		return dictObj!;
	}

	public static string? ToJsonString(Dictionary<string, ValueWithType> data, bool readValueTypes, string? keyDelimiter = null!, bool emptyStringTransformToNull = true, bool orderProperties = false)
	{
		var traverser = new Traverser(readValueTypes, keyDelimiter, emptyStringTransformToNull);
		var objectElement = traverser.Traverse(data);

		objectElement?.BuildJsonTokens(orderProperties);

		return objectElement?.JObject.ToString();
	}

	private abstract class AbstractElement
	{
		public AbstractElement? Parent { get; set; }
		public string? FullPath { get; }
		public string? Name { get; }

		protected AbstractElement(AbstractElement? parent, string? fullPath, string? name)
		{
			Parent = parent;
			FullPath = fullPath;
			Name = name ?? string.Empty;
		}

		public abstract void BuildJsonTokens(bool orderProperties);
	}

	private class ObjectElement : AbstractElement
	{
		public List<AbstractElement> Values { get; }
		public JObject JObject { get; set; }

		public ObjectElement(AbstractElement? parent, string? fullPath, string? name)
			: base(parent, fullPath, name ?? string.Empty)
		{
			Values = [];
		}

		public override void BuildJsonTokens(bool orderProperties)
		{
			JObject = new JObject();

			IEnumerable<AbstractElement> values = orderProperties
				? Values.OrderBy(x => x.FullPath)
				: Values;

			foreach (var value in values)
			{
				if (value is ObjectElement oe)
				{
					oe.BuildJsonTokens(orderProperties);
					JObject.Add(new JProperty(oe.Name!, oe.JObject));
				}
				else if (value is ArrayElement ae)
				{
					ae.BuildJsonTokens(orderProperties);
					JObject.Add(new JProperty(ae.Name!, ae.JArray));
				}
				else if (value is ValueElement ve)
				{
					ve.BuildJsonTokens(orderProperties);
					JObject.Add(new JProperty(ve.Name!, ve.JValue));
				}
			}
		}

		public ObjectElement AddObject(string name, string keyDelimiter)
		{
			Throw.IfArgumentNullOrWhiteSpace(name);

			var fullPath = $"{FullPath}{keyDelimiter}{name}";

			var objectElement = Values.FirstOrDefault(p => p.Name == name);
			if (objectElement == null)
			{
				var oe = new ObjectElement(this, fullPath, name);
				objectElement = oe;
				Values.Add(objectElement);
				return oe;
			}
			else
			{
				if (objectElement.FullPath != fullPath)
					Throw.InvalidOperationException($"Cannot add {nameof(ObjectElement)} with name = {name} | Expected {nameof(FullPath)} = {fullPath} | Found property.{nameof(FullPath)} = {objectElement.FullPath}");

				if (objectElement is ObjectElement oe)
					return oe;
				else
					Throw.InvalidOperationException($"Cannot add {nameof(ObjectElement)} with name = {name} of type = {objectElement.GetType().ToFriendlyFullName()}");

				return null;
			}
		}

		public ArrayElement AddArray(string name, string keyDelimiter)
		{
			Throw.IfArgumentNullOrWhiteSpace(name);

			var fullPath = $"{FullPath}{keyDelimiter}{name}";

			var arrayElement = Values.FirstOrDefault(p => p.Name == name);
			if (arrayElement == null)
			{
				var ae = new ArrayElement(this, fullPath, name);
				arrayElement = ae;
				Values.Add(arrayElement);
				return ae;
			}
			else
			{
				if (arrayElement.FullPath != fullPath)
					Throw.InvalidOperationException($"Cannot add {nameof(ArrayElement)} with name = {name} | Expected {nameof(FullPath)} = {fullPath} | Found property.{nameof(FullPath)} = {arrayElement.FullPath}");

				if (arrayElement is ArrayElement ae)
					return ae;
				else
					Throw.InvalidOperationException($"Cannot add {nameof(ArrayElement)} with name = {name} of type = {arrayElement.GetType().ToFriendlyFullName()}");

				return null;
			}
		}

		public ValueElement AddValue(string name, string keyDelimiter, ValueWithType valueWithType, bool readValueTypes, bool emptyStringTransformToNull)
		{
			Throw.IfArgumentNullOrWhiteSpace(name);

			var fullPath = $"{FullPath}{keyDelimiter}{name}";

			var valueElement = Values.FirstOrDefault(p => p.Name == name);
			if (valueElement == null)
			{
				var ve = new ValueElement(this, fullPath, name, valueWithType, readValueTypes, emptyStringTransformToNull);
				valueElement = ve;
				Values.Add(valueElement);
				return ve;
			}
			else
			{
				Throw.InvalidOperationException($"Cannot add duplicated {nameof(ValueElement)} with name = {name} | {nameof(FullPath)} = {fullPath}");

				return null;
			}
		}

		public override string ToString()
		{
			return $"{nameof(ObjectElement)}: {FullPath} = values count {Values.Count}";
		}
	}

	private class ArrayElement : AbstractElement
	{
		public Dictionary<int, AbstractElement> Values { get; }
		public JArray JArray { get; set; }

		public ArrayElement(AbstractElement? parent, string? fullPath, string? name)
			: base(parent, fullPath, name ?? string.Empty)
		{
			Values = [];
		}

		public override void BuildJsonTokens(bool orderProperties)
		{
			JArray = new JArray();

			foreach (var key in Values.Keys.OrderBy(x => x))
			{
				var value = Values[key];
				if (value is ObjectElement oe)
				{
					oe.BuildJsonTokens(orderProperties);
					JArray.Add(oe.JObject);
				}
				else if (value is ArrayElement ae)
				{
					ae.BuildJsonTokens(orderProperties);
					JArray.Add(ae.JArray);
				}
				else if (value is ValueElement ve)
				{
					ve.BuildJsonTokens(orderProperties);
					JArray.Add(ve.JValue);
				}
			}
		}

		public ObjectElement AddObject(int index, string keyDelimiter)
		{
			var fullPath = $"{FullPath}{keyDelimiter}{index}";

			Values.TryGetValue(index, out var objectElement);

			if (objectElement == null)
			{
				var oe = new ObjectElement(this, fullPath, null);
				objectElement = oe;
				Values.Add(index, objectElement);
				return oe;
			}
			else
			{
				if (objectElement.FullPath != fullPath)
					Throw.InvalidOperationException($"Cannot add {nameof(ObjectElement)} with index = {index} | Expected {nameof(FullPath)} = {fullPath} | Found property.{nameof(FullPath)} = {objectElement.FullPath}");

				if (objectElement is ObjectElement oe)
					return oe;
				else
					Throw.InvalidOperationException($"Cannot add {nameof(ObjectElement)} with index = {index} of type = {objectElement.GetType().ToFriendlyFullName()}");

				return null;
			}
		}

		public ArrayElement AddArray(int index/*, string name*/, string keyDelimiter)
		{
			var fullPath = $"{FullPath}{keyDelimiter}{index}";

			Values.TryGetValue(index, out var arrayElement);

			if (arrayElement == null)
			{
				var ae = new ArrayElement(this, fullPath, null);
				arrayElement = ae;
				Values.Add(index, arrayElement);
				return ae;
			}
			else
			{
				if (arrayElement.FullPath != fullPath)
					Throw.InvalidOperationException($"Cannot add {nameof(ArrayElement)} with index = {index} | Expected {nameof(FullPath)} = {fullPath} | Found property.{nameof(FullPath)} = {arrayElement.FullPath}");

				if (arrayElement is ArrayElement ae)
					return ae;
				else
					Throw.InvalidOperationException($"Cannot add {nameof(ArrayElement)} with index = {index} of type = {arrayElement.GetType().ToFriendlyFullName()}");

				return null;
			}
		}

		public ValueElement AddValue(int index, string keyDelimiter, ValueWithType valueWithType, bool readValueTypes, bool emptyStringTransformToNull)
		{
			var fullPath = $"{FullPath}{keyDelimiter}{index}";

			Values.TryGetValue(index, out var valueElement);

			if (valueElement == null)
			{
				var ve = new ValueElement(this, fullPath, null, valueWithType, readValueTypes, emptyStringTransformToNull);
				valueElement = ve;
				Values.Add(index, valueElement);
				return ve;
			}
			else
			{
				Throw.InvalidOperationException($"Cannot add duplicated {nameof(ValueElement)} with index = {index} | {nameof(FullPath)} = {fullPath}");

				return null;
			}
		}

		public override string ToString()
		{
			return $"{nameof(ArrayElement)}: {FullPath} = values count {Values.Count}";
		}
	}

	private class ValueElement : AbstractElement
	{
		public object? Value { get; }
		public JValue JValue { get; set; }

		public ValueElement(AbstractElement? parent, string? fullPath, string? name, ValueWithType valueWithType, bool readValueTypes, bool emptyStringTransformToNull)
			: base(parent, fullPath, name ?? string.Empty)
		{
			Value = ConvertValue(valueWithType, readValueTypes, emptyStringTransformToNull);
		}

		public override void BuildJsonTokens(bool orderProperties)
		{
			JValue = new JValue(Value);
		}

		private static object? ConvertValue(ValueWithType valueWithType, bool readValueTypes, bool emptyStringTransformToNull)
		{
			if (valueWithType == null)
				return null;

			if (valueWithType.Value == string.Empty)
			{
				if (emptyStringTransformToNull)
					return null;
				else
					return string.Empty;
			}

			if (readValueTypes)
				return JsonConfigurationHelper.ConvertToObejct(valueWithType, emptyStringTransformToNull);

			if (long.TryParse(valueWithType.Value, out var longValue))
				return longValue;

			if (bool.TryParse(valueWithType.Value, out var boolValue))
				return boolValue;

			if (decimal.TryParse(valueWithType.Value, out var decimalValue))
				return decimalValue;

			if (DateTime.TryParse(valueWithType.Value, out var dateTimeValue))
				return dateTimeValue;

			return valueWithType.Value; //string
		}

		public override string ToString()
		{
			return $"{nameof(ValueElement)}: {FullPath} = {Value}";
		}
	}

	private class Traverser
	{
		private readonly bool _readValueTypes;
		private readonly string[] _keyDelimiterSplitter = null!;
		private readonly bool _emptyStringTransformToNull;

		private readonly string _keyDelimiter;
		private readonly ObjectElement _root;

		public Traverser(bool readValueTypes, string? keyDelimiter = null!, bool emptyStringTransformToNull = true)
		{
			_readValueTypes = readValueTypes;
			_keyDelimiter = keyDelimiter ?? Microsoft.Extensions.Configuration.ConfigurationPath.KeyDelimiter;
			_keyDelimiterSplitter = [_keyDelimiter];
			_emptyStringTransformToNull = emptyStringTransformToNull;

			_root = new ObjectElement(null, "", "");
		}

		[return: NotNullIfNotNull(nameof(data))]
		public ObjectElement? Traverse(Dictionary<string, ValueWithType> data)
		{
			if (data == null)
				return null;

			foreach (var kvp in data.OrderBy(x => x.Value.Order))
				AddValue(kvp.Key, kvp.Value);

			return _root;
		}

		private void AddValue(string path, ValueWithType valueWithType)
		{
			var split = path.Split(_keyDelimiterSplitter, StringSplitOptions.None);
			ObjectElement? objectElement = _root;
			ArrayElement? arrayElement = null;
			int? index = null;

			var valueWasSet = false;
			for (int i = 0; i < split.Length; i++)
			{
				if (i < split.Length - 1)
				{
					var name = split[i];
					var next = split[i + 1];
					if (int.TryParse(next, out var idx))
					{
						index = idx;
						if (objectElement != null)
						{
							if (i == split.Length - 2)
							{
								arrayElement = objectElement.AddArray(name, _keyDelimiter);
								arrayElement.AddValue(index.Value, _keyDelimiter, valueWithType, _readValueTypes, _emptyStringTransformToNull);
								valueWasSet = true;
								break;
							}
							else
							{
								var ae = objectElement.AddArray(name, _keyDelimiter);

								var isMultiArray = i + 2 < split.Length && int.TryParse(split[i + 2], out _);
								if (isMultiArray)
								{
									arrayElement = ae.AddArray(idx, _keyDelimiter);
									objectElement = null;
									i++; //skip index name
								}
								else
								{
									arrayElement = null;
									objectElement = ae.AddObject(idx, _keyDelimiter);
									i++; //skip index name
								}
							}
						}
						else
						{
							arrayElement = arrayElement!.AddArray(idx, _keyDelimiter);
							objectElement = null;
						}
					}
					else
					{
						if (objectElement != null)
						{
							index = null;
							objectElement = objectElement.AddObject(name, _keyDelimiter);
							arrayElement = null;
						}
						else
						{
							idx = int.Parse(name);
							objectElement = arrayElement!.AddObject(idx, _keyDelimiter);
							arrayElement = null;
						}
					}
				}
				else
				{
					if (i == split.Length - 1)
					{
						if (objectElement != null)
						{
							var name = split[i];
							objectElement.AddValue(name, _keyDelimiter, valueWithType, _readValueTypes, _emptyStringTransformToNull);
							valueWasSet = true;
						}
						else
						{
							if (arrayElement == null)
								Throw.InvalidOperationException($"{nameof(arrayElement)} == null | {nameof(path)} = {path}");

							if (!index.HasValue)
								Throw.InvalidOperationException($"{nameof(index)} == null | {nameof(path)} = {path}");

							var name = split[i];
							if (name == index.ToString())
							{
								arrayElement.AddValue(index.Value, _keyDelimiter, valueWithType, _readValueTypes, _emptyStringTransformToNull);
								valueWasSet = true;
							}
							else
							{
								var oe = arrayElement!.AddObject(index.Value, _keyDelimiter);
								oe.AddValue(name, _keyDelimiter, valueWithType, _readValueTypes, _emptyStringTransformToNull);
								valueWasSet = true;
							}
						}
					}
					else
					{
						Throw.InvalidOperationException($"{nameof(index)} = {index} out of range | {nameof(path)} = {path}");
					}
				}
			}

			if (!valueWasSet)
				Throw.InvalidOperationException($"Value {valueWithType.Value} (with type = {valueWithType.Type?.Name}) was not set. | {nameof(path)} = {path}");
		}
	}
}
