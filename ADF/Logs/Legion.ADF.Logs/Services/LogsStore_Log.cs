namespace Legion.ADF.Logs.Services;

public partial class LogsStore : IDisposable, IAsyncDisposable
{
	public async Task<IResult<Model.Log>> SaveLogAsync(
		IScopeContext scopeContext,
		Model.Log log,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.Log>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, log))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.Log.SaveLog);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, log) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.LogRepository.Add(scopeContext, log);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public async Task<IResult<Model.Log>> SaveLogsAsync(
		IScopeContext scopeContext,
		IEnumerable<Model.Log> logs,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.Log>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, logs))
			return result.Build();

		var operationName = nameof(LogPermissions.Log.SaveLog);

		foreach (var log in logs)
		{
			if (checkPermissions)
			{
				if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, log) == false)
					return result.WithUnauthorizedException(scopeContext, null, operationName);
			}

			UoW.LogRepository.Add(scopeContext, log);
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public IResult<Model.Log> SaveLog(
		IScopeContext scopeContext,
		Model.Log log,
		bool checkPermissions)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.Log>();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, log))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.Log.SaveLog);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, log) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.LogRepository.Add(scopeContext, log);

		var saveResult = SaveInternal(scopeContext, force: false);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public IResult<Model.Log> SaveLogs(
		IScopeContext scopeContext,
		IEnumerable<Model.Log> logs,
		bool checkPermissions)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.Log>();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, logs))
			return result.Build();

		var operationName = nameof(LogPermissions.Log.SaveLog);

		foreach (var log in logs)
		{
			if (checkPermissions)
			{
				if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, log) == false)
					return result.WithUnauthorizedException(scopeContext, null, operationName);
			}

			UoW.LogRepository.Add(scopeContext, log);
		}

		var saveResult = SaveInternal(scopeContext, force: false);
		result.MergeHasError(saveResult);
		return result.Build();
	}
}
