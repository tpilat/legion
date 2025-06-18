using Legion;
using Legion.ADF.ESB.Components;
using Legion.ADF.ESB.Orchestrations;
using Legion.Model.Repositories;
using TestEnterpriseServiceBus.Orchestrations.SocPoist.Messages;

namespace TestEnterpriseServiceBus.Orchestrations.SocPoist;

public class SocPoistOrchestration_CallRpo : ESBOrchestrationStep,
	IEventSubscriber<SocPoistOrchestrationStart>
{
	public SocPoistOrchestration_CallRpo()
	{
		IdOrchestrationStep = Guid.NewGuid();
		IsMainEntry = true;
		Code = "SocPoistOrchestration_CallRpo";
		Name = "SocPoistOrchestration_CallRpo";
		Description = "SocPoistOrchestration_CallRpo";
		Properties = null;
		Order = 1;
	}

	public override ESBOrchestrationStep Default()
		=> new SocPoistOrchestration_CallRpo();

	public async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		SocPoistOrchestrationStart @event,
		IUnitOfWorkProvider unitOfWorkProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();
		//.InvocationAddContextProperty(nameof(@event.IdMessage), @event.IdMessage.ToString());

		var result = new ResultBuilder();

		return result.Build();
	}
}
