namespace Legion.ADF.Logs.Services;

public partial class LogsStore : IDisposable, IAsyncDisposable
{
	public async Task<IResult<Model.EnvironmentInfo>> SaveEnvironmentInfoAsync(
		IScopeContext scopeContext,
		string applicationName,
		string? appVersion,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Legion.Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY), Legion.Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY.ToString());

		var result = new ResultBuilder<Model.EnvironmentInfo>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, applicationName))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(LogPermissions.EnvironmentInfo.SaveEnvironmentInfo);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, (Model.EnvironmentInfo?)null) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		var environmentInfo = await UoW.EnvironmentInfoRepository
			.GetEnvironmentInfoById(new Queries.EnvironmentInfo.GetEnvironmentInfoByIdQuery(Legion.Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY, checkPermissions, AsNoTracking: false))
			.ToResultAsync(scopeContext, cancellationToken);

		if (environmentInfo != null)
			return
				result
					.WithData(environmentInfo)
					.WithWarning(scopeContext, null, x => x.InternalMessage("EnvironmentInfo already exists."));

		var createResult = Model.EnvironmentInfo.CreateEnvironmentInfo(scopeContext, applicationName, appVersion);
		if (result.MergeHasError(createResult))
			return result.Build();

		UoW.EnvironmentInfoRepository.Add(scopeContext, createResult.Data!);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(createResult.Data).Build();
	}
}
