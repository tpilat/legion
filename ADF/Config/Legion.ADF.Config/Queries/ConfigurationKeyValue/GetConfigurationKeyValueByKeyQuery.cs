using Legion.MessageBus.Messages;

namespace Legion.ADF.Config.Queries.ConfigurationKeyValue;

public record GetConfigurationKeyValueByKeyQuery(
	string Key,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = false,
	Action<Legion.Queries.IQueryableBuilder<Config.Model.ConfigurationKeyValue>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.ConfigurationKeyValue>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Config.Model.ConfigurationKeyValue, Config.Model.ConfigurationKeyValue?>;
