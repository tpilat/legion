using Legion.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Legion.Configuration;

public sealed class JsonConfigurationParser_Newtonsoft
{
	private int _currentPropertyIndex = 0;
	private readonly Dictionary<string, ValueWithType> _data = new(StringComparer.OrdinalIgnoreCase);
	private readonly Stack<string> _paths = new();
	private readonly bool _writeValueTypes;
	private readonly string? _bindTo;
	private readonly string _keyDelimiter;
	private readonly bool _nullValuesTransformToEmptyString;

	private JsonConfigurationParser_Newtonsoft(bool writeValueTypes, string? bindTo = null, string? keyDelimiter = null, bool nullValuesTransformToEmptyString = true)
	{
		_writeValueTypes = writeValueTypes;
		_bindTo = bindTo;
		_keyDelimiter = keyDelimiter ?? Microsoft.Extensions.Configuration.ConfigurationPath.KeyDelimiter;
		_nullValuesTransformToEmptyString = nullValuesTransformToEmptyString;
	}

	public static Dictionary<string, ValueWithType> Parse(string json, bool writeValueTypes, string? bindTo = null, string? keyDelimiter = null, bool nullValuesTransformToEmptyString = true)
		=> new JsonConfigurationParser_Newtonsoft(writeValueTypes, bindTo, keyDelimiter, nullValuesTransformToEmptyString).ParseJson(json);

	public static Dictionary<string, ValueWithType> Parse(JsonReader reader, JsonLoadSettings? settings, bool writeValueTypes, string? bindTo = null, string? keyDelimiter = null, bool nullValuesTransformToEmptyString = true)
		=> new JsonConfigurationParser_Newtonsoft(writeValueTypes, bindTo, keyDelimiter, nullValuesTransformToEmptyString).ParseJson(reader, settings);

	public static Task<Dictionary<string, ValueWithType>> ParseAsync(JsonReader reader, JsonLoadSettings? settings, bool writeValueTypes, string? bindTo = null, string? keyDelimiter = null, bool nullValuesTransformToEmptyString = true, CancellationToken cancellationToken = default)
	{
		var parser = new JsonConfigurationParser_Newtonsoft(writeValueTypes, bindTo, keyDelimiter, nullValuesTransformToEmptyString);
		return parser.ParseJsonAsync(reader, settings, cancellationToken);
	}

	private Dictionary<string, ValueWithType> ParseJson(string json)
	{
		Throw.IfArgumentNullOrWhiteSpace(json);

		var jobject = JObject.Parse(json, new JsonLoadSettings
		{
			CommentHandling = CommentHandling.Ignore,
			DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Error
		});

		VisitObjectElement(jobject);

		return _data;
	}

	private Dictionary<string, ValueWithType> ParseJson(JsonReader reader, JsonLoadSettings? settings)
	{
		Throw.IfArgumentNull(reader);

		var jobject = JObject.Load(reader, settings);

		VisitObjectElement(jobject);

		return _data;
	}

	private async Task<Dictionary<string, ValueWithType>> ParseJsonAsync(JsonReader reader, JsonLoadSettings? settings, CancellationToken cancellationToken)
	{
		Throw.IfArgumentNull(reader);

		var jobject = await JObject.LoadAsync(reader, settings, cancellationToken);

		VisitObjectElement(jobject);

		return _data;
	}

	private void VisitObjectElement(JObject jobject)
	{
		var isEmpty = true;

		foreach (var kvp in jobject)
		{
			isEmpty = false;
			EnterContext(kvp.Key);
			VisitValue(kvp.Value);
			ExitContext();
		}

		SetNullIfElementIsEmpty(isEmpty, false);
	}

	private void VisitArrayElement(JArray jarray)
	{
		int index = 0;

		foreach (var arrayElement in jarray)
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

	private void VisitValue(JToken? jtoken)
	{
		Debug.Assert(0 < _paths.Count);

		if (jtoken is JObject jobject)
		{
			VisitObjectElement(jobject);
			return;
		}
		else if (jtoken is JArray jarray)
		{
			VisitArrayElement(jarray);
			return;
		}
		else
		{
			StringHelper.ConcatIfNotNullOrEmpty(_bindTo, _keyDelimiter, _paths.Peek());
			string key = StringHelper.ConcatIfNotNullOrEmpty(_bindTo, _keyDelimiter, _paths.Peek());

			if (_data.ContainsKey(key))
				throw new FormatException(string.Format("A duplicate key '{0}' was found.", key));

			_data[key] = JsonConfigurationHelper.GetValue(_writeValueTypes, _nullValuesTransformToEmptyString, jtoken, _currentPropertyIndex++);

			return;
		}

		throw new FormatException(string.Format("Unsupported JSON token '{0}' was found.", jtoken?.Type.ToString() ?? "NULL"));
	}

	private void EnterContext(string context) =>
		_paths.Push(0 < _paths.Count
			? $"{_paths.Peek()}{_keyDelimiter}{context}"
			: context);

	private void ExitContext() => _paths.Pop();
}
