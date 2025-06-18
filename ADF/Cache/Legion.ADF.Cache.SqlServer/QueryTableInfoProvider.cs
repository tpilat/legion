using Legion.Extensions;

namespace Legion.ADF.Cache.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.Cache.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwReloadableCacheKeyTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"cache", "[VwReloadableCacheKey]",
				[
					new(nameof(Legion.ADF.Cache.Model.VwReloadableCacheKey.IdReloadableCacheKey), typeof(Guid), "[IdReloadableCacheKey]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Cache.Model.VwReloadableCacheKey.Key), typeof(string), "[Key]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Cache.Model.VwReloadableCacheKey.Tags), typeof(List<string>), "[Tags]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Cache.Model.VwReloadableCacheKey.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Cache.Model.VwReloadableCacheKey.ReloadAtUtc), typeof(DateTime), "[ReloadAtUtc]", "datetime2", false),
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
