using Legion.Serializer;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class Host : ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static IResult<Host> Create(
		IScopeContext scopeContext,
		string name,
		string description,
		bool isEnabled)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Host>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, description))
			return result.Build();

		var utcNow = GlobalContext.Instance.UtcNow;
		var id = Guid.NewGuid();
		var host = new Host
		{
			__IsNewObject = true,
			IdHost = id,
			Name = name,
			Description = description,
			CreatedUtc = utcNow,
			IsEnabled = isEnabled,
			LastActivityUtc = utcNow
		};

		var validationResult =
			DefaultDBValidator
				.Validate(host);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(host).Build();
	}

	public IResult SetStart(
		IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (!IsEnabled)
			return result.WithInitializationException(scopeContext, null, $"Host {Name}::{IdHost} is disabled");

		StartedUtc = GlobalContext.Instance.UtcNow;
		LastActivityUtc = StartedUtc.Value;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	public IResult UpdateLastActivity(
		IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (!IsEnabled)
			return result.WithInitializationException(scopeContext, null, $"Host {Name}::{IdHost} is disabled");

		LastActivityUtc = GlobalContext.Instance.UtcNow;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	public IResult SetStop(
		IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		StoppedUtc = GlobalContext.Instance.UtcNow;

		if (!IsEnabled)
			LastActivityUtc = StoppedUtc.Value;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	public IResult SetConfiguration(
		IScopeContext scopeContext,
		DTOs.Hosts.HostConfigurationDto hostConfiguration)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNull(scopeContext, hostConfiguration))
			return result.Build();

		var configValidationResult =
			DTOs.Hosts.HostConfigurationDto.DefaultValidator.Value
				.Validate(hostConfiguration);

		if (result.MergeHasError(scopeContext, configValidationResult, true))
			return result.Build();

		Configuration = JsonSerializerHelper.Serialize(hostConfiguration);

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	public IResult<DTOs.Hosts.HostConfigurationDto> GetHostConfiguration(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DTOs.Hosts.HostConfigurationDto>();

		if (string.IsNullOrWhiteSpace(Configuration))
			return result.WithData(null).Build();

		try
		{
			var hostConfigurationDto = JsonSerializerHelper.Deserialize<DTOs.Hosts.HostConfigurationDto>(Configuration);

			if (result.IsNull(scopeContext, hostConfigurationDto))
				return result.Build();

			return result.WithData(hostConfigurationDto).Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex);
		}
	}
}
