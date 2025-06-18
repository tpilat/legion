using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.DomainEvents.Queries.BlockedDomainEventType;

public record GetAllBlockedDomainEventTypesQuery(
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<DomainEvents.Model.BlockedDomainEventType>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.BlockedDomainEventType>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<DomainEvents.Model.BlockedDomainEventType, List<DomainEvents.Model.BlockedDomainEventType>>;
