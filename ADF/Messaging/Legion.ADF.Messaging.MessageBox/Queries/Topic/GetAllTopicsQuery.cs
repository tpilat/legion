using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.Topic;

public record GetAllTopicsQuery(
	bool IncludeInactiveTopics,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.Topic>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Topic>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<MessageBox.Model.Topic, List<MessageBox.Model.Topic>>;
