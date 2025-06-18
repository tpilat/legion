namespace Legion.ADF.Logs.Services;

public partial class LogsStore : IDisposable, IAsyncDisposable
{
	public async Task<IResult<Model.RemoteRequest>> SaveRemoteRequestAsync(
		IScopeContext scopeContext,
		Model.RemoteRequest request,
		bool checkPermissions,
		Model.RemoteRequestPayload? payload = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.RemoteRequest>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.RemoteRequestResponse.SaveRemoteRequest);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, request) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.RemoteRequestRepository.Add(scopeContext, request);
		
		if (payload != null)
			UoW.RemoteRequestPayloadRepository.Add(scopeContext, payload);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public async Task<IResult<Model.RemoteResponse>> SaveRemoteResponseAsync(
		IScopeContext scopeContext,
		Model.RemoteResponse response,
		bool checkPermissions,
		Model.RemoteResponsePayload? payload = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.RemoteResponse>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, response))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.RemoteRequestResponse.SaveRemoteResponse);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, response) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.RemoteResponseRepository.Add(scopeContext, response);

		if (payload != null)
			UoW.RemoteResponsePayloadRepository.Add(scopeContext, payload);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public async Task<IResult<Model.RemoteRequest>> SaveRemoteRequestPayloadAsync(
		IScopeContext scopeContext,
		Model.RemoteRequestPayload payload,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.RemoteRequest>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, payload))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.RemoteRequestResponse.SaveRemoteRequestPayload);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, payload) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.RemoteRequestPayloadRepository.Add(scopeContext, payload);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public async Task<IResult<Model.RemoteResponse>> SaveRemoteResponsePayloadAsync(
		IScopeContext scopeContext,
		Model.RemoteResponsePayload payload,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.RemoteResponse>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, payload))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.RemoteRequestResponse.SaveRemoteResponsePayload);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, payload) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.RemoteResponsePayloadRepository.Add(scopeContext, payload);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}
}
