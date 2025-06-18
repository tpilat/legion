using Legion.Model;
using Legion.Serializer;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventContent : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<DomainEventContent?> Create(
		IScopeContext scopeContext,
		IDomainEvent domainEventContent)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DomainEventContent?>();

		if (result.IsArgumentNull(scopeContext, domainEventContent))
			return result.Build();

		var nowUtc = GlobalContext.Instance.UtcNow;
		var dbDomainEventContent = new DomainEventContent
		{
			__IsNewObject = true,
			IdDomainEventContent = domainEventContent.Id,
			Content = JsonSerializerHelper.Serialize(domainEventContent)
		};

		var validationResult =
			DefaultDBValidator
				.Validate(dbDomainEventContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(dbDomainEventContent).Build();
	}
}
