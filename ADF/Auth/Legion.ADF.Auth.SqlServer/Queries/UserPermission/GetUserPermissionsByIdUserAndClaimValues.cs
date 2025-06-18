using Legion.ADF.Auth.Queries.UserPermission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.UserPermission;

public class GetUserPermissionsByIdUserAndClaimValues :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.UserPermission,
		List<Auth.Model.UserPermission>,
		GetUserPermissionsByIdUserAndClaimValuesQuery>,
	IGetUserPermissionsByIdUserAndClaimValues
{
	public GetUserPermissionsByIdUserAndClaimValues(
		IEFConnectionProvider connectionProvider,
		GetUserPermissionsByIdUserAndClaimValuesQuery getUserPermissionsByIdUserAndClaimValuesQuery)
		: base(connectionProvider, getUserPermissionsByIdUserAndClaimValuesQuery)
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

		if (QueryRequest.ClaimValues == null
			|| QueryRequest.ClaimValues.Count == 0)
			return Enumerable.Empty<Auth.Model.UserPermission>().AsAsyncQueryable();

		if (QueryRequest.GetDeleted)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				up =>
					up.IdUser == QueryRequest.IdUser
					&& QueryRequest.ClaimValues.Any(cv =>
						up.Permission.Code.Equals(cv)
						|| up.Permission.Name.Equals(cv)
						|| (up.Permission.ClaimValue != null && up.Permission.ClaimValue.Equals(cv))));
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
					&& QueryRequest.ClaimValues.Any(cv =>
						up.Permission.Code.Equals(cv)
						|| up.Permission.Name.Equals(cv)
						|| (up.Permission.ClaimValue != null && up.Permission.ClaimValue.Equals(cv))));
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

