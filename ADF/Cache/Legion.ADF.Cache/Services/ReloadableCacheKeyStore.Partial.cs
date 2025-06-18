using Legion.Caching;

namespace Legion.ADF.Cache.Services;

public partial class ReloadableCacheKeyStore : IReloadableCacheKeyStore, IDisposable, IAsyncDisposable
{
	public async Task<IResult<List<Model.ReloadableCacheKey>>> GetAllReloadableCacheKeyAsync(
		IScopeContext scopeContext,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<Model.ReloadableCacheKey>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await UoW.ReloadableCacheKeyRepository
			.GetAllReloadableCacheKeysByReloadAt(new Queries.ReloadableCacheKey.GetAllReloadableCacheKeysByReloadAtQuery(GlobalContext.Instance.UtcNow, checkPermissions, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	internal async Task<IResult<List<Model.ReloadableCacheKey>>> GetAllReloadableCacheKeyInternalAsync(
		IScopeContext scopeContext,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<Model.ReloadableCacheKey>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await UoW.ReloadableCacheKeyRepository
			.GetAllReloadableCacheKeysByReloadAt(new Queries.ReloadableCacheKey.GetAllReloadableCacheKeysByReloadAtQuery(GlobalContext.Instance.UtcNow, checkPermissions))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Model.ReloadableCacheKey>> SaveReloadableCacheKeyAsync(
		IScopeContext scopeContext,
		string? key,
		List<string>? tags,
		DateTime? reloadAtUtc,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(key), key)
			.AddContextProperty(nameof(tags), tags == null ? null : string.Join(Environment.NewLine, tags));

		var result = new ResultBuilder<Model.ReloadableCacheKey>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (string.IsNullOrWhiteSpace(key) && (tags == null || tags.Count == 0))
		{
			result.IsArgumentNullOrWhiteSpace(scopeContext, key);
			result.IsArgumentNullOrEmpty(scopeContext, tags);
			return result.Build();
		}

		if (!string.IsNullOrWhiteSpace(key) && (0 < tags?.Count))
		{
			result.WithArgumentException(scopeContext, key, errorCode: null, $"{nameof(key)} != null && {nameof(tags)} != null");
			result.WithArgumentException(scopeContext, tags, errorCode: null, $"{nameof(key)} != null && {nameof(tags)} != null");
			return result.Build();
		}

		Legion.ADF.Cache.Model.ReloadableCacheKey? dbReloadableCacheKey;

		if (string.IsNullOrWhiteSpace(key))
		{
			dbReloadableCacheKey = await UoW.ReloadableCacheKeyRepository
				.GetReloadableCacheKeyByTags(new Queries.ReloadableCacheKey.GetReloadableCacheKeyByTagsQuery(tags!, checkPermissions))
				.ToResultAsync(scopeContext, cancellationToken);
		}
		else
		{
			dbReloadableCacheKey = await UoW.ReloadableCacheKeyRepository
				.GetReloadableCacheKeyByKey(new Queries.ReloadableCacheKey.GetReloadableCacheKeyByKeyQuery(key, checkPermissions))
				.ToResultAsync(scopeContext, cancellationToken);
		}

		if (checkPermissions)
		{
			var operationName = nameof(CachePermissions.ReloadableCacheKey.SaveReloadableCacheKey);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbReloadableCacheKey) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		if (dbReloadableCacheKey != null)
		{
			var updateResult = dbReloadableCacheKey.UpdateReloadAtUtc(scopeContext, reloadAtUtc);
			if (result.MergeHasError(updateResult))
				return result.Build();
		}
		else
		{
			var createResult = Model.ReloadableCacheKey.Create(scopeContext, key, tags, reloadAtUtc);
			if (result.MergeHasError(createResult))
				return result.Build();

			dbReloadableCacheKey = createResult.Data!;

			UoW.ReloadableCacheKeyRepository.Add(scopeContext, dbReloadableCacheKey);
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbReloadableCacheKey).Build();
	}

	async Task<IResult> IReloadableCacheKeyStore.SaveReloadableCacheKeyAsync(
		IScopeContext scopeContext,
		string? key,
		List<string>? tags,
		DateTime? reloadAtUtc,
		bool checkPermissions,
		CancellationToken cancellationToken)
		=> await SaveReloadableCacheKeyAsync(scopeContext, key, tags, reloadAtUtc, checkPermissions, cancellationToken);

	public IResult<Model.ReloadableCacheKey> SaveReloadableCacheKey(
		IScopeContext scopeContext,
		string? key,
		List<string>? tags,
		DateTime? reloadAtUtc,
		bool checkPermissions)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(key), key)
			.AddContextProperty(nameof(tags), tags == null ? null : string.Join(Environment.NewLine, tags));

		var result = new ResultBuilder<Model.ReloadableCacheKey>();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (string.IsNullOrWhiteSpace(key) && (tags == null || tags.Count == 0))
		{
			result.IsArgumentNullOrWhiteSpace(scopeContext, key);
			result.IsArgumentNullOrEmpty(scopeContext, tags);
			return result.Build();
		}

		if (!string.IsNullOrWhiteSpace(key) && (0 < tags?.Count))
		{
			result.WithArgumentException(scopeContext, key, errorCode: null, $"{nameof(key)} != null && {nameof(tags)} != null");
			result.WithArgumentException(scopeContext, tags, errorCode: null, $"{nameof(key)} != null && {nameof(tags)} != null");
			return result.Build();
		}

		Legion.ADF.Cache.Model.ReloadableCacheKey? dbReloadableCacheKey;

		if (string.IsNullOrWhiteSpace(key))
		{
			dbReloadableCacheKey = UoW.ReloadableCacheKeyRepository
				.GetReloadableCacheKeyByTags(new Queries.ReloadableCacheKey.GetReloadableCacheKeyByTagsQuery(tags!, checkPermissions))
				.ToResult(scopeContext);
		}
		else
		{
			dbReloadableCacheKey = UoW.ReloadableCacheKeyRepository
				.GetReloadableCacheKeyByKey(new Queries.ReloadableCacheKey.GetReloadableCacheKeyByKeyQuery(key, checkPermissions))
				.ToResult(scopeContext);
		}

		if (checkPermissions)
		{
			var operationName = nameof(CachePermissions.ReloadableCacheKey.SaveReloadableCacheKey);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbReloadableCacheKey) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		if (dbReloadableCacheKey != null)
		{
			var updateResult = dbReloadableCacheKey.UpdateReloadAtUtc(scopeContext, reloadAtUtc);
			if (result.MergeHasError(updateResult))
				return result.Build();
		}
		else
		{
			var createResult = Model.ReloadableCacheKey.Create(scopeContext, key, tags, reloadAtUtc);
			if (result.MergeHasError(createResult))
				return result.Build();

			dbReloadableCacheKey = createResult.Data!;

			UoW.ReloadableCacheKeyRepository.Add(scopeContext, dbReloadableCacheKey);
		}

		var saveResult = SaveInternal(scopeContext, force: false);
		result.MergeHasError(saveResult);
		return result.WithData(dbReloadableCacheKey).Build();
	}

	IResult IReloadableCacheKeyStore.SaveReloadableCacheKey(
		IScopeContext scopeContext,
		string? key,
		List<string>? tags,
		DateTime? reloadAtUtc,
		bool checkPermissions)
		=> SaveReloadableCacheKey(scopeContext, key, tags, reloadAtUtc, checkPermissions);

	public async Task<IResult<bool>> RemoveReloadableCacheKeyAsync(
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

		var dbReloadableCacheKey = await UoW.ReloadableCacheKeyRepository
			.GetReloadableCacheKeyByKey(new Queries.ReloadableCacheKey.GetReloadableCacheKeyByKeyQuery(key, checkPermissions))
			.ToResultAsync(scopeContext, cancellationToken);

		if (checkPermissions)
		{
			var operationName = nameof(CachePermissions.ReloadableCacheKey.RemoveReloadableCacheKey);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbReloadableCacheKey) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		if (dbReloadableCacheKey == null)
			return result.WithData(false).Build();

		UoW.ReloadableCacheKeyRepository
			.Remove(scopeContext, dbReloadableCacheKey);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(true).Build();
	}

	public async Task<IResult<bool>> RemoveReloadableCacheKeyAsync(
		IScopeContext scopeContext,
		List<string> tags,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(tags), tags == null ? null : string.Join(Environment.NewLine, tags));

		var result = new ResultBuilder<bool>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNullOrEmpty(scopeContext, tags))
			return result.Build();

		var dbReloadableCacheKey = await UoW.ReloadableCacheKeyRepository
			.GetReloadableCacheKeyByTags(new Queries.ReloadableCacheKey.GetReloadableCacheKeyByTagsQuery(tags, checkPermissions))
			.ToResultAsync(scopeContext, cancellationToken);

		if (checkPermissions)
		{
			var operationName = nameof(CachePermissions.ReloadableCacheKey.RemoveReloadableCacheKey);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbReloadableCacheKey) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		if (dbReloadableCacheKey == null)
			return result.WithData(false).Build();

		UoW.ReloadableCacheKeyRepository
			.Remove(scopeContext, dbReloadableCacheKey);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(true).Build();
	}

	internal void RemoveReloadableCacheKey(
		IScopeContext scopeContext,
		Model.ReloadableCacheKey reloadableCacheKey)
	{
		UoW.ReloadableCacheKeyRepository
			.Remove(scopeContext, reloadableCacheKey);
	}
}
