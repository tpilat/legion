using Legion.MessageBus.Messages;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive;

public record GetVwInboxMessageArchiveByIdMessageQuery(
	Guid IdInboxMessage,
	bool CheckReadPermissions,
	bool AsNoTracking = true,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.VwInboxMessageArchive>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.VwInboxMessageArchive>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.VwInboxMessageArchive, Model.VwInboxMessageArchive?>;
