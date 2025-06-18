using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance;

public record ExistsMessageBoxInstanceByIdQuery(
	Guid IdMessageBoxInstance,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<MessageBox.Model.MessageBoxInstance>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.MessageBoxInstance>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<MessageBox.Model.MessageBoxInstance, bool>;
