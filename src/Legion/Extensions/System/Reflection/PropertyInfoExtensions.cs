using System.Reflection;

namespace Legion.Extensions;

public static class PropertyInfoExtensions
{
	private static readonly Lazy<Type> _stringType = new(() => typeof(string));
	private static readonly Lazy<Type> _ienumerableType = new(() => typeof(System.Collections.IEnumerable));
	public const string IsExternalInit = $"{nameof(System)}.{nameof(System.Runtime)}.{nameof(System.Runtime.CompilerServices)}.{nameof(System.Runtime.CompilerServices.IsExternalInit)}";

	public static T? GetFirstAttribute<T>(this PropertyInfo pi, bool inherit = true) where T : Attribute
	{
		if (pi == null)
			return default;

		var result = pi.GetCustomAttributes(typeof(T), inherit);
		return result != null ? result.FirstOrDefault() as T : null;
	}

	public static T[]? GetAttributeList<T>(this PropertyInfo pi, bool inherit = true) where T : Attribute
	{
		if (pi == null)
			return default;

		var result = pi.GetCustomAttributes(typeof(T), inherit);
		return result != null ? result as T[] : null;
	}

	public static bool IsArray(this PropertyInfo pi)
	{
		if (pi == null)
			return false;

		return pi.PropertyType.IsArray;
	}

	public static bool IsEnumerable(this PropertyInfo pi)
	{
		if (pi == null)
			return false;

		return pi.PropertyType.IsEnumerable();
	}

	public static bool IsDictionary(this PropertyInfo pi)
	{
		if (pi == null)
			return false;

		return pi.PropertyType.IsDictionary();
	}

	public static bool HasPublicSetterWithoutInit(this PropertyInfo property)
	{
		var setter = property.SetMethod;

		if (setter == null || !setter.IsPublic)
			return false;

		// Check if the setter is marked as an 'init' accessor
		return setter.ReturnParameter?.GetRequiredCustomModifiers()?
			.Any(modifier => modifier.FullName == IsExternalInit) != true;
	}
}
