using Legion.ADF.Auth.Queries.UserRole;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.UserRole;

public class GetUserRoleByIdUserAndIdRole :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.UserRole,
		Auth.Model.UserRole?,
		GetUserRoleByIdUserAndIdRoleQuery>,
	IGetUserRoleByIdUserAndIdRole
{
	public GetUserRoleByIdUserAndIdRole(
		IEFConnectionProvider connectionProvider,
		GetUserRoleByIdUserAndIdRoleQuery getUserRoleByIdUserAndIdRole)
		: base(connectionProvider, getUserRoleByIdUserAndIdRole)
	{
	}

	protected override IQueryable<Auth.Model.UserRole> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.UserRole
			.Include(ur => ur.User)
			.Include(ur => ur.Role);
	}

	public override IQueryable<Auth.Model.UserRole> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				rp => rp.IdUser == QueryRequest.IdUser
					&& rp.IdRole == QueryRequest.IdRole);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				rp => rp.IdUser == QueryRequest.IdUser
					&& rp.IdRole == QueryRequest.IdRole
					&& rp.DeletedUtc == DateTime.MinValue
					&& rp.User.DeletedUtc == DateTime.MinValue
					&& rp.Role.DeletedUtc == DateTime.MinValue);
		}
	}

	public override async Task<Auth.Model.UserRole?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}

