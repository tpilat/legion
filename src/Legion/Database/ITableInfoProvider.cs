namespace Legion.Database;

public interface ITableInfoProvider
{
	IReadOnlyDictionary<Type, Legion.Database.Metamodel.Info.TableInfo> TableInfoDictionary { get; }

	Legion.Database.Metamodel.Info.TableInfo GetTableInfo<T>();
	
	Legion.Database.Metamodel.Info.TableInfo GetTableInfo(Type type);
}
