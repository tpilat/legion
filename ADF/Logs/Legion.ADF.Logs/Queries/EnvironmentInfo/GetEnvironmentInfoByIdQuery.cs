using Legion.MessageBus.Messages;

namespace Legion.ADF.Logs.Queries.EnvironmentInfo;

public record GetEnvironmentInfoByIdQuery(
	Guid IdEnvironmentInfo,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.EnvironmentInfo>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.EnvironmentInfo>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.EnvironmentInfo, Model.EnvironmentInfo?>;
