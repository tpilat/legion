using Legion.Infrastructure;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class BlockedInboxMessageType : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<BlockedInboxMessageType?> Create(
		IScopeContext scopeContext,
		string blockedNamespace)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<BlockedInboxMessageType?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, blockedNamespace))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var dbBlockedInboxMessageType = new BlockedInboxMessageType
		{
			__IsNewObject = true,
			IdBlockedInboxMessageType = id,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			Namespace = blockedNamespace,
			IdInboxInstance = EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(dbBlockedInboxMessageType);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(dbBlockedInboxMessageType).Build();
	}
}
