namespace Legion.ADF.Logs.Services;

public partial class LogsStore : IDisposable, IAsyncDisposable
{
	public async Task<IResult<Model.UnstructuredLog>> SaveUnstructuredLogAsync(
		IScopeContext scopeContext,
		Model.UnstructuredLog unstructuredLog,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.UnstructuredLog>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, unstructuredLog))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.UnstructuredLog.SaveUnstructuredLog);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, unstructuredLog) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.UnstructuredLogRepository.Add(scopeContext, unstructuredLog);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public async Task<IResult<Model.UnstructuredLog>> SaveUnstructuredLogsAsync(
		IScopeContext scopeContext,
		IEnumerable<Model.UnstructuredLog> unstructuredLogs,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.UnstructuredLog>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, unstructuredLogs))
			return result.Build();

		var operationName = nameof(LogPermissions.UnstructuredLog.SaveUnstructuredLog);

		foreach (var unstructuredLog in unstructuredLogs)
		{
			if (checkPermissions)
			{
				if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, unstructuredLog) == false)
					return result.WithUnauthorizedException(scopeContext, null, operationName);
			}

			UoW.UnstructuredLogRepository.Add(scopeContext, unstructuredLog);
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public IResult<Model.UnstructuredLog> SaveUnstructuredLog(
		IScopeContext scopeContext,
		Model.UnstructuredLog unstructuredLog,
		bool checkPermissions)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.UnstructuredLog>();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, unstructuredLog))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.UnstructuredLog.SaveUnstructuredLog);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, unstructuredLog) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.UnstructuredLogRepository.Add(scopeContext, unstructuredLog);

		var saveResult = SaveInternal(scopeContext, force: false);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public IResult<Model.UnstructuredLog> SaveUnstructuredLogs(
		IScopeContext scopeContext,
		IEnumerable<Model.UnstructuredLog> unstructuredLogs,
		bool checkPermissions)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.UnstructuredLog>();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, unstructuredLogs))
			return result.Build();

		var operationName = nameof(LogPermissions.UnstructuredLog.SaveUnstructuredLog);

		foreach (var unstructuredLog in unstructuredLogs)
		{
			if (checkPermissions)
			{
				if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, unstructuredLog) == false)
					return result.WithUnauthorizedException(scopeContext, null, operationName);
			}

			UoW.UnstructuredLogRepository.Add(scopeContext, unstructuredLog);
		}

		var saveResult = SaveInternal(scopeContext, force: false);
		result.MergeHasError(saveResult);
		return result.Build();
	}
}
