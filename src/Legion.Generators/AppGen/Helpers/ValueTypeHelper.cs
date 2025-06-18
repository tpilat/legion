using Legion.Extensions;

namespace Legion.Generators.AppGen.Helpers;

public static class ValueTypeHelper
{
	public static Type GetType(Model.ValueType valueType, bool isNullable)
	{
		return valueType switch
		{
			Model.ValueType.Int => isNullable ? typeof(int?) : typeof(int),
			Model.ValueType.Long => isNullable ? typeof(long?) : typeof(long),
			Model.ValueType.Guid => isNullable ? typeof(Guid?) : typeof(Guid),
			Model.ValueType.String => typeof(string),
			Model.ValueType.Byte => typeof(byte),
			Model.ValueType.ByteArray => typeof(byte[]),
			Model.ValueType.Char => isNullable ? typeof(char?) : typeof(char),
			Model.ValueType.DateTime => isNullable ? typeof(DateTime?) : typeof(DateTime),
			Model.ValueType.TimeSpan => isNullable ? typeof(TimeSpan?) : typeof(TimeSpan),
			Model.ValueType.Bool => isNullable ? typeof(bool?) : typeof(bool),
			Model.ValueType.Decimal => isNullable ? typeof(decimal?) : typeof(decimal),
			Model.ValueType.Single => isNullable ? typeof(float?) : typeof(float),
			Model.ValueType.Double => isNullable ? typeof(double?) : typeof(double),
			Model.ValueType.Short => isNullable ? typeof(short?) : typeof(short),
			Model.ValueType.List_of_strings => typeof(List<string>),
			_ => throw new NotSupportedException(valueType.ToString()),
		};
	}

	public static Model.ValueType GetValueType(Type clrType)
	{
		var type = clrType.GetUnderlyingNullableType();
		if (type == typeof(int))
			return Model.ValueType.Int;
		else if (type == typeof(long))
			return Model.ValueType.Long;
		else if (type == typeof(Guid))
			return Model.ValueType.Guid;
		else if (type == typeof(string))
			return Model.ValueType.String;
		else if (type == typeof(char))
			return Model.ValueType.Char;
		else if (type == typeof(decimal))
			return Model.ValueType.Decimal;
		else if (type == typeof(float))
			return Model.ValueType.Single;
		else if (type == typeof(double))
			return Model.ValueType.Double;
		else if (type == typeof(bool))
			return Model.ValueType.Bool;
		else if (type == typeof(DateTime))
			return Model.ValueType.DateTime;
		else if (type == typeof(TimeSpan))
			return Model.ValueType.TimeSpan;
		else if (type == typeof(byte))
			return Model.ValueType.Byte;
		else if (type == typeof(byte[]))
			return Model.ValueType.ByteArray;
		else if (type == typeof(short))
			return Model.ValueType.Short;
		else if (type == typeof(List<string>))
			return Model.ValueType.List_of_strings;
		else
			throw new NotSupportedException(type?.FullName);
	}
}
