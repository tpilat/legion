using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityRoleStore : IRoleClaimStore<Model.Role>
{
	public async Task AddClaimAsync(Model.Role role, Claim claim, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);
		Throw.IfArgumentNull(claim);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(role.IdRole), role.IdRole.ToString())
			.AddContextProperty(nameof(claim), claim.Value);

		var permission = await UoW.PermissionRepository
			.GetPermissionByClaimValue(
				new Queries.Permission.GetPermissionByClaimValueQuery(claim.Value, CheckReadPermissions: false, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		if (permission == null)
			Throw.InvalidOperationException($"{nameof(permission)} == null", scopeContext);

		var rolePermissionResult = Model.RolePermission.CreateRolePermission(
			scopeContext,
			role.IdRole,
			permission.IdPermission);

		if (rolePermissionResult.HasError)
			rolePermissionResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.RolePermissionException.InvalidRolePermission, true);

		UoW.RolePermissionRepository.Add(scopeContext, rolePermissionResult.Data!);
	}

	public async Task<IList<Claim>> GetClaimsAsync(Model.Role role, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(role.IdRole), role.IdRole.ToString());

		return await UoW.PermissionRepository
			.GetClaimsByRoleId(
				new Queries.Permission.GetValidClaimsByRoleIdQuery(role.IdRole, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);
	}

	public async Task RemoveClaimAsync(Model.Role role, Claim claim, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(role);
		Throw.IfArgumentNull(claim);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(role.IdRole), role.IdRole.ToString())
			.AddContextProperty(nameof(claim), claim.Value);

		var rolePermissions = await UoW.RolePermissionRepository
			.GetRolePermissionsByRoleIdAndClaimValue(
				new Queries.RolePermission.GetValidRolePermissionsByRoleIdAndClaimValueQuery(role.IdRole, claim.Value, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);

		foreach (var rolePermission in rolePermissions)
			rolePermission.SetSoftDelete(scopeContext);
	}
}
