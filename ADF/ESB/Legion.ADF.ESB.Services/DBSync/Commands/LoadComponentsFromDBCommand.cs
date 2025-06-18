using Legion.ADF.ESB.ComponentsModel;
using Legion.MessageBus.MessageHandlers;
using Legion.MessageBus.Messages;
using Legion.Model.Repositories;

namespace Legion.ADF.ESB.Services.DBSync.Commands;

public record LoadComponentsFromDBCommand
	: ICommand;

public class LoadComponentsFromDBCommandHandler : AsyncMessageHandlerBase<LoadComponentsFromDBCommand>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		LoadComponentsFromDBCommand command,
		IUnitOfWorkProvider unitOfWorkProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew()
			.InvocationAppendTraceFrameWithComponent(nameof(LoadComponentsFromDBCommandHandler), false);

		var result = new ResultBuilder();

		var componentsUowResult = unitOfWorkProvider.Create<IComponentsUnitOfWork>(invocationContext);
		if (result.MergeHasError(componentsUowResult))
			return result.Build();

		var componentsUoW = componentsUowResult.Data!;

		//var getAllActiveAdaptersQuery = new GetAllActiveAdaptersQuery();

		var saveResult = await componentsUoW.SaveAsync(invocationContext, cancellationToken);
		if (result.MergeHasError(saveResult))
			return result.Build();

		return result.Build();
	}
}
