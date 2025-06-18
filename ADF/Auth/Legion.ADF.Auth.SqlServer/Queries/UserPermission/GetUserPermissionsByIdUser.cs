using Legion.ADF.Auth.Queries.UserPermission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.UserPermission;

public class GetUserPermissionsByIdUser :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.UserPermission,
		List<Auth.Model.UserPermission>,
		GetUserPermissionsByIdUserQuery>,
	IGetUserPermissionsByIdUser
{
	public GetUserPermissionsByIdUser(
		IEFConnectionProvider connectionProvider,
		GetUserPermissionsByIdUserQuery getUserPermissionsByIdUserQuery)
		: base(connectionProvider, getUserPermissionsByIdUserQuery)
	{
	}

	protected override IQueryable<Auth.Model.UserPermission> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.UserPermission.Include(up => up.User);
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
				up => up.IdUser == QueryRequest.IdUser);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				up => up.DeletedUtc == DateTime.MinValue
					&& up.IdUser == QueryRequest.IdUser
					&& up.User.DeletedUtc == DateTime.MinValue);
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

