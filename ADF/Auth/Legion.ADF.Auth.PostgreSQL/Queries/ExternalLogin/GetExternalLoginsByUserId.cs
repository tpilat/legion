using Legion.ADF.Auth.Queries.ExternalLogin;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.PostgreSQL.Queries.ExternalLogin;

public class GetExternalLoginsByUserId :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.ExternalLogin,
		List<Auth.Model.ExternalLogin>,
		GetExternalLoginsByUserIdQuery>,
	IGetExternalLoginsByUserId
{
	public GetExternalLoginsByUserId(
		IEFConnectionProvider connectionProvider,
		GetExternalLoginsByUserIdQuery getExternalLoginsByUserIdQuery)
		: base(connectionProvider, getExternalLoginsByUserIdQuery)
	{
	}

	protected override IQueryable<Auth.Model.ExternalLogin> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.ExternalLogin
			.Include(el => el.LoginProvider);
	}

	public override IQueryable<Auth.Model.ExternalLogin> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.ValidToUtc.HasValue)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				el => el.IdUser == QueryRequest.IdUser
					&& QueryRequest.ValidToUtc.Value <= el.ValidToUtc);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				el => el.IdUser == QueryRequest.IdUser);
		}
	}

	public override async Task<List<Auth.Model.ExternalLogin>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}
}

