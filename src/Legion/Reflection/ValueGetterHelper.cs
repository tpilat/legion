using Legion.Extensions;
using Legion.Validation;

namespace Legion;

public static class ValueGetterHelper
{
	public static T GetNewValueIfSet<T>(T oldValue, T? newValue)
	{
		if (newValue == null)
			return oldValue;

		return newValue is bool
			? newValue
			: (ValidationHelper.IsDefaultOrEmpty(newValue, true)
				? oldValue
				: newValue);
	}

	public static string GetNewValueIfNotWhitespace(string oldValue, string newValue)
		=> (!string.IsNullOrWhiteSpace(newValue))
			? newValue
			: oldValue;

	public static T GetNewValueIfNotDefault<T>(T oldValue, T newValue)
		=> Equals(newValue, typeof(T).GetDefaultNullableValue())
			? oldValue
			: newValue;

	public static T GetNewValueIfHasValue<T>(T oldValue, Nullable<T> newValue)
		where T : struct
		=> newValue ?? oldValue;

	public static T GetNewValueIfHasValue<T>(T oldValue, T? newValue)
		=> newValue ?? oldValue;

	public static T GetNewValueIfNotNull<T>(T oldValue, T newValue)
		=> newValue == null
			? oldValue
			: newValue;

	public static T GetNewValueIfNotDefaultOrNotEmpty<T>(T oldValue, T newValue)
		=> ValidationHelper.IsDefaultOrEmpty(newValue, false)
			? oldValue
			: newValue;
}
