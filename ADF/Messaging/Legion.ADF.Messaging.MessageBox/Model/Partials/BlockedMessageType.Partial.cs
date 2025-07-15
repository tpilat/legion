namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class BlockedMessageType : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<BlockedMessageType?> Create(
		IScopeContext scopeContext,
		string blockedNamespace)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<BlockedMessageType?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, blockedNamespace))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var dbBlockedMessageType = new BlockedMessageType
		{
			__IsNewObject = true,
			IdBlockedMessageType = id,
			Namespace = blockedNamespace,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(dbBlockedMessageType);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(dbBlockedMessageType).Build();
	}
}
