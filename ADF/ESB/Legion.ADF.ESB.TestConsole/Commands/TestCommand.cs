using Legion.ADF.ESB.ComponentsModel;
using Legion.MessageBus.MessageHandlers;
using Legion.MessageBus.Messages;
using Legion.Model.Repositories;

namespace Legion.ADF.ESB.TestConsole.Commands;

public record TestCommand(Guid IdMessage)
	: ICommand;

public class TestCommandHandler : AsyncMessageHandlerBase<TestCommand>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		TestCommand command,
		IUnitOfWorkProvider unitOfWorkProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew()
			.InvocationAddContextProperty(nameof(command.IdMessage), command.IdMessage.ToString());

		var result = new ResultBuilder();

		//var adapterResult = Components.Model.Adapter.Create(invocationContext, "UPVS", "UPVS Client", "desc", typeof(TestCommandHandler), null);
		//if (result.MergeHasError(adapterResult))
		//	return result.Build();

		//var componentsUowResult = unitOfWorkProvider.Create<IComponentsUnitOfWork>(invocationContext);
		//if (result.MergeHasError(componentsUowResult))
		//	return result.Build();

		//var componentsUow = componentsUowResult.Data!;
		
		//componentsUow.AdapterRepository
		//	.Add(invocationContext, adapterResult.Data!);

		//var saveResult = await componentsUow.SaveAsync(invocationContext, cancellationToken);

		return result.Build();
	}
}
