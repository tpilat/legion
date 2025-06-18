using Legion.Extensions;
using Legion.Reflection;
using System.Reflection;

namespace Legion.Enums;

public static class EnumHelper
{
	public static int ConvertEnumToInt(Enum @enum)
		=> Convert.ToInt32(@enum);

	public static List<object> GetAllEnumValues(Type enumType)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		return Enum.GetValues(enumType).OfType<object>().ToList();
	}

	public static List<string> GetAllEnumValuesAsStrings(Type enumType)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		var enumValues = GetAllEnumValues(enumType);
		var result = new List<string>();
		if (enumValues != null && 0 < enumValues.Count)
			result.AddRange(enumValues.Select(x => x.ToString()!));

		return result;
	}

	public static List<int> GetAllEnumValuesAsInts(Type enumType)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		var enumValues = GetAllEnumValues(enumType).Cast<Enum>().ToArray();
		return enumValues.Cast<int>().ToList();
	}

	public static object ConvertIntToEnum(Type enumType, int enumIntValue)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		if (Enum.IsDefined(enumType, enumIntValue))
			return Enum.ToObject(enumType, enumIntValue);
		else
			throw new System.ArgumentException($"Requested int value {enumIntValue} was not found in enum type {enumType.ToFriendlyFullName()}");
	}

	public static object ConvertIntToEnumWithDefault(Type enumType, int enumIntValue, Enum defaultValue)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		if (Enum.IsDefined(enumType, enumIntValue))
			return Enum.ToObject(enumType, enumIntValue);
		else
			return defaultValue;
	}

	public static object ConvertStringToEnum(Type enumType, string enumStringValue)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		var result = Enum.Parse(enumType, enumStringValue);
		return result;
	}

	public static object ConvertStringToEnum(Type enumType, string enumStringValue, bool ignoreCase)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		var result = Enum.Parse(enumType, enumStringValue, ignoreCase);
		return result;
	}

	public static object ConvertStringToEnumWithDefault(Type enumType, string enumStringValue, Enum defaultValue)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		try
		{
			var result = Enum.Parse(enumType, enumStringValue);
			return result;
		}
		catch
		{
			return defaultValue;
		}
	}

	public static object ConvertStringToEnumWithDefault(Type enumType, string enumStringValue, Enum defaultValue, bool ignoreCase)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		try
		{
			var result = Enum.Parse(enumType, enumStringValue, ignoreCase);
			return result;
		}
		catch
		{
			return defaultValue;
		}
	}

	public static int ConvertStringToInt(Type enumType, string enumStringValue)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		var e = (Enum)ConvertStringToEnum(enumType, enumStringValue);
		return Convert.ToInt32(e);
	}

	public static int ConvertStringToInt(Type enumType, string enumStringValue, bool ignoreCase)
	{
		if (enumType == null)
			throw new ArgumentNullException(nameof(enumType));

		var e = (Enum)ConvertStringToEnum(enumType, enumStringValue, ignoreCase);
		return Convert.ToInt32(e);
	}

	public static List<TEnum> GetAllEnumValues<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		=> Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

	public static List<string> GetAllEnumValuesAsStrings<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		var enumValues = GetAllEnumValues<TEnum>();
		var result = new List<string>();
		if (enumValues != null && 0 < enumValues.Count)
			result.AddRange(enumValues.Select(x => x.ToString()!));

		return result;
	}

	public static List<int> GetAllEnumValuesAsInts<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		var enumValues = GetAllEnumValues<TEnum>().Cast<Enum>().ToArray();
		return enumValues.Cast<int>().ToList();
	}

	public static TEnum ConvertIntToEnum<TEnum>(int enumIntValue) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		var enumType = typeof(TEnum);

		if (Enum.IsDefined(enumType, enumIntValue))
			return (TEnum)Enum.ToObject(enumType, enumIntValue);
		else
			throw new System.ArgumentException($"Requested int value {enumIntValue} was not found in enum type {enumType.ToFriendlyFullName()}");
	}

	public static bool TryConvertIntToEnum<TEnum>(int enumIntValue, out TEnum result) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		var enumType = typeof(TEnum);

		if (Enum.IsDefined(enumType, enumIntValue))
		{
			result = (TEnum)Enum.ToObject(enumType, enumIntValue);
			return true;
		}
		else
		{
			result = default;
			return false;
		}
	}

	public static TEnum ConvertIntToEnumWithDefault<TEnum>(int enumIntValue, TEnum defaultValue) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		var enumType = typeof(TEnum);

		if (Enum.IsDefined(enumType, enumIntValue))
			return (TEnum)Enum.ToObject(enumType, enumIntValue);
		else
			return defaultValue;
	}

	public static TEnum ConvertStringToEnum<TEnum>(string enumStringValue) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		if (Enum.TryParse(enumStringValue, out TEnum result))
			return result;
		else
			throw new System.ArgumentException($"Requested string enum value {enumStringValue} was not found in enum type {typeof(TEnum).ToFriendlyFullName()}");
	}

	public static bool TryConvertStringToEnum<TEnum>(string enumStringValue, out TEnum result) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		if (Enum.TryParse(enumStringValue, out result))
			return true;
		else
			return false;
	}

	public static TEnum ConvertStringToEnum<TEnum>(string enumStringValue, bool ignoreCase) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		if (Enum.TryParse(enumStringValue, ignoreCase, out TEnum result))
			return result;
		else
			throw new System.ArgumentException($"Requested string enum value {enumStringValue} was not found in enum type {typeof(TEnum).ToFriendlyFullName()}");
	}

	public static bool TryConvertStringToEnum<TEnum>(string enumStringValue, bool ignoreCase, out TEnum result) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		if (Enum.TryParse(enumStringValue, ignoreCase, out result))
			return true;
		else
			return false;
	}

	public static TEnum ConvertStringToEnumWithDefault<TEnum>(string enumStringValue, TEnum defaultValue) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		return Enum.TryParse<TEnum>(enumStringValue, out TEnum result) ? result : defaultValue;
	}

	public static TEnum ConvertStringToEnumWithDefault<TEnum>(string enumStringValue, TEnum defaultValue, bool ignoreCase) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		return Enum.TryParse<TEnum>(enumStringValue, ignoreCase, out TEnum result) ? result : defaultValue;
	}

	public static int ConvertStringToInt<TEnum>(string enumStringValue) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		var e = ConvertStringToEnum<TEnum>(enumStringValue);
		return Convert.ToInt32(e);
	}

	public static bool TryConvertStringToInt<TEnum>(string enumStringValue, out int result) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		if (TryConvertStringToEnum<TEnum>(enumStringValue, out TEnum e))
		{
			result = Convert.ToInt32(e);
			return true;
		}

		result = 0;
		return false;
	}

	public static int ConvertStringToInt<TEnum>(string enumStringValue, bool ignoreCase) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		var e = ConvertStringToEnum<TEnum>(enumStringValue, ignoreCase);
		return Convert.ToInt32(e);
	}

	public static bool TryConvertStringToInt<TEnum>(string enumStringValue, bool ignoreCase, out int result) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		if (TryConvertStringToEnum<TEnum>(enumStringValue, ignoreCase, out TEnum e))
		{
			result = Convert.ToInt32(e);
			return true;
		}

		result = 0;
		return false;
	}

	public static TEnum? ConvertAttributeToEnum<TEnum, TAttribute>(Func<TAttribute, bool> predicate)
		where TEnum : struct, IComparable, IFormattable, IConvertible
		where TAttribute : Attribute
	{
		var enumType = typeof(TEnum);
		var tw = TypeWrapper<TEnum>.Create(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

		var found = false;
		var result = Enum.GetValues(enumType)
			.Cast<TEnum>()
			.Where(x =>
			{
				var matched = (tw.ObjectInfo.Fields.FirstOrDefault(p => p.Name == x.ToString()!)?.GetCustomAttributes(typeof(TAttribute), false))?.FirstOrDefault() is TAttribute attribute && predicate(attribute);
				if (matched)
					found = true;
				return matched;
			})
			.FirstOrDefault();

		return found
			? result
			: null;
	}

	public static bool TryConvertAttributeToEnum<TEnum, TAttribute>(Func<TAttribute, bool> predicate, out TEnum result)
		where TEnum : struct, IComparable, IFormattable, IConvertible
		where TAttribute : Attribute
	{
		var enumType = typeof(TEnum);
		var tw = TypeWrapper<TEnum>.Create(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

		var found = false;
		result = Enum.GetValues(enumType)
			.Cast<TEnum>()
			.Where(x =>
			{
				var matched = (tw.ObjectInfo.Fields.FirstOrDefault(p => p.Name == x.ToString()!)?.GetCustomAttributes(typeof(TAttribute), false))?.FirstOrDefault() is TAttribute attribute && predicate(attribute);
				if (matched)
					found = true;
				return matched;
			})
			.FirstOrDefault();

		return found;
	}

	public static TEnum ConvertAttributeToEnumWithDefault<TEnum, TAttribute>(Func<TAttribute, bool> predicate, TEnum defaultValue)
		where TEnum : struct, IComparable, IFormattable, IConvertible
		where TAttribute : Attribute
	{
		var enumType = typeof(TEnum);
		var tw = TypeWrapper<TEnum>.Create(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

		var found = false;
		var result = Enum.GetValues(enumType)
			.Cast<TEnum>()
			.Where(x =>
			{
				var matched = (tw.ObjectInfo.Fields.FirstOrDefault(p => p.Name == x.ToString()!)?.GetCustomAttributes(typeof(TAttribute), false))?.FirstOrDefault() is TAttribute attribute && predicate(attribute);
				if (matched)
					found = true;
				return matched;
			})
			.FirstOrDefault();

		return found
			? result
			: defaultValue;
	}

	public static Dictionary<TEnum, int> ToDictinoaryEnumInt<TEnum, TAttribute>()
		where TEnum : struct, IComparable, IFormattable, IConvertible
		where TAttribute : Attribute
	{
		var enumType = typeof(TEnum);
		var tw = TypeWrapper<TEnum>.Create(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

		return Enum.GetValues(enumType)
			.Cast<TEnum>()
			.ToDictionary(en => en, en => Convert.ToInt32(en));
	}

	public static Dictionary<int, TEnum> ToDictinoaryIntEnum<TEnum, TAttribute>()
		where TEnum : struct, IComparable, IFormattable, IConvertible
		where TAttribute : Attribute
	{
		var enumType = typeof(TEnum);
		var tw = TypeWrapper<TEnum>.Create(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

		return Enum.GetValues(enumType)
			.Cast<TEnum>()
			.ToDictionary(en => Convert.ToInt32(en), en => en);
	}

	public static Dictionary<TEnum, string> ToDictinoaryEnumString<TEnum, TAttribute>()
		where TEnum : struct, IComparable, IFormattable, IConvertible
		where TAttribute : Attribute
	{
		var enumType = typeof(TEnum);
		var tw = TypeWrapper<TEnum>.Create(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

		return Enum.GetValues(enumType)
			.Cast<TEnum>()
			.ToDictionary(en => en, en => en.ToString()!);
	}

	public static Dictionary<string, TEnum> ToDictinoaryStringEnum<TEnum, TAttribute>()
		where TEnum : struct, IComparable, IFormattable, IConvertible
		where TAttribute : Attribute
	{
		var enumType = typeof(TEnum);
		var tw = TypeWrapper<TEnum>.Create(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

		return Enum.GetValues(enumType)
			.Cast<TEnum>()
			.ToDictionary(en => en.ToString()!, en => en);
	}

	public static Dictionary<TEnum, TAttribute> ToDictinoaryEnumAttribute<TEnum, TAttribute>(Func<TAttribute, bool> predicate)
		where TEnum : struct, IComparable, IFormattable, IConvertible
		where TAttribute : Attribute
	{
		var enumType = typeof(TEnum);
		var tw = TypeWrapper<TEnum>.Create(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

		return Enum.GetValues(enumType)
			.Cast<TEnum>()
			.Select(x => (x, (tw.ObjectInfo.Fields.FirstOrDefault(p => p.Name == x.ToString()!)?.GetCustomAttributes(typeof(TAttribute), false))?.FirstOrDefault() as TAttribute))
			.Where(en => en.Item2 != null && predicate(en.Item2))
			.ToDictionary(en => en.x, en => en.Item2!);
	}

	public static Dictionary<TAttribute, TEnum> ToDictinoaryAttributeEnum<TEnum, TAttribute>(Func<TAttribute, bool> predicate)
		where TEnum : struct, IComparable, IFormattable, IConvertible
		where TAttribute : Attribute
	{
		var enumType = typeof(TEnum);
		var tw = TypeWrapper<TEnum>.Create(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

		return Enum.GetValues(enumType)
			.Cast<TEnum>()
			.Select(x => (x, (tw.ObjectInfo.Fields.FirstOrDefault(p => p.Name == x.ToString()!)?.GetCustomAttributes(typeof(TAttribute), false))?.FirstOrDefault() as TAttribute))
			.Where(en => en.Item2 != null && predicate(en.Item2))
			.ToDictionary(en => en.Item2!, en => en.x);
	}
}
