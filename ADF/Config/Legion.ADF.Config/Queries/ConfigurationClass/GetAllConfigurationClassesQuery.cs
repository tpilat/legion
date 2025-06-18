using Legion.MessageBus.Messages;

namespace Legion.ADF.Config.Queries.ConfigurationClass;

public record GetAllConfigurationClassesQuery(
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = false,
	Action<Legion.Queries.IQueryableBuilder<Config.Model.ConfigurationClass>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.ConfigurationClass>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Config.Model.ConfigurationClass, List<Config.Model.ConfigurationClass>>;
