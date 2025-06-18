using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.BlockedMessageType;

public record GetBlockedMessageTypesByNamespacesQuery(
	List<string> Namespaces,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.BlockedMessageType>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.BlockedMessageType>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<MessageBox.Model.BlockedMessageType, List<MessageBox.Model.BlockedMessageType>>;

