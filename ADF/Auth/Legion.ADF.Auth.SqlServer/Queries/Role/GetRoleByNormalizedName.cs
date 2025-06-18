using Legion.ADF.Auth.Queries.Role;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.Role;

public class GetRoleByNormalizedName :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Role,
		Auth.Model.Role?,
		GetRoleByNormalizedNameQuery>,
	IGetRoleByNormalizedName
{
	public GetRoleByNormalizedName(
		IEFConnectionProvider connectionProvider,
		GetRoleByNormalizedNameQuery getRoleByNormalizedNameQuery)
		: base(connectionProvider, getRoleByNormalizedNameQuery)
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
				r => r.NormalizedName == QueryRequest.NormalizedRoleName);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				r => r.NormalizedName == QueryRequest.NormalizedRoleName && r.DeletedUtc == DateTime.MinValue);
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

