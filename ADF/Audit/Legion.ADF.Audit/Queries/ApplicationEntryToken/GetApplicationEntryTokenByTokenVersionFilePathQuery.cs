using Legion.MessageBus.Messages;

namespace Legion.ADF.Audit.Queries.ApplicationEntryToken;

public record GetApplicationEntryTokenByTokenVersionFilePathQuery(
	string Token,
	string SourceFilePath,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.ApplicationEntryToken>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.ApplicationEntryToken>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.ApplicationEntryToken, Model.ApplicationEntryToken?>;
