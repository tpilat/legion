namespace Legion.ADF.Logs.Services;

public partial class LogsStore : IDisposable, IAsyncDisposable
{
	public async Task<IResult<Model.LocalRequest>> SaveLocalRequestAsync(
		IScopeContext scopeContext,
		Model.LocalRequest request,
		bool checkPermissions,
		Model.LocalRequestPayload? payload = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.LocalRequest>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.LocalRequestResponse.SaveLocalRequest);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, request) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.LocalRequestRepository.Add(scopeContext, request);
		
		if (payload != null)
			UoW.LocalRequestPayloadRepository.Add(scopeContext, payload);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public async Task<IResult<Model.LocalResponse>> SaveLocalResponseAsync(
		IScopeContext scopeContext,
		Model.LocalResponse response,
		bool checkPermissions,
		Model.LocalResponsePayload? payload = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.LocalResponse>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, response))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.LocalRequestResponse.SaveLocalResponse);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, response) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.LocalResponseRepository.Add(scopeContext, response);

		if (payload != null)
			UoW.LocalResponsePayloadRepository.Add(scopeContext, payload);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public async Task<IResult<Model.LocalRequest>> SaveLocalRequestPayloadAsync(
		IScopeContext scopeContext,
		Model.LocalRequestPayload payload,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.LocalRequest>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, payload))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.LocalRequestResponse.SaveLocalRequestPayload);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, payload) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.LocalRequestPayloadRepository.Add(scopeContext, payload);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public async Task<IResult<Model.LocalResponse>> SaveLocalResponsePayloadAsync(
		IScopeContext scopeContext,
		Model.LocalResponsePayload payload,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.LocalResponse>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, payload))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.LocalRequestResponse.SaveLocalResponsePayload);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, payload) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.LocalResponsePayloadRepository.Add(scopeContext, payload);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}
}
