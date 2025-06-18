namespace Legion.Database;

public interface ITableInfoBulkInsert<T> : IDisposable
{
	ulong BulkInsert(IEnumerable<T> entities, bool alwaysCreateNewConnection);
}
