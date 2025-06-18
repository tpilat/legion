using Legion.ADF.Auth.Queries.UserRole;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.PostgreSQL.Queries.UserRole;

public class GetUserRoleByIdUserAndNormalizedRoleName :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.UserRole,
		Auth.Model.UserRole?,
		GetUserRoleByIdUserAndNormalizedRoleNameQuery>,
	IGetUserRoleByIdUserAndNormalizedRoleName
{
	public GetUserRoleByIdUserAndNormalizedRoleName(
		IEFConnectionProvider connectionProvider,
		GetUserRoleByIdUserAndNormalizedRoleNameQuery getUserRoleByIdUserAndNormalizedRoleName)
		: base(connectionProvider, getUserRoleByIdUserAndNormalizedRoleName)
	{
	}

	protected override IQueryable<Auth.Model.UserRole> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.UserRole
			.Include(ur => ur.Role)
			.Include(ur => ur.User);
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
					&& rp.Role.NormalizedName == QueryRequest.NormalizedRoleName);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				rp => rp.IdUser == QueryRequest.IdUser
					&& rp.Role.NormalizedName == QueryRequest.NormalizedRoleName
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

