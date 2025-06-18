using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Config.QueryHandlers.ConfigurationKeyValue;

public class GetConfigurationKeyValueByKeyQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Config.Queries.ConfigurationKeyValue.GetConfigurationKeyValueByKeyQuery, Legion.ADF.Config.Model.ConfigurationKeyValue?>
{
	public override async Task<IResult<Legion.ADF.Config.Model.ConfigurationKeyValue?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Config.Queries.ConfigurationKeyValue.GetConfigurationKeyValueByKeyQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Config.Model.ConfigurationKeyValue?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IConfigUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.ConfigurationKeyValueRepository.GetConfigurationKeyValueByKey(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
