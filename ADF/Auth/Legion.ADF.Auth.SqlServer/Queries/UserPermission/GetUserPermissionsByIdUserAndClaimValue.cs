using Legion.ADF.Auth.Queries.UserPermission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.UserPermission;

public class GetUserPermissionsByIdUserAndClaimValue :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.UserPermission,
		List<Auth.Model.UserPermission>,
		GetUserPermissionsByIdUserAndClaimValueQuery>,
	IGetUserPermissionsByIdUserAndClaimValue
{
	public GetUserPermissionsByIdUserAndClaimValue(
		IEFConnectionProvider connectionProvider,
		GetUserPermissionsByIdUserAndClaimValueQuery getUserPermissionsByIdUserAndClaimValueQuery)
		: base(connectionProvider, getUserPermissionsByIdUserAndClaimValueQuery)
	{
	}

	protected override IQueryable<Auth.Model.UserPermission> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.UserPermission
			.Include(up => up.User)
			.Include(up => up.Permission);
	}

	public override IQueryable<Auth.Model.UserPermission> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				up =>
					up.IdUser == QueryRequest.IdUser
						&& (up.Permission.Code.Equals(QueryRequest.ClaimValue)
						|| up.Permission.Name.Equals(QueryRequest.ClaimValue)
						|| (up.Permission.ClaimValue != null && up.Permission.ClaimValue.Equals(QueryRequest.ClaimValue))));
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				up =>
					up.DeletedUtc == DateTime.MinValue
					&& up.User.DeletedUtc == DateTime.MinValue
					&& up.IdUser == QueryRequest.IdUser
						&& (up.Permission.Code.Equals(QueryRequest.ClaimValue)
						|| up.Permission.Name.Equals(QueryRequest.ClaimValue)
						|| (up.Permission.ClaimValue != null && up.Permission.ClaimValue.Equals(QueryRequest.ClaimValue))));
		}
	}

	public override async Task<List<Auth.Model.UserPermission>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}
}

