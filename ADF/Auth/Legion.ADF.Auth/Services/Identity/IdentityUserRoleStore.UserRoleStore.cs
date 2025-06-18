using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserRoleStore : IdentityUserOnlyStore,
	IUserRoleStore<Model.User>
{
	public async Task AddToRoleAsync(Model.User user, string normalizedRoleName, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);
		Throw.IfArgumentNullOrWhiteSpace(normalizedRoleName);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		var role = await FindRoleAsync(scopeContext, normalizedRoleName, cancellationToken);
		if (role == null)
			Throw.InvalidOperationException($"{nameof(role)} == null", scopeContext);

		var userRoleResult = Model.UserRole.CreateUserRole(
			scopeContext,
			user.IdUser,
			role.IdRole);

		if (userRoleResult.HasError)
			userRoleResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.UserRoleException.InvalidUserRole, true);

		UoW.UserRoleRepository.Add(scopeContext, userRoleResult.Data!);
	}

	public async Task RemoveFromRoleAsync(Model.User user, string normalizedRoleName, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);
		Throw.IfArgumentNullOrWhiteSpace(normalizedRoleName);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty(nameof(normalizedRoleName), normalizedRoleName);

		var userRole = await UoW.UserRoleRepository
			.GetUserRoleByIdUserAndNormalizedRoleName(new Queries.UserRole.GetValidUserRoleByIdUserAndNormalizedRoleNameQuery(user.IdUser, normalizedRoleName, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		userRole?.SetSoftDelete(scopeContext);
	}

	public async Task<IList<string>> GetRolesAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		return await UoW.RoleRepository
			.GetRolesByIdUser(new Queries.Role.GetValidRolesByIdUserQuery(user.IdUser, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);
	}

	public async Task<bool> IsInRoleAsync(Model.User user, string normalizedRoleName, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);
		Throw.IfArgumentNullOrWhiteSpace(normalizedRoleName);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty(nameof(normalizedRoleName), normalizedRoleName);

		return await UoW.UserRoleRepository
			.IsInRole(new Queries.UserRole.IsInValidRoleQuery(user.IdUser, normalizedRoleName, CheckReadPermissions: false, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);
	}

	public async Task<IList<Model.User>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNullOrWhiteSpace(normalizedRoleName);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(normalizedRoleName), normalizedRoleName);

		return await UoW.UserRepository
			.GetUserByNormalizedRoleName(new Queries.User.GetValidUserByNormalizedRoleNameQuery(normalizedRoleName, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);
	}

	protected async Task<Model.Role?> FindRoleAsync(IScopeContext scopeContext, string normalizedRoleName, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		scopeContext = ScopeContext.Create(scopeContext);

		return await UoW.RoleRepository
			.GetRoleByNormalizedName(new Queries.Role.GetValidRoleByNormalizedNameQuery(normalizedRoleName, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);
	}

	protected async Task<Model.UserRole?> FindUserRoleAsync(IScopeContext scopeContext, Guid userId, Guid roleId, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		scopeContext = ScopeContext.Create(scopeContext);

		return await UoW.UserRoleRepository
			.GetUserRoleByIdUserAndIdRole(new Queries.UserRole.GetValidUserRoleByIdUserAndIdRoleQuery(userId, roleId, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);
	}
}
