namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class BlockedDomainEventType : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<BlockedDomainEventType?> Create(
		IScopeContext scopeContext,
		string blockedNamespace)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<BlockedDomainEventType?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, blockedNamespace))
			return result.Build();

		var id = Guid.NewGuid();
		var dbBlockedDomainEventType = new BlockedDomainEventType
		{
			__IsNewObject = true,
			IdBlockedDomainEventType = id,
			Namespace = blockedNamespace
		};

		var validationResult =
			DefaultDBValidator
				.Validate(dbBlockedDomainEventType);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(dbBlockedDomainEventType).Build();
	}
}
