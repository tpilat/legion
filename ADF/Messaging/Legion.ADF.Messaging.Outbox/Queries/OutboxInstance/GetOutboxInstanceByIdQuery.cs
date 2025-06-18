using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxInstance;

public record GetOutboxInstanceByIdQuery(
	Guid IdOutboxInstance,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Outbox.Model.OutboxInstance>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.OutboxInstance>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Outbox.Model.OutboxInstance, Outbox.Model.OutboxInstance?>;
