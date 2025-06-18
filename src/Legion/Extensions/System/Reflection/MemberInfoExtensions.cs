using Legion.Reflection;
using System.Reflection;

namespace Legion.Extensions;

public static class MemberInfoExtensions
{
	public static T? GetFirstAttribute<T>(this MemberInfo mi, bool inherit = true)
		where T : Attribute
	{
		if (mi == null) return default;
		var result = mi.GetCustomAttributes(typeof(T), inherit);
		return result != null ? result.FirstOrDefault() as T : null;
	}

	public static T[]? GetAttributeList<T>(this MemberInfo mi, bool inherit = true)
		where T : Attribute
	{
		if (mi == null)
			return default;

		var result = mi.GetCustomAttributes(typeof(T), inherit);
		return result != null ? result as T[] : null;
	}

	public static MemberInfo? GetFirstMemberInfoWithAttribute<T>(this IEnumerable<MemberInfo> infoList, Func<T, bool> attributeMatch, bool inherit = true)
		where T : Attribute
		=> ReflectionHelper.GetFirstMemberInfoWithAttribute(infoList, attributeMatch, inherit);

	public static List<MemberInfo> GetAllMemberInfosWithAttribute<T>(this IEnumerable<MemberInfo> infoList, Func<T, bool> attributeMatch, bool inherit = true)
		where T : Attribute
		=> ReflectionHelper.GetAllMemberInfosWithAttribute<T>(infoList, attributeMatch, inherit);

	/// <summary>
	/// Determines whether the given <paramref name="member"/> is a static member.
	/// </summary>
	/// <returns>True for static fields, properties and methods and false for instance fields,
	/// properties and methods. Throws an exception for all other <see href="MemberTypes" />.</returns>
	public static bool IsStatic(this MemberInfo member)
	{
		var field = member as FieldInfo;
		if (field != null)
			return field.IsStatic;
		var property = member as PropertyInfo;
		if (property != null)
			return property.CanRead
				? (property.GetGetMethod(true)?.IsStatic ?? false)
				: (property.GetSetMethod(true)?.IsStatic ?? false);
		var method = member as MethodInfo;
		if (method != null)
			return method.IsStatic;
		string message = string.Format("Unable to determine IsStatic for member {0}.{1}" +
			"MemberType was {2} but only fields, properties and methods are supported.",
			member.Name, member.MemberType, Environment.NewLine);
		throw new NotSupportedException(message);
	}

	public static bool IsPublic(this MemberInfo member)
	{
		var field = member as FieldInfo;
		if (field != null)
			return field.IsPublic;
		var property = member as PropertyInfo;
		if (property != null)
			return property.CanRead
				? (property.GetGetMethod(true)?.IsPublic ?? false)
				: (property.GetSetMethod(true)?.IsPublic ?? false);
		var method = member as MethodInfo;
		if (method != null)
			return method.IsPublic;
		string message = string.Format("Unable to determine IsPublic for member {0}.{1}" +
			"MemberType was {2} but only fields, properties and methods are supported.",
			member.Name, member.MemberType, Environment.NewLine);
		throw new NotSupportedException(message);
	}

	public static bool IsPrivate(this MemberInfo member)
	{
		var field = member as FieldInfo;
		if (field != null)
			return field.IsPrivate;
		var property = member as PropertyInfo;
		if (property != null)
			return property.CanRead
				? (property.GetGetMethod(true)?.IsPrivate ?? false)
				: (property.GetSetMethod(true)?.IsPrivate ?? false);
		var method = member as MethodInfo;
		if (method != null)
			return method.IsPrivate;
		string message = string.Format("Unable to determine IsPrivate for member {0}.{1}" +
			"MemberType was {2} but only fields, properties and methods are supported.",
			member.Name, member.MemberType, Environment.NewLine);
		throw new NotSupportedException(message);
	}

	public static bool IsInternal(this MemberInfo member)
	{
		var field = member as FieldInfo;
		if (field != null)
			return field.IsAssembly;
		var property = member as PropertyInfo;
		if (property != null)
			return property.CanRead
				? (property.GetGetMethod(true)?.IsAssembly ?? false)
				: (property.GetSetMethod(true)?.IsAssembly ?? false);
		var method = member as MethodInfo;
		if (method != null)
			return method.IsAssembly;
		string message = string.Format("Unable to determine IsAssembly for member {0}.{1}" +
			"MemberType was {2} but only fields, properties and methods are supported.",
			member.Name, member.MemberType, Environment.NewLine);
		throw new NotSupportedException(message);
	}

	/// <summary>
	/// Gets the system type of the field or property identified by the <paramref name="member"/>.
	/// </summary>
	/// <returns>The system type of the member.</returns>
	public static Type GetFieldOrPropertyType(this MemberInfo member)
	{
		Throw.IfArgumentNull(member);

		if (member.MemberType == MemberTypes.Property)
		{
			var property = member as PropertyInfo;
			if (property != null)
				return property.PropertyType;
		}

		if (member.MemberType == MemberTypes.Field)
		{
			var field = member as FieldInfo;
			if (field != null)
				return field.FieldType;
		}

		throw new NotSupportedException("Can only determine the type for fields and properties.");
	}

	/// <summary>
	/// Gets the return type of an member.
	/// </summary>
	/// <param name="member">The member.</param>
	/// <returns></returns>
	/// <exception cref="System.NotSupportedException">Unable to get return type of member of type  + member.MemberType</exception>
	public static Type GetReturnType(this MemberInfo member)
		=> member switch
		{
			PropertyInfo propertyInfo => propertyInfo.PropertyType,
			MethodInfo methodInfo => methodInfo.ReturnType,
			FieldInfo fieldInfo => fieldInfo.FieldType,
			_ => throw new NotSupportedException("Unable to get return type of member of type " + member.GetType().Name),
		};
}
