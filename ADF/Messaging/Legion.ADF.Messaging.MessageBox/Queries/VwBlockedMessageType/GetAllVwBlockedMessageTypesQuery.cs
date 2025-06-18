using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwBlockedMessageType;

public record GetAllVwBlockedMessageTypesQuery(
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.VwBlockedMessageType>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwBlockedMessageType>(AsNoTracking, DisableCahce: true, QueryableBuilder),
		IQueryRequest<MessageBox.Model.VwBlockedMessageType, List<MessageBox.Model.VwBlockedMessageType>>;

