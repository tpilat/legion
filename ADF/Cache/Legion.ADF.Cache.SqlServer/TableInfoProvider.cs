using Legion.Extensions;

namespace Legion.ADF.Cache.SqlServer;

public class TableInfoProvider : Legion.ADF.Cache.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ReloadableCacheKeyTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"cache", "[ReloadableCacheKey]",
				[
					new(nameof(Legion.ADF.Cache.Model.ReloadableCacheKey.IdReloadableCacheKey), typeof(Guid), "[IdReloadableCacheKey]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Cache.Model.ReloadableCacheKey.Key), typeof(string), "[Key]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Cache.Model.ReloadableCacheKey.Tags), typeof(List<string>), "[Tags]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Cache.Model.ReloadableCacheKey.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Cache.Model.ReloadableCacheKey.ReloadAtUtc), typeof(DateTime), "[ReloadAtUtc]", "datetime2", false),
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
