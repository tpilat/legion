using Legion.Extensions;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Legion.Configuration;

public static class JsonConfigurationHelper
{
	public const string IGNORED_VALUE = "##IGNORE##";
	public const string REMOVE_VALUE = "##REMOVE##";

	public static Type LONG_TYPE = typeof(long);
	public static Type BOOL_TYPE = typeof(bool);
	public static Type DECIMAL_TYPE = typeof(decimal);
	public static Type DATETIME_TYPE = typeof(DateTime);
	public static Type STRING_TYPE = typeof(string);

	//private const string LONG = "::long";
	//private const string BOOL = "::bool";
	//private const string DECIMAL = "::num";
	//private const string DATETIME = "::datetime";
	//private const string STRING = "::str";

	//public static Type? GetValueType(string value)
	//{
	//	if (string.IsNullOrWhiteSpace(value))
	//		return null;

	//	if (value.EndsWith(LONG, StringComparison.InvariantCultureIgnoreCase))
	//		return LONG_TYPE;

	//	if (value.EndsWith(BOOL, StringComparison.InvariantCultureIgnoreCase))
	//		return BOOL_TYPE;

	//	if (value.EndsWith(DECIMAL, StringComparison.InvariantCultureIgnoreCase))
	//		return DECIMAL_TYPE;

	//	if (value.EndsWith(DATETIME, StringComparison.InvariantCultureIgnoreCase))
	//		return DATETIME_TYPE;

	//	if (value.EndsWith(STRING, StringComparison.InvariantCultureIgnoreCase))
	//		return STRING_TYPE;

	//	return null;
	//}

	//public static string? RemoveValueType(string value)
	//{
	//	if (string.IsNullOrWhiteSpace(value))
	//		return value;

	//	string? trimmedValue;
	//	if (value.TryTrimPostfix(LONG, out trimmedValue, true))
	//		return trimmedValue;

	//	if (value.TryTrimPostfix(BOOL, out trimmedValue, true))
	//		return trimmedValue;

	//	if (value.TryTrimPostfix(DECIMAL, out trimmedValue, true))
	//		return trimmedValue;

	//	if (value.TryTrimPostfix(DATETIME, out trimmedValue, true))
	//		return trimmedValue;

	//	if (value.TryTrimPostfix(STRING, out trimmedValue, true))
	//		return trimmedValue;

	//	return null;
	//}

	//public static bool TryParseValueAndType(string value, out string? parsedValue, out Type? type)
	//{
	//	parsedValue = value;
	//	type = null;

	//	if (string.IsNullOrWhiteSpace(value))
	//		return false;

	//	if (value.TryTrimPostfix(LONG, out parsedValue, true))
	//	{
	//		type = LONG_TYPE;
	//		return true;
	//	}

	//	if (value.TryTrimPostfix(BOOL, out parsedValue, true))
	//	{
	//		type = BOOL_TYPE;
	//		return true;
	//	}

	//	if (value.TryTrimPostfix(DECIMAL, out parsedValue, true))
	//	{
	//		type = DECIMAL_TYPE;
	//		return true;
	//	}

	//	if (value.TryTrimPostfix(DATETIME, out parsedValue, true))
	//	{
	//		type = DATETIME_TYPE;
	//		return true;
	//	}

	//	if (value.TryTrimPostfix(STRING, out parsedValue, true))
	//	{
	//		type = STRING_TYPE;
	//		return true;
	//	}

	//	return false;
	//}

	public static object? ConvertToObejct(ValueWithType valueWithType, bool emptyStringTransformToNull)
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

		if (valueWithType.Type == STRING_TYPE)
			return valueWithType.Value;

		if (valueWithType.Type == BOOL_TYPE)
		{
			if (bool.TryParse(valueWithType.Value, out var longValue))
				return longValue;

			return valueWithType.Value;
		}

		if (valueWithType.Type == LONG_TYPE)
		{
			if (long.TryParse(valueWithType.Value, out var longValue))
				return longValue;

			return valueWithType.Value;
		}

		if (valueWithType.Type == DECIMAL_TYPE)
		{
			if (decimal.TryParse(valueWithType.Value, out var longValue))
				return longValue;

			return valueWithType.Value;
		}

		if (valueWithType.Type == DATETIME_TYPE)
		{
			if (DateTime.TryParse(valueWithType.Value, out var longValue))
				return longValue;

			return valueWithType.Value;
		}

