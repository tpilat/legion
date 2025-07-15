using Legion.Extensions;

namespace Legion.ADF.Cache.SqlServer;

public class TableInfoProvider : Legion.ADF.Cache.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _CacheDataTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"cache", "[CacheData]",
				[
					new(nameof(Legion.ADF.Cache.Model.CacheData.KeyHash), typeof(string), "[KeyHash]", "nvarchar(32)", false),
					new(nameof(Legion.ADF.Cache.Model.CacheData.ValueHash), typeof(string), "[ValueHash]", "nvarchar(32)", false),
					new(nameof(Legion.ADF.Cache.Model.CacheData.Key), typeof(string), "[Key]", "nvarchar(max)", false),
					new(nameof(Legion.ADF.Cache.Model.CacheData.Value), typeof(string), "[Value]", "nvarchar(max)", false),
					new(nameof(Legion.ADF.Cache.Model.CacheData.KeyPrefix450), typeof(string), "[KeyPrefix450]", "nvarchar(450)", false),
					new(nameof(Legion.ADF.Cache.Model.CacheData.ExpiresUtc), typeof(DateTime?), "[ExpiresUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Cache.Model.CacheData.SlidingTime), typeof(TimeSpan?), "[SlidingTime]", "time", true),
					new(nameof(Legion.ADF.Cache.Model.CacheData.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Cache.Model.CacheData.LastAccessedUtc), typeof(DateTime), "[LastAccessedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Cache.Model.CacheData.RowVersion), typeof(Guid), "[RowVersion]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetCacheDataTableInfo()
		=> _CacheDataTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _DistributedLockTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"cache", "[DistributedLock]",
				[
					new(nameof(Legion.ADF.Cache.Model.DistributedLock.KeyHash), typeof(string), "[KeyHash]", "nvarchar(32)", false),
					new(nameof(Legion.ADF.Cache.Model.DistributedLock.LockKey), typeof(string), "[LockKey]", "nvarchar(max)", false),
					new(nameof(Legion.ADF.Cache.Model.DistributedLock.LockId), typeof(string), "[LockId]", "nvarchar(32)", false),
					new(nameof(Legion.ADF.Cache.Model.DistributedLock.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Cache.Model.DistributedLock.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Cache.Model.DistributedLock.ExpiresUtc), typeof(DateTime), "[ExpiresUtc]", "datetime2", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetDistributedLockTableInfo()
		=> _DistributedLockTableInfo.Value;

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
			{ typeof(Legion.ADF.Cache.Model.CacheData), GetCacheDataTableInfo() },
			{ typeof(Legion.ADF.Cache.Model.DistributedLock), GetDistributedLockTableInfo() },
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
