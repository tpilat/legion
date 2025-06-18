using Legion.Extensions;

namespace Legion.ADF.Config.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.Config.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwConfigurationClassTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"conf", "[VwConfigurationClass]",
				[
					new(nameof(Legion.ADF.Config.Model.VwConfigurationClass.IdConfigurationClass), typeof(Guid), "[IdConfigurationClass]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Config.Model.VwConfigurationClass.RootPath), typeof(string), "[RootPath]", "nvarchar(4000)", false),
					new(nameof(Legion.ADF.Config.Model.VwConfigurationClass.DisplayName), typeof(string), "[DisplayName]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.Config.Model.VwConfigurationClass.Class), typeof(string), "[Class]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwConfigurationClassTableInfo()
		=> _VwConfigurationClassTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Config.Model.VwConfigurationClass), GetVwConfigurationClassTableInfo() },
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
