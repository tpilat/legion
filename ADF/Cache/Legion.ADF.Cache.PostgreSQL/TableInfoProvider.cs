using Legion.Extensions;

namespace Legion.ADF.Cache.PostgreSQL;

public class TableInfoProvider : Legion.ADF.Cache.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ReloadableCacheKeyTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"cache", "\"ReloadableCacheKey\"",
				[
					new(nameof(Legion.ADF.Cache.Model.ReloadableCacheKey.IdReloadableCacheKey), typeof(Guid), "\"IdReloadableCacheKey\"", "uuid", false),
					new(nameof(Legion.ADF.Cache.Model.ReloadableCacheKey.Key), typeof(string), "\"Key\"", "text", true),
					new(nameof(Legion.ADF.Cache.Model.ReloadableCacheKey.Tags), typeof(List<string>), "\"Tags\"", "text[]", true),
					new(nameof(Legion.ADF.Cache.Model.ReloadableCacheKey.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Cache.Model.ReloadableCacheKey.ReloadAtUtc), typeof(DateTime), "\"ReloadAtUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetReloadableCacheKeyTableInfo()
		=> _ReloadableCacheKeyTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Cache.Model.ReloadableCacheKey), GetReloadableCacheKeyTableInfo() },
		});

	public IReadOnlyDictionary<Type, Legion.Database.Metamodel.Info.TableInfo> TableInfoDictionary => _tableInfoDictionary.Value;

	public Legion.Database.Metamodel.Info.TableInfo GetTableInfo<T>()
		=> GetTableInfo(typeof(T));

	public Legion.Database.Metamodel.Info.TableInfo GetTableInfo(Type type)
	{
		if (TableInfoDictionary.TryGetValue(type, out var tableInfo))
			return tableInfo;

		Legion.Throw.InvalidOperationException($"Invalid entity type = {type.ToFriendlyFullName()}");
		return null;
	}
}
