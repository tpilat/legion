using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageContent;

public record GetVwMessageContentByIdQuery(
	Guid IdMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwMessageContent>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwMessageContent>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwMessageContent, Model.VwMessageContent?>;
