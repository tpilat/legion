using System.Reflection;

namespace Legion.Generators.AppGen.Helpers;

public class TypeHelper
{
	private static readonly Dictionary<Type, string> _primitiveTypeNames = new()
	{
			{ typeof(bool), "bool" },
			{ typeof(byte), "byte" },
			{ typeof(byte[]), "byte[]" },
			{ typeof(sbyte), "sbyte" },
			{ typeof(short), "short" },
			{ typeof(ushort), "ushort" },
			{ typeof(int), "int" },
			{ typeof(uint), "uint" },
			{ typeof(long), "long" },
			{ typeof(ulong), "ulong" },
			{ typeof(char), "char" },
			{ typeof(float), "float" },
			{ typeof(double), "double" },
			{ typeof(string), "string" },
			{ typeof(decimal), "decimal" }
		};

	public static string TypeToCSharpSourceCode(Type type)
	{
		Throw.IfArgumentNull(type);

		if (type.IsArray)
		{
			return TypeToCSharpSourceCode(type.GetElementType()!) + "[]";
		}

		if (type.GetTypeInfo().IsGenericType)
		{
			if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return TypeToCSharpSourceCode(Nullable.GetUnderlyingType(type)!) + '?';
			}

			var genericTypeDefName = type.Name.Substring(0, type.Name.IndexOf('`'));
			var genericTypeArguments = string.Join(", ", type.GenericTypeArguments.Select(TypeToCSharpSourceCode));
			return $"{genericTypeDefName}<{genericTypeArguments}>";
		}

		return _primitiveTypeNames.TryGetValue(type, out var typeName)
			? typeName
			: type.Name;
	}
}
