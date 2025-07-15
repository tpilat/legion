namespace Legion.ADF.ServiceBus.Model;

public sealed partial class HostActivity : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	internal static IResult<HostActivity> Create(
		IScopeContext scopeContext,
		Host host)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<HostActivity>();

		if (result.IsArgumentNull(scopeContext, host))
			return result.Build();

		var utcNow = GlobalContext.Instance.UtcNow;
		var id = GlobalContext.Instance.NewGuid();
		var hostActivity = new HostActivity
		{
			__IsNewObject = true,
			IdHostActivity = id,
			Host = host,
			StartedUtc = utcNow,
			LastActivityUtc = utcNow,
			StoppedUtc = null,
			IsDistributedManagerAvailable = false,
			RowVersion = id
		};

		host.AttachActivity(scopeContext, hostActivity);

		var validationResult =
			DefaultDBValidator
				.Validate(hostActivity);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(hostActivity).Build();
	}

	internal IResult SetStart(
		IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsNull(scopeContext, Host, errorCode: null, detail: "Invalid object graph"))
			return result.Build();

		if (!Host.IsEnabled)
			return result.WithInitializationException(scopeContext, null, $"Host {Host.Name}::{IdHost} is disabled");

		StartedUtc = GlobalContext.Instance.UtcNow;
		LastActivityUtc = StartedUtc;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult UpdateLastActivity(
		IScopeContext scopeContext,
		DateTime utcNow,
		bool isAvailableDistributedCache)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsNull(scopeContext, Host, errorCode: null, detail: "Invalid object graph"))
			return result.Build();

		if (!Host.IsEnabled)
			return result.WithInitializationException(scopeContext, null, $"Host {Host.Name}::{IdHost} is disabled");

		LastActivityUtc = utcNow;
		IsDistributedManagerAvailable = isAvailableDistributedCache;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetStop(
		IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsNull(scopeContext, Host, errorCode: null, detail: "Invalid object graph"))
			return result.Build();

		StoppedUtc = GlobalContext.Instance.UtcNow;

		if (!Host.IsEnabled)
			LastActivityUtc = StoppedUtc.Value;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}
}
