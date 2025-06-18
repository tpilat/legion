namespace Legion.ADF.Logs.Services;

public partial class LogsStore : IDisposable, IAsyncDisposable
{
	public async Task<IResult<Model.EventCounterData>> SaveEventCounterDataAsync(
		IScopeContext scopeContext,
		Model.EventCounterData eventCounterData,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(eventCounterData.IdEventCounterData), eventCounterData?.IdEventCounterData.ToString())
			.AddContextProperty(nameof(eventCounterData.IdEventCounter), eventCounterData?.IdEventCounter.ToString());

		var result = new ResultBuilder<Model.EventCounterData>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, eventCounterData))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.EventCounterData.SaveEventCounterData);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, eventCounterData) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.EventCounterDataRepository.Add(scopeContext, eventCounterData);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}
}
