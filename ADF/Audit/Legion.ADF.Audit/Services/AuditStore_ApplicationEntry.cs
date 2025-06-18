using Legion.Model.Audit;

namespace Legion.ADF.Audit.Services;

public partial class AuditStore : IAuditEntryStore, IDisposable, IAsyncDisposable
{
	public async Task<IResult<Guid>> WriteApplicationEntryAsync(
		IScopeContext scopeContext,
		Guid idApplicationEntryToken,
		Guid idAuditOperation,
		string? aggregateIdentifier,
		string? uri,
		bool checkPermissions,
		List<DTOs.Content> requestData,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Guid>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var createResult = Model.ApplicationEntry.Create(
			scopeContext,
			idApplicationEntryToken,
			idAuditOperation,
			aggregateIdentifier,
			uri);

		if (result.MergeHasError(createResult))
			return result.Build();

		var applicationEntry = createResult.Data!;

		if (checkPermissions)
		{
			var operationName = nameof(AuditPermissions.ApplicationEntry.WriteApplicationEntry);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, applicationEntry) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.ApplicationEntryRepository.Add(scopeContext, applicationEntry);

		if (0 < requestData?.Count)
		{
			foreach (var request in requestData)
			{
				var createRequestResult = Model.ApplicationEntryRequest.Create(
					scopeContext,
					applicationEntry,
					request);

				if (result.MergeHasError(createRequestResult))
					return result.Build();

				UoW.ApplicationEntryRequestRepository.Add(scopeContext, createRequestResult.Data!);
			}
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(applicationEntry.IdApplicationEntry).Build();
	}

	public async Task<IResult<List<Guid>>> WriteApplicationEntryResponseAsync(
		IScopeContext scopeContext,
		Guid idApplicationEntry,
		string statusCode,
		string? error,
		decimal elapsedMilliseconds,
		List<DTOs.Content>? responseData,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<Guid>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var responseIds = new List<Guid>();

		if (0 < responseData?.Count)
		{
			foreach (var response in responseData)
			{
				var createResult = Model.ApplicationEntryResponse.Create(
					scopeContext,
					idApplicationEntry,
					statusCode,
					error,
					elapsedMilliseconds,
					response);

				if (result.MergeHasError(createResult))
					return result.Build();

				UoW.ApplicationEntryResponseRepository.Add(scopeContext, createResult.Data!);

				responseIds.Add(createResult.Data!.IdApplicationEntryResponse);
			}
		}
		else
		{
			var createResult = Model.ApplicationEntryResponse.Create(
				scopeContext,
				idApplicationEntry,
				statusCode,
				error,
				elapsedMilliseconds);

			if (result.MergeHasError(createResult))
				return result.Build();

			UoW.ApplicationEntryResponseRepository.Add(scopeContext, createResult.Data!);

			responseIds.Add(createResult.Data!.IdApplicationEntryResponse);
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(responseIds).Build();
	}
}
