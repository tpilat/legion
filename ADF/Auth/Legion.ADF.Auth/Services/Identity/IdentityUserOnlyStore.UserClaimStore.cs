using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserClaimStore<Model.User>
{
	public async Task AddClaimsAsync(Model.User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);
		Legion.Throw.IfArgumentNullOrEmpty(claims);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty($"{nameof(claims)}.Count", claims.Count().ToString());

		//TODO ALLOW EF CACHE - aby sa tie iste permissiony nevytahovali dookola z DB

		var permissions = await UoW.PermissionRepository
			.GetPermissionsByClaimValues(
				new Queries.Permission.GetPermissionsByClaimValuesQuery(claims.Select(c => c.Value).ToList(), CheckReadPermissions: false, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		if (permissions == null || permissions.Count == 0)
			Throw.InvalidOperationException($"{nameof(permissions)} == null", scopeContext);

		foreach (var permission in permissions)
		{
			var userPermissionResult = Model.UserPermission.CreateUserPermission(
				scopeContext,
				user.IdUser,
				permission.IdPermission);

			if (userPermissionResult.HasError)
				userPermissionResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.UserPermissionException.InvalidUserPermission, true);

			UoW.UserPermissionRepository.Add(scopeContext, userPermissionResult.Data!);
		}
	}

	public async Task<IList<Claim>> GetClaimsAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		//TODO ALLOW EF CACHE - aby sa tie iste permissiony nevytahovali dookola z DB

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		return await UoW.PermissionRepository
			.GetClaimsByUserId(
				new Queries.Permission.GetValidClaimsByUserIdQuery(user.IdUser, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);
	}

	public async Task<IList<Model.User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(claim);

		//TODO ALLOW EF CACHE - aby sa tie iste permissiony nevytahovali dookola z DB

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(claim), claim.Value);

		return await UoW.UserRepository
			.GetUsersByClaimValue(
				new Queries.User.GetValidUsersByClaimValueQuery(claim.Value, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);
	}

	public async Task RemoveClaimsAsync(Model.User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);
		Legion.Throw.IfArgumentNullOrEmpty(claims);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty($"{nameof(claims)}.Count", claims.Count().ToString());

		//TODO ALLOW EF CACHE - aby sa tie iste permissiony nevytahovali dookola z DB

		var userPermissions = await UoW.UserPermissionRepository
			.GetUserPermissionsByIdUserAndClaimValues(
				new Queries.UserPermission.GetValidUserPermissionsByIdUserAndClaimValuesQuery(user.IdUser, claims.Select(c => c.Value).ToList(), CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		if (userPermissions == null || userPermissions.Count == 0)
			return;

		foreach (var userPermission in userPermissions)
			userPermission.SetSoftDelete(scopeContext);
	}

	public async Task ReplaceClaimAsync(Model.User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);
		Legion.Throw.IfArgumentNull(claim);
		Legion.Throw.IfArgumentNull(newClaim);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString())
			.AddContextProperty(nameof(claim), claim.Value)
			.AddContextProperty(nameof(newClaim), newClaim.Value);

		//TODO ALLOW EF CACHE - aby sa tie iste permissiony nevytahovali dookola z DB

		var claimPermission = await UoW.PermissionRepository
			.GetPermissionByClaimValue(
				new Queries.Permission.GetPermissionByClaimValueQuery(claim.Value, CheckReadPermissions: false, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		Throw.IfNull(claimPermission, scopeContext);

		var newClaimPermission = await UoW.PermissionRepository
			.GetPermissionByClaimValue(
				new Queries.Permission.GetPermissionByClaimValueQuery(newClaim.Value, CheckReadPermissions: false, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		Throw.IfNull(newClaimPermission, scopeContext);

		var userPermissions = await UoW.UserPermissionRepository
			.GetUserPermissionsByIdUser(
				new Queries.UserPermission.GetValidUserPermissionsByIdUserQuery(user.IdUser, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		foreach (var userPermission in userPermissions)
			if (userPermission.IdPermission == claimPermission.IdPermission)
				userPermission.SetSoftDelete(scopeContext);

		var userPermissionResult = Model.UserPermission.CreateUserPermission(
			scopeContext,
			user.IdUser,
			newClaimPermission.IdPermission);

		if (userPermissionResult.HasError)
			userPermissionResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.UserPermissionException.InvalidUserPermission, true);

		UoW.UserPermissionRepository.Add(scopeContext, userPermissionResult.Data!);
	}
}
