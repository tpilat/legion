using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEventContent;

public record GetDomainEventContentByIdQuery(
	Guid IdDomainEvent,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<DomainEvents.Model.DomainEventContent>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.DomainEventContent>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<DomainEvents.Model.DomainEventContent, DomainEvents.Model.DomainEventContent?>;
