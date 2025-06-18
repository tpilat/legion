using Legion.Model.Audit;

namespace Legion.ADF.Audit.Services;

public partial class AuditStore : IAuditEntryStore, IDisposable, IAsyncDisposable
{
	public async Task<IResult<Model.ApplicationEntryToken>> SaveApplicationEntryTokenAsync(
		IScopeContext scopeContext,
		string token,
		string sourceFilePath,
		string? methodInfo,
		string? aggregateName,
		string? description,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.ApplicationEntryToken>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, token))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, sourceFilePath))
			return result.Build();

		var dbApplicationEntryToken = await UoW.ApplicationEntryTokenRepository
			.GetApplicationEntryTokenByTokenVersionFilePath(new Queries.ApplicationEntryToken.GetApplicationEntryTokenByTokenVersionFilePathQuery(token, sourceFilePath, checkPermissions))
			.ToResultAsync(scopeContext, cancellationToken);

		if (checkPermissions)
		{
			var operationName = nameof(AuditPermissions.ApplicationEntryToken.SaveApplicationEntryToken);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbApplicationEntryToken) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		if (dbApplicationEntryToken != null)
		{
			var updateResult = dbApplicationEntryToken.Update(
				scopeContext,
				methodInfo,
				aggregateName,
				description);

			if (result.MergeHasError(updateResult))
				return result.Build();
		}
		else
		{
			var createResult = Model.ApplicationEntryToken.Create(
				scopeContext,
				token,
				sourceFilePath,
				methodInfo,
				aggregateName,
				description);

			if (result.MergeHasError(createResult))
				return result.Build();

			dbApplicationEntryToken = createResult.Data!;

			UoW.ApplicationEntryTokenRepository.Add(scopeContext, dbApplicationEntryToken);
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbApplicationEntryToken).Build();
	}
}