		return valueWithType.Value;
	}

	public static ValueWithType GetValue(bool writeValueTypes, bool nullValuesTransformToEmptyString, JToken? jtoken, int currentPropertyIndex)
	{
		var valueWithType = new ValueWithType(null, null, currentPropertyIndex);

		if (writeValueTypes)
		{
			if (jtoken != null)
			{
				if (jtoken.Type == JTokenType.Integer)
					valueWithType.Type = LONG_TYPE;
				else if (jtoken.Type == JTokenType.Float)
					valueWithType.Type = DECIMAL_TYPE;
				else if (jtoken.Type == JTokenType.String)
					valueWithType.Type = STRING_TYPE;
				else if (jtoken.Type == JTokenType.Boolean)
					valueWithType.Type = BOOL_TYPE;
				else if (jtoken.Type == JTokenType.Date)
					valueWithType.Type = DATETIME_TYPE;
			}
		}

		if (!nullValuesTransformToEmptyString && jtoken is JValue jvalue)
		{
			valueWithType.Value = jvalue.Value == null ? null : jvalue?.ToString();
		}
		else
		{
			valueWithType.Value = jtoken?.ToString();
		}

		return valueWithType;
	}

#if NET5_0_OR_GREATER

	public static ValueWithType GetValue(bool writeValueTypes, bool nullValuesTransformToEmptyString, System.Text.Json.JsonElement jsonElement, int currentPropertyIndex)
	{
		var valueWithType =
			new ValueWithType(
				(!nullValuesTransformToEmptyString && jsonElement.ValueKind == System.Text.Json.JsonValueKind.Null) ? null : jsonElement.ToString(),
				null,
				currentPropertyIndex);

		if (writeValueTypes)
		{
			var value = jsonElement.ToString();

			if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
				valueWithType.Type = (value?.Any(ch => ch == ',' || ch == '.') == true)
					? DECIMAL_TYPE
					: LONG_TYPE;
			else if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
				valueWithType.Type = STRING_TYPE;
			else if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.True
				|| jsonElement.ValueKind == System.Text.Json.JsonValueKind.False)
				valueWithType.Type = BOOL_TYPE;
		}

		return valueWithType;
	}

