using Legion.Extensions;

namespace Legion.ADF.Auth.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.Auth.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwUserTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "[VwUser]",
				[
					new(nameof(Legion.ADF.Auth.Model.VwUser.IdUser), typeof(Guid), "[IdUser]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Auth.Model.VwUser.Login), typeof(string), "[Login]", "nvarchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.NormalizedLogin), typeof(string), "[NormalizedLogin]", "nvarchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.TenantIdentifier), typeof(Guid?), "[TenantIdentifier]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.Email), typeof(string), "[Email]", "nvarchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.NormalizedEmail), typeof(string), "[NormalizedEmail]", "nvarchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.EmailConfirmed), typeof(bool), "[EmailConfirmed]", "bit", false),
					new(nameof(Legion.ADF.Auth.Model.VwUser.PasswordHash), typeof(string), "[PasswordHash]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.SecurityStamp), typeof(string), "[SecurityStamp]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.ADDistinguishedName), typeof(string), "[ADDistinguishedName]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.Data), typeof(string), "[Data]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.PhoneNumber), typeof(string), "[PhoneNumber]", "nvarchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.PhoneNumberConfirmed), typeof(bool), "[PhoneNumberConfirmed]", "bit", false),
					new(nameof(Legion.ADF.Auth.Model.VwUser.MultiFactorEnabled), typeof(bool), "[MultiFactorEnabled]", "bit", false),
					new(nameof(Legion.ADF.Auth.Model.VwUser.LockoutEndUtc), typeof(DateTime?), "[LockoutEndUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.LockoutEnabled), typeof(bool), "[LockoutEnabled]", "bit", false),
					new(nameof(Legion.ADF.Auth.Model.VwUser.AccessFailedCount), typeof(int), "[AccessFailedCount]", "int", false),
					new(nameof(Legion.ADF.Auth.Model.VwUser.IsSystemUser), typeof(bool), "[IsSystemUser]", "bit", false),
					new(nameof(Legion.ADF.Auth.Model.VwUser.ConfirmationUrlSlug), typeof(string), "[ConfirmationUrlSlug]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.ConfirmationUrlValidToUtc), typeof(DateTime?), "[ConfirmationUrlValidToUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.AuditCreatedUtc), typeof(DateTime), "[AuditCreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Auth.Model.VwUser.AuditModifiedUtc), typeof(DateTime?), "[AuditModifiedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.IdAuditCreatedBy), typeof(Guid?), "[IdAuditCreatedBy]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.IdAuditModifiedBy), typeof(Guid?), "[IdAuditModifiedBy]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.ConcurrencyToken), typeof(Guid), "[ConcurrencyToken]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Auth.Model.VwUser.DeletedUtc), typeof(DateTime), "[DeletedUtc]", "datetime2", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwUserTableInfo()
		=> _VwUserTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Auth.Model.VwUser), GetVwUserTableInfo() },
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
