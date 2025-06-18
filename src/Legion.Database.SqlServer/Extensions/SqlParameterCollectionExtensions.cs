using Microsoft.Data.SqlClient;

namespace Legion.Database.SqlServer.Extensions;

public static class SqlParameterCollectionExtensions
{
	public static SqlParameter AddWithNullableValue(this SqlParameterCollection sqlParameterCollection, string parameterName, System.Data.SqlDbType parameterType, object? value)
	{
		return sqlParameterCollection.Add(new SqlParameter(parameterName, parameterType)
		{
			Value = value ?? DBNull.Value
		});
	}

	public static SqlParameter AddWithValue(this SqlParameterCollection sqlParameterCollection, string parameterName, System.Data.SqlDbType parameterType, object value)
	{
		return sqlParameterCollection.Add(new SqlParameter(parameterName, parameterType)
		{
			Value = value
		});
	}
}
