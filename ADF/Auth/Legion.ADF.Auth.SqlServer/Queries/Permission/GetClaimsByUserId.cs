using Legion.ADF.Auth.Queries.Permission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.Permission;

public class GetClaimsByUserId :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Permission,
		List<System.Security.Claims.Claim>,
		GetClaimsByUserIdQuery>,
	IGetClaimsByUserId
{
	public GetClaimsByUserId(
		IEFConnectionProvider connectionProvider,
		GetClaimsByUserIdQuery getClaimsByUserIdQuery)
		: base(connectionProvider, getClaimsByUserIdQuery)
	{
	}

	protected override IQueryable<Auth.Model.Permission> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Permission
			.Include(x => x.UserPermissions).ThenInclude(up => up.User);
	}

	public override IQueryable<Auth.Model.Permission> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				p => p.UserPermissions.Any(rp => rp.IdUser == QueryRequest.IdUser));
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				p => p.UserPermissions.Any(rp => rp.IdUser == QueryRequest.IdUser && rp.DeletedUtc == DateTime.MinValue && rp.User.DeletedUtc == DateTime.MinValue));
		}
	}

	public override async Task<List<System.Security.Claims.Claim>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(p => string.IsNullOrWhiteSpace(p.ClaimValue)
				? new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, p.Code)
				: new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, p.ClaimValue!))
			.ToListAsync(cancellationToken);
	}
}

