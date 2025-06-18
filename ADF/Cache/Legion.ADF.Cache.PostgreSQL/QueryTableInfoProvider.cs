using Legion.Extensions;

namespace Legion.ADF.Cache.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.Cache.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwReloadableCacheKeyTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"cache", "\"VwReloadableCacheKey\"",
				[
					new(nameof(Legion.ADF.Cache.Model.VwReloadableCacheKey.IdReloadableCacheKey), typeof(Guid), "\"IdReloadableCacheKey\"", "uuid", false),
					new(nameof(Legion.ADF.Cache.Model.VwReloadableCacheKey.Key), typeof(string), "\"Key\"", "text", true),
					new(nameof(Legion.ADF.Cache.Model.VwReloadableCacheKey.Tags), typeof(List<string>), "\"Tags\"", "text[]", true),
					new(nameof(Legion.ADF.Cache.Model.VwReloadableCacheKey.CreatedUtc), typeof(DateTime?), "\"CreatedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Cache.Model.VwReloadableCacheKey.ReloadAtUtc), typeof(DateTime?), "\"ReloadAtUtc\"", "timestamp with time zone", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwReloadableCacheKeyTableInfo()
		=> _VwReloadableCacheKeyTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Cache.Model.VwReloadableCacheKey), GetVwReloadableCacheKeyTableInfo() },
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
