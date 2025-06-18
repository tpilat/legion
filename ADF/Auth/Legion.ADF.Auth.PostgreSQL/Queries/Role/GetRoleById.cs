using Legion.ADF.Auth.Queries.Role;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.PostgreSQL.Queries.Role;

public class GetRoleById :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Role,
		Auth.Model.Role?,
		GetRoleByIdQuery>,
	IGetRoleById
{
	public GetRoleById(
		IEFConnectionProvider connectionProvider,
		GetRoleByIdQuery getRoleByIdQuery)
		: base(connectionProvider, getRoleByIdQuery)
	{
	}

	protected override IQueryable<Auth.Model.Role> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Role;
	}

	public override IQueryable<Auth.Model.Role> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				r => r.IdRole == QueryRequest.IdRole);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				r => r.IdRole == QueryRequest.IdRole && r.DeletedUtc == DateTime.MinValue);
		}
	}

	public override async Task<Auth.Model.Role?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}

