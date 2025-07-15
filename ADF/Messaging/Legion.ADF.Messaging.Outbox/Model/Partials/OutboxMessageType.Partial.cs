namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageType : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<OutboxMessageType?> Create(
		IScopeContext scopeContext,
		string code,
		string name,
		string @namespace)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageType?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, @namespace))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var outboxMessageType = new OutboxMessageType
		{
			__IsNewObject = true,
			IdOutboxMessageType = id,
			Code = code,
			Name = name,
			Namespace = @namespace,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdOutboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		outboxMessageType.TrimStringValuesToFitDatabaseMaxLengths();
		outboxMessageType.Namespace = @namespace; //do not trim the namespace

		var validationResult =
			DefaultDBValidator
				.Validate(outboxMessageType);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxMessageType).Build();
	}

	internal IResult Update(
		IScopeContext scopeContext,
		string code,
		string name)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		Code = code;
		Name = name;
		IdOutboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		result.MergeHasError(scopeContext, validationResult, true);
		return result.Build();
	}
}
