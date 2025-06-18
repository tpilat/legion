namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageType : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<InboxMessageType?> Create(
		IScopeContext scopeContext,
		string code,
		string name,
		string @namespace)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageType?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, @namespace))
			return result.Build();

		var id = Guid.NewGuid();
		var inboxMessageType = new InboxMessageType
		{
			__IsNewObject = true,
			IdInboxMessageType = id,
			Code = code,
			Name = name,
			Namespace = @namespace,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdInboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		inboxMessageType.TrimStringValuesToFitDatabaseMaxLengths();
		inboxMessageType.Namespace = @namespace; //do not trim the namespace

		var validationResult =
			DefaultDBValidator
				.Validate(inboxMessageType);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxMessageType).Build();
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
		IdInboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		result.MergeHasError(scopeContext, validationResult, true);
		return result.Build();
	}
}
