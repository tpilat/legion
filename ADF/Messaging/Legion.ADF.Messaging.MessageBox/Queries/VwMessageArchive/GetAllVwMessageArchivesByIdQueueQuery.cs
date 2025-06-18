using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive;

public record GetAllVwMessageArchivesByIdQueueQuery(
	Guid IdQueue,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwMessageArchive>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwMessageArchive>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwMessageArchive, List<Model.VwMessageArchive>>;
