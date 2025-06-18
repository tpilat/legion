using Legion.ADF.Config.Events;
using Legion.Extensions;
using Legion.Model.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config.Services;

public partial class ConfigStore : IDisposable, IAsyncDisposable
{
	public async Task<IResult<Model.ConfigurationKeyValue>> SaveConfigKeyValueAsync(
		IScopeContext scopeContext,
		string key,
		string? value,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(key), key);

		var result = new ResultBuilder<Model.ConfigurationKeyValue>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build();

		var dbConfigKeyValue = await UoW.ConfigurationKeyValueRepository
			.GetConfigurationKeyValueByKey(new Queries.ConfigurationKeyValue.GetConfigurationKeyValueByKeyQuery(key, checkPermissions, DisableCahce: true))
			.ToResultAsync(scopeContext, cancellationToken);

		if (checkPermissions)
		{
			var operationName = nameof(ConfigPermissions.ConfigurationKeyValue.SaveConfigKeyValue);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbConfigKeyValue) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		if (dbConfigKeyValue != null)
		{
			var updateResult = dbConfigKeyValue.UpdateValue(scopeContext, value);
			if (result.MergeHasError(updateResult))
				return result.Build();
		}
		else
		{
			var createResult = Model.ConfigurationKeyValue.Create(scopeContext, key, value);
			if (result.MergeHasError(createResult))
				return result.Build();

			dbConfigKeyValue = createResult.Data!;

			UoW.ConfigurationKeyValueRepository.Add(scopeContext, dbConfigKeyValue);
		}

		var saveResult = await SaveInternalAsync(scopeContext, AutoSaveChanges, [typeof(Model.ConfigurationKeyValue).FullName], cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbConfigKeyValue).Build();
	}

	public async Task<IResult<bool>> RemoveConfigKeyValueAsync(
		IScopeContext scopeContext,
		string key,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(key), key);

		var result = new ResultBuilder<bool>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build();

		var dbConfigKeyValue = await UoW.ConfigurationKeyValueRepository
			.GetConfigurationKeyValueByKey(new Queries.ConfigurationKeyValue.GetConfigurationKeyValueByKeyQuery(key, checkPermissions, DisableCahce: true))
			.ToResultAsync(scopeContext, cancellationToken);

		if (checkPermissions)
		{
			var operationName = nameof(ConfigPermissions.ConfigurationKeyValue.RemoveConfigKeyValue);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbConfigKeyValue) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		if (dbConfigKeyValue == null)
			return result.WithData(false).Build();

		UoW.ConfigurationKeyValueRepository
			.Remove(scopeContext, dbConfigKeyValue);

		ConnectionProvider?.DomainEventStore?.AddDomainEvent(
			scopeContext,
			new ConfigKeyRemovedEvent(dbConfigKeyValue.Key),
			this.GetType().GetSimplifiedAssemblyQualifiedName(),
			nameof(RemoveConfigKeyValueAsync));

		var saveResult = await SaveInternalAsync(scopeContext, AutoSaveChanges, [typeof(Model.ConfigurationKeyValue).FullName], cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(true).Build();
	}
}
