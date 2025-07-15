using Legion.Infrastructure;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class BlockedOutboxMessageType : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<BlockedOutboxMessageType?> Create(
		IScopeContext scopeContext,
		string blockedNamespace)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<BlockedOutboxMessageType?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, blockedNamespace))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var dbBlockedOutboxMessageType = new BlockedOutboxMessageType
		{
			__IsNewObject = true,
			IdBlockedOutboxMessageType = id,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			Namespace = blockedNamespace,
			IdOutboxInstance = EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(dbBlockedOutboxMessageType);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(dbBlockedOutboxMessageType).Build();
	}
}
