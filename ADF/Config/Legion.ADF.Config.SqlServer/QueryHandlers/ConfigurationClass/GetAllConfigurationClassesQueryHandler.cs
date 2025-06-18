using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Config.QueryHandlers.ConfigurationClass;

public class GetAllConfigurationClassesQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Config.Queries.ConfigurationClass.GetAllConfigurationClassesQuery, System.Collections.Generic.List<Legion.ADF.Config.Model.ConfigurationClass>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Config.Model.ConfigurationClass>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Config.Queries.ConfigurationClass.GetAllConfigurationClassesQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Config.Model.ConfigurationClass>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IConfigUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.ConfigurationClassRepository.GetAllConfigurationClasses(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
