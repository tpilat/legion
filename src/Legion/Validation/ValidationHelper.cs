using Legion.Exceptions.Internal;
using Legion.Extensions;
using System.Collections;

namespace Legion.Validation;

public class ValidationHelper
{
	public static bool IsDefault<T>(T? value)
		=> Equals(value, typeof(T).GetDefaultNullableValue());

	public static bool IsDefaultOrEmpty<T>(T? value, bool stringWhiteSpaceCheck)
	{
		var defaultValue = typeof(T).GetDefaultNullableValue();
		return IsDefaultOrEmpty(value, defaultValue, stringWhiteSpaceCheck);
	}

	public static bool IsDefaultOrEmpty<T>(T? value, object? defaultValue, bool stringWhiteSpaceCheck)
	{
		if (Equals(value, defaultValue))
			return true;

		switch (value)
		{
			case null:
			case string s when (stringWhiteSpaceCheck ? string.IsNullOrWhiteSpace(s) : string.IsNullOrEmpty(s)):
			case ICollection c when c.Count == 0:
			case Array a when a.Length == 0:
			case IEnumerable e when !e.Cast<object>().Any():
				return true;
			default:
				return false;
		}
	}

	public static IErrorCode? GetDefaultOrEmptyErrorCode<T>(T? value, object? defaultValue, string message, bool stringWhiteSpaceCheck)
	{
		if (Equals(value, defaultValue))
			return ErrorCodes.DefaultValueException.CustomValidation(message);

		return value switch
		{
			null => ErrorCodes.NullValueException.CustomValidation(message),
			string s when (stringWhiteSpaceCheck && string.IsNullOrWhiteSpace(s)) => ErrorCodes.EmptyValueException.WhiteSpaceValidation(message),
			string s when string.IsNullOrEmpty(s) => ErrorCodes.EmptyValueException.EmptyStringValidation(message),
			ICollection c when c.Count == 0 => ErrorCodes.EmptyValueException.EmptyCollectionValidation(message),
			Array a when a.Length == 0 => ErrorCodes.EmptyValueException.EmptyArrayValidation(message),
			IEnumerable e when !e.Cast<object>().Any() => ErrorCodes.EmptyValueException.EmptyEnumerableValidation(message),
			_ => null,
		};
	}

	public static IErrorCode? GetNotDefaultOrEmptyErrorCode<T>(T? value, object? defaultValue, string message, bool stringWhiteSpaceCheck)
	{
		if (Equals(value, defaultValue))
			return ErrorCodes.DefaultValueException.NotDefaultCustomValidation(message);

		return value switch
		{
			null => ErrorCodes.NullValueException.NotNullCustomValidation(message),
			string s when (stringWhiteSpaceCheck && string.IsNullOrWhiteSpace(s)) => ErrorCodes.EmptyValueException.NotWhiteSpaceStringValidation(message),
			string s when string.IsNullOrEmpty(s) => ErrorCodes.EmptyValueException.NotEmptyStringValidation(message),
			ICollection c when c.Count == 0 => ErrorCodes.EmptyValueException.NotEmptyCollectionValidation(message),
			Array a when a.Length == 0 => ErrorCodes.EmptyValueException.NotEmptyArrayValidation(message),
			IEnumerable e when !e.Cast<object>().Any() => ErrorCodes.EmptyValueException.NotEmptyEnumerableValidation(message),
			_ => null,
		};
	}
}
