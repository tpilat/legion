using Legion.Serializer;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class Host : ServiceBusBaseEntity, Legion.Model.IEntity
{
	[NotMapped]
	internal List<Job>? DefaultJobs { get; set; }

	[NotMapped]
	internal List<Job>? RunningOwnJobs { get; set; }

	[NotMapped]
	internal List<Job>? RunningForeignJobs { get; set; }

	internal static IResult<Host> Create(
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
		var id = GlobalContext.Instance.NewGuid();
		var host = new Host
		{
			__IsNewObject = true,
			IdHost = id,
			Name = name,
			Description = description,
			CreatedUtc = utcNow,
			IsEnabled = isEnabled
		};

		var validationResult =
			DefaultDBValidator
				.Validate(host);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(host).Build();
	}

	internal IResult AttachActivity(
		IScopeContext scopeContext,
		HostActivity hostActivity)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNull(scopeContext, hostActivity))
			return result.Build();

		HostActivity = hostActivity;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetConfiguration(
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

	internal IResult<DTOs.Hosts.HostConfigurationDto> GetHostConfiguration(IScopeContext scopeContext)
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
			return result.WithInvalidOperationException(scopeContext, Exceptions.Internal.ErrorCodes.ServiceBusHostException.InvalidHostConfig(Name), detail: null, ex);
		}
	}

	internal DTOs.Hosts.HostDto ToDto(
		IScopeContext scopeContext,
		ILogger logger)
	{
		var cfgResult = GetHostConfiguration(scopeContext);
		logger.LogResultErrorMessages(
			scopeContext,
			Exceptions.Internal.ErrorCodes.ServiceBusHostException.InvalidHostConfig(Name),
			cfgResult,
			dataMustBeNotNull: true,
			skipIfAlreadyLogged: true,
			logWarnings: true);

		var cfg = cfgResult.Data;

		var dto = new DTOs.Hosts.HostDto
		{
			IdHost = IdHost,
			Name = Name,
			Description = Description,
			IsEnabled = IsEnabled,
			LastActivityAt = HostActivity?.LastActivityUtc.ToLocalTime(),
			IsDistributedManagerAvailable = HostActivity?.IsDistributedManagerAvailable ?? false,
			IsAvailable =
				HostActivity != null
				&& cfg != null
				&& GlobalContext.Instance.UtcNow <= HostActivity.LastActivityUtc.AddSeconds(cfg.HeartbeatInSeconds + Services.Internal.Dto.HostContext._heartbeatDelayDeltaInSeconds)
		};

		return dto;
	}

	internal DTOs.Hosts.HostDetailDto ToDetailDto(
		IScopeContext scopeContext,
		ILogger logger)
	{
		var cfgResult = GetHostConfiguration(scopeContext);
		logger.LogResultErrorMessages(
			scopeContext,
			Exceptions.Internal.ErrorCodes.ServiceBusHostException.InvalidHostConfig(Name),
			cfgResult,
			dataMustBeNotNull: true,
			skipIfAlreadyLogged: true,
			logWarnings: true);

		var cfg = cfgResult.Data;

		var dto = new DTOs.Hosts.HostDetailDto
		{
			IdHost = IdHost,
			Name = Name,
			Description = Description,
			IsEnabled = IsEnabled,
			StartedAt = HostActivity?.StartedUtc.ToLocalTime(),
			Configuration = Configuration,
			LastActivityAt = HostActivity?.LastActivityUtc.ToLocalTime(),
			IsDistributedManagerAvailable = HostActivity?.IsDistributedManagerAvailable ?? false,
			IsAvailable =
				HostActivity != null
				&& cfg != null
				&& GlobalContext.Instance.UtcNow <= HostActivity.LastActivityUtc.AddSeconds(cfg.HeartbeatInSeconds + Services.Internal.Dto.HostContext._heartbeatDelayDeltaInSeconds)
		};

		return dto;
	}

	internal string GetDistributedCacheKey(
		string systemName,
		string? operation)
		=> GetHostDistributedCacheKey(systemName, Name, operation);

	public static string GetHostDistributedCacheKey(
		string systemName,
		string hostName,
		string? operation)
		=> string.IsNullOrEmpty(operation)
			? $"{systemName}:Legion.ADF.ServiceBus.Host:{hostName}"
			: $"{systemName}:Legion.ADF.ServiceBus.Host:{hostName}:{operation}";
}