#endif

	public static void UpdateConfig(
		Dictionary<string, ValueWithType> config,
		Dictionary<string, ValueWithType> values,
		string keyDelimiter,
		bool addNewKeys,
		bool overwriteExistingKeys,
		bool removeMissingKeys)
	{
		Throw.IfArgumentNull(config);

		if (values == null)
			return;

		values = values.Where(kvp => kvp.Value.Value != IGNORED_VALUE).ToDictionary(k => k.Key, v => v.Value);

		if (removeMissingKeys)
		{
			var keysToRemove = config.Keys.Where(k => !values.ContainsKey(k));
			foreach (var configKey in keysToRemove)
				config.Remove(configKey);
		}

		IEnumerable<KeyValuePair<string, ValueWithType>> vals;

		if (overwriteExistingKeys)
		{
			if (addNewKeys)
			{
				vals = values;
			}
			else
			{
				vals = values.Where(kvp => config.ContainsKey(kvp.Key));
			}
		}
		else
		{
			if (addNewKeys)
			{
				vals = values.Where(kvp => !config.ContainsKey(kvp.Key));
			}
			else
			{
				return; //do nothing
			}
		}

		var allOrders = config.Values.Select(x => x.Order).ToList();
		var maxOrder = 0;
		if (0 < allOrders.Count)
			maxOrder = allOrders.Max();

		foreach (var valuesKvp in vals.OrderBy(x => x.Value.Order))
		{
			if (valuesKvp.Value.Value == REMOVE_VALUE)
			{
				config.TryGetValue(valuesKvp.Key, out var originalValue);

				config.RemoveIfKeyExists(valuesKvp.Key);
				var keyPrefix = $"{valuesKvp.Key}{keyDelimiter}";
				var keysToRemove = config.Keys.Where(k => k.StartsWith(keyPrefix)).ToList();
				foreach (var keyToRemove in keysToRemove)
					config.Remove(keyToRemove);

				var deletedSplit = valuesKvp.Key.Split(new string[] { keyDelimiter }, StringSplitOptions.None);
				var deletedIndexString = deletedSplit.LastOrDefault();
				if (int.TryParse(deletedIndexString, out var deletedIndex))
				{
					var arrayPath = string.Join(keyDelimiter, deletedSplit.Take(deletedSplit.Length - 1));
					var allArrayKeys = config.Keys.Where(k => k.StartsWith(arrayPath)).ToList();

					//there was only one element in the array, write null array
					if (allArrayKeys.Count == 0)
					{
						config.Add(arrayPath, new ValueWithType(null, null, originalValue?.Order ?? ++maxOrder));
					}

					var decrementKeys = new Dictionary<int, List<(string OldKey, string NewKey)>>();
					foreach (var arrayKey in allArrayKeys)
					{
						var arrayKeySplit = arrayKey
							.TrimPrefix(arrayPath)
							.TrimPrefix(keyDelimiter)
							.Split(new string[] { keyDelimiter }, StringSplitOptions.None);

						var arrayKeyIndexString = arrayKeySplit.FirstOrDefault();
						if (int.TryParse(arrayKeyIndexString, out var arrayKeyIndex) && deletedIndex < arrayKeyIndex)
						{
							arrayKeySplit[0] = (arrayKeyIndex - 1).ToString(); //DECREMENT INDEX
							var newKey = $"{arrayPath}{keyDelimiter}{string.Join(keyDelimiter, arrayKeySplit)}";

							if (!decrementKeys.TryAdd(arrayKeyIndex, [(arrayKey, newKey)]))
								decrementKeys[arrayKeyIndex].Add((arrayKey, newKey));
						}
					}

					foreach (var key in decrementKeys.Keys.OrderBy(x => x))
					{
						var keys = decrementKeys[key];
						foreach (var k in keys)
						{
							config.Remove(k.OldKey, out var originalVlaue);
							config.Add(k.NewKey, originalVlaue!);
						}
					}
				}
			}
			else
			{
				if (valuesKvp.Value.Type == null)
					RemoveAllChilds(config, valuesKvp.Key, keyDelimiter);
				else
					RemoveEmptyParents(config, valuesKvp.Key, keyDelimiter);

				if (config.TryGetValue(valuesKvp.Key, out var configValue))
				{
					valuesKvp.Value.Order = configValue.Order;
					config[valuesKvp.Key] = valuesKvp.Value;
				}
				else
				{
					valuesKvp.Value.Order = ++maxOrder;
					config.Add(valuesKvp.Key, valuesKvp.Value);
				}
			}
		}
	}

	private static void RemoveAllChilds(
		Dictionary<string, ValueWithType> config,
		string key,
		string keyDelimiter)
	{
		var exists = config.Keys.Any(k => k == key);
		if (exists)
			config.Remove(key);

		var path = $"{key}{keyDelimiter}";
		foreach (var item in config.Keys.Where(k => k.StartsWith(path)))
		{
			config.Remove(item);
		}
	}

	private static void RemoveEmptyParents(
		Dictionary<string, ValueWithType> config,
		string key,
		string keyDelimiter)
	{
		var split = key.Split([keyDelimiter], StringSplitOptions.None);
		var sb = new StringBuilder();
		var first = true;
		foreach (var item in split)
		{
			if (!first)
				sb.Append(keyDelimiter);

			sb.Append(item);

			first = false;
			var path = sb.ToString();
			if (config.TryGetValue(path, out var configValue)
				&& configValue.Type == null)
			{
				config.Remove(path);
			}
		}
	}

	public static void RemoveAllConfigValues(
		Dictionary<string, ValueWithType> config,
		Dictionary<string, ValueWithType> values)
	{
		Throw.IfArgumentNull(config);

		if (values == null)
			return;

		values = values.Where(kvp => kvp.Key != IGNORED_VALUE).ToDictionary(k => k.Key, v => v.Value);

		foreach (var key in values.Keys.Where(config.ContainsKey))
			config.Remove(key);
	}

	public static void RemoveMissedConfigValues(
		Dictionary<string, ValueWithType> config,
		Dictionary<string, ValueWithType> values)
	{
		Throw.IfArgumentNull(config);

		if (values == null)
			return;

		values = values.Where(kvp => kvp.Key != IGNORED_VALUE).ToDictionary(k => k.Key, v => v.Value);

		var keysToRemove = config.Keys.Where(k => !values.ContainsKey(k));
		foreach (var configKey in keysToRemove)
			config.Remove(configKey);
	}
}
