namespace Legion.ADF.Config.Services;

public partial class ConfigStore : IDisposable, IAsyncDisposable
{
	public async Task<IResult<Model.ConfigurationClass>> SaveConfigurationClassAsync(
		IScopeContext scopeContext,
		string rootPath,
		string displayName,
		string? csharpClassTypeToDeserialize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(rootPath), rootPath);

		var result = new ResultBuilder<Model.ConfigurationClass>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, rootPath))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, displayName))
			return result.Build();

		var dbConfiguration = await UoW.ConfigurationClassRepository
			.GetConfigurationClassByRootPath(new Queries.ConfigurationClass.GetConfigurationClassByRootPathQuery(rootPath, checkPermissions, DisableCahce: true))
			.ToResultAsync(scopeContext, cancellationToken);

		if (checkPermissions)
		{
			var operationName = nameof(ConfigPermissions.ConfigurationClass.SaveConfigurationClass);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbConfiguration) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		if (dbConfiguration != null)
		{
			var updateResult = dbConfiguration.UpdateValue(scopeContext, displayName, csharpClassTypeToDeserialize);
			if (result.MergeHasError(updateResult))
				return result.Build();
		}
		else
		{
			var createResult = Model.ConfigurationClass.Create(scopeContext, rootPath, displayName, csharpClassTypeToDeserialize);
			if (result.MergeHasError(createResult))
				return result.Build();

			dbConfiguration = createResult.Data!;

			UoW.ConfigurationClassRepository.Add(scopeContext, dbConfiguration);
		}

		var saveResult = await SaveInternalAsync(scopeContext, AutoSaveChanges, [typeof(Model.ConfigurationClass).FullName], cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbConfiguration).Build();
	}

	public async Task<IResult<bool>> RemoveConfigurationAsync(
		IScopeContext scopeContext,
		string rootPath,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(rootPath), rootPath);

		var result = new ResultBuilder<bool>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, rootPath))
			return result.Build();

		var dbConfiguration = await UoW.ConfigurationClassRepository
			.GetConfigurationClassByRootPath(new Queries.ConfigurationClass.GetConfigurationClassByRootPathQuery(rootPath, checkPermissions, DisableCahce: true))
			.ToResultAsync(scopeContext, cancellationToken);

		if (checkPermissions)
		{
			var operationName = nameof(ConfigPermissions.ConfigurationClass.RemoveConfiguration);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbConfiguration) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		if (dbConfiguration == null)
			return result.WithData(false).Build();

		UoW.ConfigurationClassRepository
			.Remove(scopeContext, dbConfiguration);

		var saveResult = await SaveInternalAsync(scopeContext, AutoSaveChanges, [typeof(Model.ConfigurationClass).FullName], cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(true).Build();
	}
}
