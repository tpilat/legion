using Legion.Extensions;

namespace Legion.ADF.Config.SqlServer;

public class TableInfoProvider : Legion.ADF.Config.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ConfigurationClassTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"conf", "[ConfigurationClass]",
				[
					new(nameof(Legion.ADF.Config.Model.ConfigurationClass.IdConfigurationClass), typeof(Guid), "[IdConfigurationClass]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationClass.RootPath), typeof(string), "[RootPath]", "nvarchar(4000)", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationClass.DisplayName), typeof(string), "[DisplayName]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationClass.Class), typeof(string), "[Class]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetConfigurationClassTableInfo()
		=> _ConfigurationClassTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ConfigurationKeyValueTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"conf", "[ConfigurationKeyValue]",
				[
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.IdConfigurationKeyValue), typeof(Guid), "[IdConfigurationKeyValue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.Key), typeof(string), "[Key]", "nvarchar(4000)", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.Value), typeof(string), "[Value]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.AuditCreatedUtc), typeof(DateTime), "[AuditCreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.AuditModifiedUtc), typeof(DateTime?), "[AuditModifiedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.IdAuditCreatedBy), typeof(Guid?), "[IdAuditCreatedBy]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.IdAuditModifiedBy), typeof(Guid?), "[IdAuditModifiedBy]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.ConcurrencyToken), typeof(Guid), "[ConcurrencyToken]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetConfigurationKeyValueTableInfo()
		=> _ConfigurationKeyValueTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Config.Model.ConfigurationClass), GetConfigurationClassTableInfo() },
			{ typeof(Legion.ADF.Config.Model.ConfigurationKeyValue), GetConfigurationKeyValueTableInfo() },
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
