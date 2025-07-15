namespace Legion.ADF.Cache.Model;

public sealed partial class ReloadableCacheKey : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	internal static IResult<ReloadableCacheKey?> Create(
		IScopeContext scopeContext,
		string? key,
		List<string>? tags,
		DateTime? reloadAtUtc)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ReloadableCacheKey?>();

		if (string.IsNullOrWhiteSpace(key) && (tags == null || tags.Count == 0))
		{
			result.IsArgumentNullOrWhiteSpace(scopeContext, key);
			result.IsArgumentNullOrEmpty(scopeContext, tags);
			return result.Build();
		}

		var nowUtc = GlobalContext.Instance.UtcNow;
		var id = GlobalContext.Instance.NewGuid();
		var reloadableCacheKey = new ReloadableCacheKey
		{
			__IsNewObject = true,
			IdReloadableCacheKey = id,
			Key = key,
			Tags = tags?.OrderBy(x => x).ToList(), //order + clone!
			CreatedUtc = nowUtc,
			ReloadAtUtc = reloadAtUtc ?? nowUtc.AddYears(-1)
		};

		var validationResult =
			DefaultDBValidator
				.Validate(reloadableCacheKey);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(reloadableCacheKey).Build();
	}

	internal IResult UpdateReloadAtUtc(
		IScopeContext scopeContext,
		DateTime? reloadAtUtc)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		ReloadAtUtc = reloadAtUtc ?? GlobalContext.Instance.UtcNow.AddYears(-1);

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}
}
