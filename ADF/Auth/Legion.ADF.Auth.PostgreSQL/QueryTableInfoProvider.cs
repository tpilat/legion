using Legion.Extensions;

namespace Legion.ADF.Auth.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.Auth.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwUserTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "\"VwUser\"",
				[
					new(nameof(Legion.ADF.Auth.Model.VwUser.IdUser), typeof(Guid), "\"IdUser\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.VwUser.Login), typeof(string), "\"Login\"", "varchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.NormalizedLogin), typeof(string), "\"NormalizedLogin\"", "varchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.TenantIdentifier), typeof(Guid?), "\"TenantIdentifier\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.Email), typeof(string), "\"Email\"", "varchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.NormalizedEmail), typeof(string), "\"NormalizedEmail\"", "varchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.EmailConfirmed), typeof(bool?), "\"EmailConfirmed\"", "boolean", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.PasswordHash), typeof(string), "\"PasswordHash\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.SecurityStamp), typeof(string), "\"SecurityStamp\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.ADDistinguishedName), typeof(string), "\"ADDistinguishedName\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.Data), typeof(string), "\"Data\"", "jsonb", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.PhoneNumber), typeof(string), "\"PhoneNumber\"", "varchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.PhoneNumberConfirmed), typeof(bool?), "\"PhoneNumberConfirmed\"", "boolean", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.MultiFactorEnabled), typeof(bool?), "\"MultiFactorEnabled\"", "boolean", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.LockoutEndUtc), typeof(DateTime?), "\"LockoutEndUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.LockoutEnabled), typeof(bool?), "\"LockoutEnabled\"", "boolean", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.AccessFailedCount), typeof(int?), "\"AccessFailedCount\"", "integer", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.IsSystemUser), typeof(bool?), "\"IsSystemUser\"", "boolean", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.ConfirmationUrlSlug), typeof(string), "\"ConfirmationUrlSlug\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.ConfirmationUrlValidToUtc), typeof(DateTime?), "\"ConfirmationUrlValidToUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.AuditCreatedUtc), typeof(DateTime?), "\"AuditCreatedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.AuditModifiedUtc), typeof(DateTime?), "\"AuditModifiedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.IdAuditCreatedBy), typeof(Guid?), "\"IdAuditCreatedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.IdAuditModifiedBy), typeof(Guid?), "\"IdAuditModifiedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.ConcurrencyToken), typeof(Guid?), "\"ConcurrencyToken\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.VwUser.DeletedUtc), typeof(DateTime?), "\"DeletedUtc\"", "timestamp with time zone", true),
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
