using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Auth;

public partial interface IAuthUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.Auth.Model.Repositories.IExternalLoginRepository ExternalLoginRepository { get; }

	Legion.ADF.Auth.Model.Repositories.ILoginProviderRepository LoginProviderRepository { get; }

	Legion.ADF.Auth.Model.Repositories.IPermissionRepository PermissionRepository { get; }

	Legion.ADF.Auth.Model.Repositories.IRoleRepository RoleRepository { get; }

	Legion.ADF.Auth.Model.Repositories.IRolePermissionRepository RolePermissionRepository { get; }

	Legion.ADF.Auth.Model.Repositories.IUserRepository UserRepository { get; }

	Legion.ADF.Auth.Model.Repositories.IUserPermissionRepository UserPermissionRepository { get; }

	Legion.ADF.Auth.Model.Repositories.IUserRoleRepository UserRoleRepository { get; }

	Legion.ADF.Auth.Model.Repositories.IUserTokenRepository UserTokenRepository { get; }
}
