using Legion.ADF.Auth.Queries.LoginProvider;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.LoginProvider;

public class GetLoginProviderByName :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.LoginProvider,
		Auth.Model.LoginProvider?,
		GetLoginProviderByNameQuery>,
	IGetLoginProviderByName
{
	public GetLoginProviderByName(
		IEFConnectionProvider connectionProvider,
		GetLoginProviderByNameQuery getLoginProviderByNameQuery)
		: base(connectionProvider, getLoginProviderByNameQuery)
	{
	}

	protected override IQueryable<Auth.Model.LoginProvider> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.LoginProvider;
	}

	public override IQueryable<Auth.Model.LoginProvider> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.GetDisabledProviders)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				lp => lp.Name == QueryRequest.Name);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				lp => lp.Name == QueryRequest.Name
					&& !lp.DisabledUtc.HasValue);
		}
	}

	public override async Task<Auth.Model.LoginProvider?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}

