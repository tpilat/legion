using Legion.ADF.Config.Events;

namespace Legion.ADF.Config.Model;

public sealed partial class ConfigurationKeyValue : Config.ConfigBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	internal static IResult<ConfigurationKeyValue?> Create(
		IScopeContext scopeContext,
		string key,
		string? value)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ConfigurationKeyValue?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build();

		var id = Guid.NewGuid();
		var configurationKeyValue = new ConfigurationKeyValue
		{
			__IsNewObject = true,
			IdConfigurationKeyValue = id,
			Key = key,
			Value = value,
			AuditCreatedUtc = GlobalContext.Instance.UtcNow,
			AuditModifiedUtc = null,
			IdAuditCreatedBy = scopeContext.IdUser,
			IdAuditModifiedBy = null,
			ConcurrencyToken = id
		};

		configurationKeyValue.RaiseDomainEventOnCommit(new ConfigKeyCreatedEvent(configurationKeyValue.Key));

		var validationResult =
			DefaultDBValidator
				.Validate(configurationKeyValue);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(configurationKeyValue).Build();
	}

	internal IResult UpdateValue(
		IScopeContext scopeContext,
		string? value)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();
		
		Value = value;
		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = Guid.NewGuid();
		}

		RaiseDomainEventOnCommit(new ConfigValueChangedEvent(Key));

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}
}
