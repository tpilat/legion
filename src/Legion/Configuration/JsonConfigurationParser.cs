#if NET5_0_OR_GREATER

using Legion.Text;
using System.Diagnostics;
using System.Text.Json;

namespace Legion.Configuration;

public sealed class JsonConfigurationParser
{
	private int _currentPropertyIndex = 0;
	private readonly Dictionary<string, ValueWithType> _data = new(StringComparer.OrdinalIgnoreCase);
	private readonly Stack<string> _paths = new();
	private readonly bool _writeValueTypes;
	private readonly string? _bindTo;
	private readonly string _keyDelimiter;
	private readonly bool _nullValuesTransformToEmptyString;

	private JsonConfigurationParser(bool writeValueTypes, string? bindTo = null, string ? keyDelimiter = null, bool nullValuesTransformToEmptyString = true)
	{
		_writeValueTypes = writeValueTypes;
		_bindTo = bindTo;
		_keyDelimiter = keyDelimiter ?? Microsoft.Extensions.Configuration.ConfigurationPath.KeyDelimiter;
		_nullValuesTransformToEmptyString = nullValuesTransformToEmptyString;
	}

	public static Dictionary<string, ValueWithType> Parse(string json, bool writeValueTypes, string? bindTo = null, string? keyDelimiter = null, bool nullValuesTransformToEmptyString = true)
		=> new JsonConfigurationParser(writeValueTypes, bindTo, keyDelimiter, nullValuesTransformToEmptyString).ParseStream(json);

	public static Dictionary<string, ValueWithType> Parse(Stream input, bool writeValueTypes, string? bindTo = null, string? keyDelimiter = null, bool nullValuesTransformToEmptyString = true)
		=> new JsonConfigurationParser(writeValueTypes, bindTo, keyDelimiter, nullValuesTransformToEmptyString).ParseStream(input);

	public static Task<Dictionary<string, ValueWithType>> ParseAsync(Stream utf8Json, bool writeValueTypes, string? bindTo = null, string? keyDelimiter = null, bool nullValuesTransformToEmptyString = true)
	{
		var parser = new JsonConfigurationParser(writeValueTypes, bindTo, keyDelimiter, nullValuesTransformToEmptyString);
		return parser.ParseStreamAsync(utf8Json);
	}

	private Dictionary<string, ValueWithType> ParseStream(string json)
	{
		Throw.IfArgumentNullOrWhiteSpace(json);

		var jsonDocumentOptions = new JsonDocumentOptions
		{
			CommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true,
		};

		using (JsonDocument doc = JsonDocument.Parse(json, jsonDocumentOptions))
		{
			if (doc.RootElement.ValueKind != JsonValueKind.Object)
			{
				throw new FormatException(string.Format("Top-level JSON element must be an object. Instead, '{0}' was found.", doc.RootElement.ValueKind));
			}
			VisitObjectElement(doc.RootElement);
		}

		return _data;
	}

	private Dictionary<string, ValueWithType> ParseStream(Stream input)
	{
		Throw.IfArgumentNull(input);

		var jsonDocumentOptions = new JsonDocumentOptions
		{
			CommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true,
		};

		using (var reader = new StreamReader(input))
		using (JsonDocument doc = JsonDocument.Parse(reader.ReadToEnd(), jsonDocumentOptions))
		{
			if (doc.RootElement.ValueKind != JsonValueKind.Object)
			{
				throw new FormatException(string.Format("Top-level JSON element must be an object. Instead, '{0}' was found.", doc.RootElement.ValueKind));
			}
			VisitObjectElement(doc.RootElement);
		}

		return _data;
	}

	private async Task<Dictionary<string, ValueWithType>> ParseStreamAsync(Stream utf8Json)
	{
		Throw.IfArgumentNull(utf8Json);

		var jsonDocumentOptions = new JsonDocumentOptions
		{
			CommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true,
		};

		using (JsonDocument doc = await JsonDocument.ParseAsync(utf8Json, jsonDocumentOptions))
		{
			if (doc.RootElement.ValueKind != JsonValueKind.Object)
			{
				throw new FormatException(string.Format("Top-level JSON element must be an object. Instead, '{0}' was found.", doc.RootElement.ValueKind));
			}
			VisitObjectElement(doc.RootElement);
		}

		return _data;
	}

	private void VisitObjectElement(JsonElement element)
	{
		var isEmpty = true;

		foreach (JsonProperty property in element.EnumerateObject())
		{
			isEmpty = false;
			EnterContext(property.Name);
			VisitValue(property.Value);
			ExitContext();
		}

		SetNullIfElementIsEmpty(isEmpty, false);
	}

	private void VisitArrayElement(JsonElement element)
	{
		int index = 0;

		foreach (JsonElement arrayElement in element.EnumerateArray())
		{
			EnterContext(index.ToString());
			VisitValue(arrayElement);
			ExitContext();
			index++;
		}

		SetNullIfElementIsEmpty(isEmpty: index == 0, true);
	}

	private void SetNullIfElementIsEmpty(bool isEmpty, bool isArray)
	{
		if (isEmpty && 0 < _paths.Count)
		{
			string key = StringHelper.ConcatIfNotNullOrEmpty(_bindTo, _keyDelimiter, _paths.Peek());
			_data[key] =
				new ValueWithType(
					_nullValuesTransformToEmptyString ? "" : null,
					null,
					_currentPropertyIndex++);
		}
	}

	private void VisitValue(JsonElement value)
	{
		Debug.Assert(0 < _paths.Count);

		switch (value.ValueKind)
		{
			case JsonValueKind.Object:
				VisitObjectElement(value);
				break;

			case JsonValueKind.Array:
				VisitArrayElement(value);
				break;

			case JsonValueKind.Number:
			case JsonValueKind.String:
			case JsonValueKind.True:
			case JsonValueKind.False:
			case JsonValueKind.Null:
				string key = StringHelper.ConcatIfNotNullOrEmpty(_bindTo, _keyDelimiter, _paths.Peek());

				if (_data.ContainsKey(key))
					throw new FormatException(string.Format("A duplicate key '{0}' was found.", key));

				_data[key] = JsonConfigurationHelper.GetValue(_writeValueTypes, _nullValuesTransformToEmptyString, value, _currentPropertyIndex++);
				break;

			default:
				throw new FormatException(string.Format("Unsupported JSON token '{0}' was found.", value.ValueKind));
		}
	}

	private void EnterContext(string context) =>
		_paths.Push(0 < _paths.Count
			? $"{_paths.Peek()}{_keyDelimiter}{context}"
			: context);

	private void ExitContext() => _paths.Pop();
}

#endif
