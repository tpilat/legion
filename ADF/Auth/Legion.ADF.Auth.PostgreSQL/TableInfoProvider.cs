using Legion.Extensions;

namespace Legion.ADF.Auth.PostgreSQL;

public class TableInfoProvider : Legion.ADF.Auth.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ExternalLoginTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "\"ExternalLogin\"",
				[
					new(nameof(Legion.ADF.Auth.Model.ExternalLogin.IdExternalLogin), typeof(Guid), "\"IdExternalLogin\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.ExternalLogin.IdLoginProvider), typeof(Guid), "\"IdLoginProvider\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.ExternalLogin.IdUser), typeof(Guid), "\"IdUser\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.ExternalLogin.ExternalUserIdentifier), typeof(string), "\"ExternalUserIdentifier\"", "text", false),
					new(nameof(Legion.ADF.Auth.Model.ExternalLogin.Data), typeof(string), "\"Data\"", "jsonb", true),
					new(nameof(Legion.ADF.Auth.Model.ExternalLogin.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Auth.Model.ExternalLogin.ValidToUtc), typeof(DateTime), "\"ValidToUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Auth.Model.ExternalLogin.LastAccessUtc), typeof(DateTime?), "\"LastAccessUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.ExternalLogin.RemoteIP), typeof(string), "\"RemoteIP\"", "varchar(64)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetExternalLoginTableInfo()
		=> _ExternalLoginTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _LoginProviderTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "\"LoginProvider\"",
				[
					new(nameof(Legion.ADF.Auth.Model.LoginProvider.IdLoginProvider), typeof(Guid), "\"IdLoginProvider\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.LoginProvider.Code), typeof(string), "\"Code\"", "varchar(128)", false),
					new(nameof(Legion.ADF.Auth.Model.LoginProvider.Name), typeof(string), "\"Name\"", "varchar(128)", false),
					new(nameof(Legion.ADF.Auth.Model.LoginProvider.DisabledUtc), typeof(DateTime?), "\"DisabledUtc\"", "timestamp with time zone", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetLoginProviderTableInfo()
		=> _LoginProviderTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _PermissionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "\"Permission\"",
				[
					new(nameof(Legion.ADF.Auth.Model.Permission.IdPermission), typeof(Guid), "\"IdPermission\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.Permission.Code), typeof(string), "\"Code\"", "varchar(256)", false),
					new(nameof(Legion.ADF.Auth.Model.Permission.Name), typeof(string), "\"Name\"", "varchar(1024)", false),
					new(nameof(Legion.ADF.Auth.Model.Permission.Description), typeof(string), "\"Description\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.Permission.ClaimValue), typeof(string), "\"ClaimValue\"", "varchar(1024)", true),
					new(nameof(Legion.ADF.Auth.Model.Permission.IsSystemPermission), typeof(bool), "\"IsSystemPermission\"", "boolean", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetPermissionTableInfo()
		=> _PermissionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _RoleTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "\"Role\"",
				[
					new(nameof(Legion.ADF.Auth.Model.Role.IdRole), typeof(Guid), "\"IdRole\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.Role.Name), typeof(string), "\"Name\"", "varchar(256)", false),
					new(nameof(Legion.ADF.Auth.Model.Role.NormalizedName), typeof(string), "\"NormalizedName\"", "varchar(256)", false),
					new(nameof(Legion.ADF.Auth.Model.Role.ADGroupDistinguishedName), typeof(string), "\"ADGroupDistinguishedName\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.Role.Data), typeof(string), "\"Data\"", "jsonb", true),
					new(nameof(Legion.ADF.Auth.Model.Role.Description), typeof(string), "\"Description\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.Role.HasConstantPermissions), typeof(bool), "\"HasConstantPermissions\"", "boolean", false),
					new(nameof(Legion.ADF.Auth.Model.Role.HasConstantUsers), typeof(bool), "\"HasConstantUsers\"", "boolean", false),
					new(nameof(Legion.ADF.Auth.Model.Role.IsSystemRole), typeof(bool), "\"IsSystemRole\"", "boolean", false),
					new(nameof(Legion.ADF.Auth.Model.Role.AuditCreatedUtc), typeof(DateTime), "\"AuditCreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Auth.Model.Role.AuditModifiedUtc), typeof(DateTime?), "\"AuditModifiedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.Role.IdAuditCreatedBy), typeof(Guid?), "\"IdAuditCreatedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.Role.IdAuditModifiedBy), typeof(Guid?), "\"IdAuditModifiedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.Role.ConcurrencyToken), typeof(Guid), "\"ConcurrencyToken\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.Role.DeletedUtc), typeof(DateTime), "\"DeletedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetRoleTableInfo()
		=> _RoleTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _RolePermissionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "\"RolePermission\"",
				[
					new(nameof(Legion.ADF.Auth.Model.RolePermission.IdRolePermission), typeof(Guid), "\"IdRolePermission\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.RolePermission.IdRole), typeof(Guid), "\"IdRole\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.RolePermission.IdPermission), typeof(Guid), "\"IdPermission\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.RolePermission.AuditCreatedUtc), typeof(DateTime), "\"AuditCreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Auth.Model.RolePermission.AuditModifiedUtc), typeof(DateTime?), "\"AuditModifiedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.RolePermission.IdAuditCreatedBy), typeof(Guid?), "\"IdAuditCreatedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.RolePermission.IdAuditModifiedBy), typeof(Guid?), "\"IdAuditModifiedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.RolePermission.ConcurrencyToken), typeof(Guid), "\"ConcurrencyToken\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.RolePermission.DeletedUtc), typeof(DateTime), "\"DeletedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetRolePermissionTableInfo()
		=> _RolePermissionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _UserTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "\"User\"",
				[
					new(nameof(Legion.ADF.Auth.Model.User.IdUser), typeof(Guid), "\"IdUser\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.User.Login), typeof(string), "\"Login\"", "varchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.User.NormalizedLogin), typeof(string), "\"NormalizedLogin\"", "varchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.User.TenantIdentifier), typeof(Guid?), "\"TenantIdentifier\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.User.Email), typeof(string), "\"Email\"", "varchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.User.NormalizedEmail), typeof(string), "\"NormalizedEmail\"", "varchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.User.EmailConfirmed), typeof(bool), "\"EmailConfirmed\"", "boolean", false),
					new(nameof(Legion.ADF.Auth.Model.User.PasswordHash), typeof(string), "\"PasswordHash\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.User.SecurityStamp), typeof(string), "\"SecurityStamp\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.User.ADDistinguishedName), typeof(string), "\"ADDistinguishedName\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.User.Data), typeof(string), "\"Data\"", "jsonb", true),
					new(nameof(Legion.ADF.Auth.Model.User.PhoneNumber), typeof(string), "\"PhoneNumber\"", "varchar(256)", true),
					new(nameof(Legion.ADF.Auth.Model.User.PhoneNumberConfirmed), typeof(bool), "\"PhoneNumberConfirmed\"", "boolean", false),
					new(nameof(Legion.ADF.Auth.Model.User.MultiFactorEnabled), typeof(bool), "\"MultiFactorEnabled\"", "boolean", false),
					new(nameof(Legion.ADF.Auth.Model.User.LockoutEndUtc), typeof(DateTime?), "\"LockoutEndUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.User.LockoutEnabled), typeof(bool), "\"LockoutEnabled\"", "boolean", false),
					new(nameof(Legion.ADF.Auth.Model.User.AccessFailedCount), typeof(int), "\"AccessFailedCount\"", "integer", false),
					new(nameof(Legion.ADF.Auth.Model.User.IsSystemUser), typeof(bool), "\"IsSystemUser\"", "boolean", false),
					new(nameof(Legion.ADF.Auth.Model.User.ConfirmationUrlSlug), typeof(string), "\"ConfirmationUrlSlug\"", "text", true),
					new(nameof(Legion.ADF.Auth.Model.User.ConfirmationUrlValidToUtc), typeof(DateTime?), "\"ConfirmationUrlValidToUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.User.AuditCreatedUtc), typeof(DateTime), "\"AuditCreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Auth.Model.User.AuditModifiedUtc), typeof(DateTime?), "\"AuditModifiedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.User.IdAuditCreatedBy), typeof(Guid?), "\"IdAuditCreatedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.User.IdAuditModifiedBy), typeof(Guid?), "\"IdAuditModifiedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.User.ConcurrencyToken), typeof(Guid), "\"ConcurrencyToken\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.User.DeletedUtc), typeof(DateTime), "\"DeletedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetUserTableInfo()
		=> _UserTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _UserPermissionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "\"UserPermission\"",
				[
					new(nameof(Legion.ADF.Auth.Model.UserPermission.IdUserPermission), typeof(Guid), "\"IdUserPermission\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserPermission.IdUser), typeof(Guid), "\"IdUser\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserPermission.IdPermission), typeof(Guid), "\"IdPermission\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserPermission.TenantIdentifier), typeof(Guid), "\"TenantIdentifier\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserPermission.AuditCreatedUtc), typeof(DateTime), "\"AuditCreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Auth.Model.UserPermission.AuditModifiedUtc), typeof(DateTime?), "\"AuditModifiedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.UserPermission.IdAuditCreatedBy), typeof(Guid?), "\"IdAuditCreatedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.UserPermission.IdAuditModifiedBy), typeof(Guid?), "\"IdAuditModifiedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.UserPermission.ConcurrencyToken), typeof(Guid), "\"ConcurrencyToken\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserPermission.DeletedUtc), typeof(DateTime), "\"DeletedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetUserPermissionTableInfo()
		=> _UserPermissionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _UserRoleTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "\"UserRole\"",
				[
					new(nameof(Legion.ADF.Auth.Model.UserRole.IdUserRole), typeof(Guid), "\"IdUserRole\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserRole.IdUser), typeof(Guid), "\"IdUser\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserRole.IdRole), typeof(Guid), "\"IdRole\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserRole.TenantIdentifier), typeof(Guid), "\"TenantIdentifier\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserRole.AuditCreatedUtc), typeof(DateTime), "\"AuditCreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Auth.Model.UserRole.AuditModifiedUtc), typeof(DateTime?), "\"AuditModifiedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.UserRole.IdAuditCreatedBy), typeof(Guid?), "\"IdAuditCreatedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.UserRole.IdAuditModifiedBy), typeof(Guid?), "\"IdAuditModifiedBy\"", "uuid", true),
					new(nameof(Legion.ADF.Auth.Model.UserRole.ConcurrencyToken), typeof(Guid), "\"ConcurrencyToken\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserRole.DeletedUtc), typeof(DateTime), "\"DeletedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetUserRoleTableInfo()
		=> _UserRoleTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _UserTokenTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"auth", "\"UserToken\"",
				[
					new(nameof(Legion.ADF.Auth.Model.UserToken.IdUserToken), typeof(Guid), "\"IdUserToken\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserToken.IdLoginProvider), typeof(Guid), "\"IdLoginProvider\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserToken.IdUser), typeof(Guid), "\"IdUser\"", "uuid", false),
					new(nameof(Legion.ADF.Auth.Model.UserToken.Name), typeof(string), "\"Name\"", "text", false),
					new(nameof(Legion.ADF.Auth.Model.UserToken.Value), typeof(string), "\"Value\"", "text", false),
					new(nameof(Legion.ADF.Auth.Model.UserToken.Data), typeof(string), "\"Data\"", "jsonb", true),
					new(nameof(Legion.ADF.Auth.Model.UserToken.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Auth.Model.UserToken.ModifiedUtc), typeof(DateTime?), "\"ModifiedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.UserToken.ValidToUtc), typeof(DateTime), "\"ValidToUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Auth.Model.UserToken.LastAccessUtc), typeof(DateTime?), "\"LastAccessUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Auth.Model.UserToken.RemoteIP), typeof(string), "\"RemoteIP\"", "varchar(64)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetUserTokenTableInfo()
		=> _UserTokenTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Auth.Model.ExternalLogin), GetExternalLoginTableInfo() },
			{ typeof(Legion.ADF.Auth.Model.LoginProvider), GetLoginProviderTableInfo() },
			{ typeof(Legion.ADF.Auth.Model.Permission), GetPermissionTableInfo() },
			{ typeof(Legion.ADF.Auth.Model.Role), GetRoleTableInfo() },
			{ typeof(Legion.ADF.Auth.Model.RolePermission), GetRolePermissionTableInfo() },
			{ typeof(Legion.ADF.Auth.Model.User), GetUserTableInfo() },
			{ typeof(Legion.ADF.Auth.Model.UserPermission), GetUserPermissionTableInfo() },
			{ typeof(Legion.ADF.Auth.Model.UserRole), GetUserRoleTableInfo() },
			{ typeof(Legion.ADF.Auth.Model.UserToken), GetUserTokenTableInfo() },
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
