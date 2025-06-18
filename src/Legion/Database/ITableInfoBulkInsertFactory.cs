using Legion.Database.Metamodel.Info;
using System.Data.Common;

namespace Legion.Database;

public interface ITableInfoBulkInsertFactory
{
	ITableInfoBulkInsert<T> Create<T>(
		ITableInfoProvider tableInfoProvider,
		Func<T, Dictionary<string, object?>> dictionaryMapper,
		string connectionString);

	ITableInfoBulkInsert<T> Create<T>(
		ITableInfoProvider tableInfoProvider,
		Func<T, Dictionary<string, object?>> dictionaryMapper,
		DbConnection dbConnection);

	ITableInfoBulkInsert<T> Create<T>(
		TableInfo tableInfo,
		Func<T, Dictionary<string, object?>> dictionaryMapper,
		string connectionString);

	ITableInfoBulkInsert<T> Create<T>(
		TableInfo tableInfo,
		Func<T, Dictionary<string, object?>> dictionaryMapper,
		DbConnection dbConnection);
}
