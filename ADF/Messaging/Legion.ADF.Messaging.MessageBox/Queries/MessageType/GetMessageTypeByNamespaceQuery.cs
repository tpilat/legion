using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.MessageType;

public record GetMessageTypeByNamespaceQuery(
	string Namespace,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.MessageType>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.MessageType>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<MessageBox.Model.MessageType, MessageBox.Model.MessageType?>;
