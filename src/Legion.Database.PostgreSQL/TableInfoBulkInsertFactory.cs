using Legion.Database.Metamodel.Info;
using Legion.Extensions;
using Npgsql;
using System.Data.Common;

namespace Legion.Database.PostgreSQL;

public class TableInfoBulkInsertFactory : ITableInfoBulkInsertFactory
{
	public ITableInfoBulkInsert<T> Create<T>(
		ITableInfoProvider tableInfoProvider,
		Func<T, Dictionary<string, object?>> dictionaryMapper,
		string connectionString)
	{
		Throw.IfArgumentNull(tableInfoProvider);
		var tableInfo = tableInfoProvider.GetTableInfo<T>();
		return Create(tableInfo, dictionaryMapper, connectionString);
	}

	public ITableInfoBulkInsert<T> Create<T>(
		ITableInfoProvider tableInfoProvider,
		Func<T, Dictionary<string, object?>> dictionaryMapper,
		DbConnection dbConnection)
	{
		Throw.IfArgumentNull(tableInfoProvider);
		var tableInfo = tableInfoProvider.GetTableInfo<T>();
		return Create(tableInfo, dictionaryMapper, dbConnection);
	}

	public ITableInfoBulkInsert<T> Create<T>(
		TableInfo tableInfo,
		Func<T, Dictionary<string, object?>> dictionaryMapper,
		string connectionString)
		=> new TableInfoBulkInsert<T>(tableInfo, dictionaryMapper, connectionString);

	public ITableInfoBulkInsert<T> Create<T>(
		TableInfo tableInfo,
		Func<T, Dictionary<string, object?>> dictionaryMapper,
		DbConnection dbConnection)
	{
		Throw.IfArgumentNull(dbConnection);

		if (dbConnection is NpgsqlConnection npgsqlConnection)
			return new TableInfoBulkInsert<T>(tableInfo, dictionaryMapper, npgsqlConnection);

		Throw.NotSupportedException($"Invalid {nameof(dbConnection)} type = {dbConnection.GetType().ToFriendlyFullName()} | Expected = {typeof(NpgsqlConnection).ToFriendlyFullName()}");
		return null;
	}
}
