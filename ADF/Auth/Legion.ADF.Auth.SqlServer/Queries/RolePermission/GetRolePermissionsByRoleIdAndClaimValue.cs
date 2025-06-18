using Legion.ADF.Auth.Queries.RolePermission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.RolePermission;

public class GetRolePermissionsByRoleIdAndClaimValue :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.RolePermission,
		List<Auth.Model.RolePermission>,
		GetRolePermissionsByRoleIdAndClaimValueQuery>,
	IGetRolePermissionsByRoleIdAndClaimValue
{
	public GetRolePermissionsByRoleIdAndClaimValue(
		IEFConnectionProvider connectionProvider,
		GetRolePermissionsByRoleIdAndClaimValueQuery getRolePermissionsByRoleIdAndClaimValueQuery)
		: base(connectionProvider, getRolePermissionsByRoleIdAndClaimValueQuery)
	{
	}

	protected override IQueryable<Auth.Model.RolePermission> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.RolePermission
			.Include(rp => rp.Permission)
			.Include(rp => rp.Role);
	}

	public override IQueryable<Auth.Model.RolePermission> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IAuthAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			rp => rp.IdRole == QueryRequest.IdRole
				&& rp.DeletedUtc == DateTime.MinValue
				&& rp.Role.DeletedUtc == DateTime.MinValue
				&& ((string.IsNullOrWhiteSpace(rp.Permission.ClaimValue) && rp.Permission.Code == QueryRequest.ClaimValue)
					|| (!string.IsNullOrWhiteSpace(rp.Permission.ClaimValue) && rp.Permission.ClaimValue == QueryRequest.ClaimValue)));
	}

	public override async Task<List<Auth.Model.RolePermission>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}
}

