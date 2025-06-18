using Legion.Extensions;

namespace Legion.ADF.Config.PostgreSQL;

public class TableInfoProvider : Legion.ADF.Config.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ConfigurationClassTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"conf", "\"ConfigurationClass\"",
				[
					new(nameof(Legion.ADF.Config.Model.ConfigurationClass.IdConfigurationClass), typeof(Guid), "\"IdConfigurationClass\"", "uuid", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationClass.RootPath), typeof(string), "\"RootPath\"", "text", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationClass.DisplayName), typeof(string), "\"DisplayName\"", "varchar(255)", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationClass.Class), typeof(string), "\"Class\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetConfigurationClassTableInfo()
		=> _ConfigurationClassTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ConfigurationKeyValueTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"conf", "\"ConfigurationKeyValue\"",
				[
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.IdConfigurationKeyValue), typeof(Guid), "\"IdConfigurationKeyValue\"", "uuid", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.Key), typeof(string), "\"Key\"", "text", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.Value), typeof(string), "\"Value\"", "text", true),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.AuditCreatedUtc), typeof(DateTime), "\"AuditCreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.AuditModifiedUtc), typeof(DateTime?), "\"AuditModifiedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.IdAuditCreatedBy), typeof(Guid?), "\"IdAuditCreatedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.IdAuditModifiedBy), typeof(Guid?), "\"IdAuditModifiedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Config.Model.ConfigurationKeyValue.ConcurrencyToken), typeof(Guid), "\"ConcurrencyToken\"", "uuid", false),
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
