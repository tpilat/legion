namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageType : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<MessageType?> Create(
		IScopeContext scopeContext,
		string code,
		string name,
		string @namespace)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageType?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, @namespace))
			return result.Build();

		var id = Guid.NewGuid();
		var messageType = new MessageType
		{
			__IsNewObject = true,
			IdMessageType = id,
			Code = code,
			Name = name,
			Namespace = @namespace,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		messageType.TrimStringValuesToFitDatabaseMaxLengths();
		messageType.Namespace = @namespace; //do not trim the namespace

		var validationResult =
			DefaultDBValidator
				.Validate(messageType);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageType).Build();
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
		IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		result.MergeHasError(scopeContext, validationResult, true);
		return result.Build();
	}
}
