using System.Reflection;

namespace Legion.Extensions;

public static class FieldInfoExtensions
{
	private static readonly Lazy<Type> _stringType = new(() => typeof(string));
	private static readonly Lazy<Type> _ienumerableType = new(() => typeof(System.Collections.IEnumerable));

	public static T? GetFirstAttribute<T>(this FieldInfo fi, bool inherit = true) where T : Attribute
	{
		if (fi == null) return default;
		var result = fi.GetCustomAttributes(typeof(T), inherit);
		return result != null ? result.FirstOrDefault() as T : null;
	}

	public static T[]? GetAttributeList<T>(this FieldInfo fi, bool inherit = true) where T : Attribute
	{
		if (fi == null) return default;
		var result = fi.GetCustomAttributes(typeof(T), inherit);
		return result != null ? result as T[] : null;
	}

	public static bool IsConst(this FieldInfo fi)
	{
		if (fi == null) return false;
		return fi.IsLiteral && !fi.IsInitOnly;
	}

	public static bool IsArray(this FieldInfo fi)
	{
		if (fi == null)
			return false;

		return fi.FieldType.IsArray;
	}

	public static bool IsEnumerable(this FieldInfo fi)
	{
		if (fi == null)
			return false;

		return fi.FieldType.IsEnumerable();
	}

	public static bool IsDictionary(this FieldInfo fi)
	{
		if (fi == null)
			return false;

		return fi.FieldType.IsDictionary();
	}

	public static bool IsBackingField(this FieldInfo fi)
		=> fi.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false);
}
