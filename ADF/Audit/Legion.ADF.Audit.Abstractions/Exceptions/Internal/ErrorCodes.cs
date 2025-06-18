using Legion.Exceptions;

namespace Legion.ADF.Audit.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class ConnectionStringProviderException
	{
		public static IErrorCode InvalidStoreId(string storeId)
			=> new ErrorCode(
				"L_AUD_CONN-STR_0001",
				$"Invalid connection string strore ID = {storeId}");
	}

	public static partial class AuditUnitOfWorkException
	{
		public static IErrorCode InvalidUoW => _invalidUoW.Value;
		private static readonly Lazy<IErrorCode> _invalidUoW = new(() =>
			new ErrorCode(
				"L_AUD_UoW_0001",
				$"Cannot create UnitOfWork"));
	}

	public static partial class RoleStoreException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_AUD_RoleStore_0001",
				$"Cannot save to role store."));
	}

	public static partial class UserStoreException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_AUD_UserStore_0001",
				$"Cannot save to user store."));
	}

	public static partial class RolePermissionException
	{
		public static IErrorCode InvalidRolePermission => _invalidRolePermission.Value;
		private static readonly Lazy<IErrorCode> _invalidRolePermission = new(() =>
			new ErrorCode(
				"L_AUD_RolePerm_0001",
				$"Cannot create RolePermission"));
	}

	public static partial class UserPermissionException
	{
		public static IErrorCode InvalidUserPermission => _invalidUserPermission.Value;
		private static readonly Lazy<IErrorCode> _invalidUserPermission = new(() =>
			new ErrorCode(
				"L_AUD_UserPerm_0001",
				$"Cannot create UserPermission"));
	}

	public static partial class UserRoleException
	{
		public static IErrorCode InvalidUserRole => _invalidUserRole.Value;
		private static readonly Lazy<IErrorCode> _invalidUserRole = new(() =>
			new ErrorCode(
				"L_AUD_UserPerm_0001",
				$"Cannot create UserRole"));
	}

	public static partial class UserTokenException
	{
		public static IErrorCode InvalidUserToken => _invalidUserToken.Value;
		private static readonly Lazy<IErrorCode> _invalidUserToken = new(() =>
			new ErrorCode(
				"L_AUD_RolePerm_0001",
				$"Cannot create user token"));
	}

	public static partial class ExternalLoginException
	{
		public static IErrorCode InvalidExternalLogin => _invalidExternalLogin.Value;
		private static readonly Lazy<IErrorCode> _invalidExternalLogin = new(() =>
			new ErrorCode(
				"L_AUD_ExtLogin_0001",
				$"Cannot create external login"));
	}

	public static partial class RoleException
	{
		public static IErrorCode InvalidRoleName => _invalidRoleName.Value;
		private static readonly Lazy<IErrorCode> _invalidRoleName = new(() =>
			new ErrorCode(
				"L_AUD_Role_0001",
				$"Invali role name"));

		public static IErrorCode InvalidRoleNormalizedName => _invalidRoleNormalizedName.Value;
		private static readonly Lazy<IErrorCode> _invalidRoleNormalizedName = new(() =>
			new ErrorCode(
				"L_AUD_Role_0002",
				$"Invali role normalized name"));
	}

	public static partial class UserException
	{
		public static IErrorCode InvalidLogin => _invalidLogin.Value;
		private static readonly Lazy<IErrorCode> _invalidLogin = new(() =>
			new ErrorCode(
				"L_AUD_User_0001",
				$"Invali login"));

		public static IErrorCode InvalidNormalizedLogin => _invalidNormalizedLogin.Value;
		private static readonly Lazy<IErrorCode> _invalidNormalizedLogin = new(() =>
			new ErrorCode(
				"L_AUD_User_0002",
				$"Invali normalized login"));
	}
}
