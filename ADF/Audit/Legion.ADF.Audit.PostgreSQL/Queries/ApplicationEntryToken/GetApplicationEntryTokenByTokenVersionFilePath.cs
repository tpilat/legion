using Legion.ADF.Audit.Queries.ApplicationEntryToken;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Audit.PostgreSQL.Queries.ApplicationEntryToken;

public class GetApplicationEntryTokenByTokenVersionFilePath :
	QueryDefinition<
		IAuditDbContext,
		Audit.Model.ApplicationEntryToken,
		Audit.Model.ApplicationEntryToken?,
		GetApplicationEntryTokenByTokenVersionFilePathQuery>,
	IGetApplicationEntryTokenByTokenVersionFilePath
{
	public GetApplicationEntryTokenByTokenVersionFilePath(
		IEFConnectionProvider connectionProvider,
		GetApplicationEntryTokenByTokenVersionFilePathQuery getApplicationEntryTokenByTokenVersionFilePath)
		: base(connectionProvider, getApplicationEntryTokenByTokenVersionFilePath)
	{
	}

	protected override IQueryable<Audit.Model.ApplicationEntryToken> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.ApplicationEntryToken;
	}

	public override IQueryable<Audit.Model.ApplicationEntryToken> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			u => u.Token == QueryRequest.Token
				&& u.SourceFilePath == QueryRequest.SourceFilePath);
	}

	public override async Task<Audit.Model.ApplicationEntryToken?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}

