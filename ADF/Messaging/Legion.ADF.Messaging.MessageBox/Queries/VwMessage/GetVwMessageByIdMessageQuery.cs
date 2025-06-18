using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessage;

public record GetVwMessageByIdMessageQuery(
	Guid IdMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwMessage>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwMessage>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwMessage, Model.VwMessage?>;
