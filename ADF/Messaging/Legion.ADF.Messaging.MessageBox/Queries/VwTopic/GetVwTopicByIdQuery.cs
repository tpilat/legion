using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwTopic;

public record GetVwTopicByIdQuery(
	Guid IdTopic,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwTopic>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwTopic>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwTopic, Model.VwTopic?>;
