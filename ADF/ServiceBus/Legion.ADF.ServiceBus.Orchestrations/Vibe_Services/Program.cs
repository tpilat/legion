namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class Program
{
	public async Task TestAsync()
	{
		//var flow = new OrchestrationFlow();
		//flow.AddStep("ValidateCustomer", result => "CreateOrder");
		//flow.AddStep("CreateOrder", result => "NotifyUser");
		//flow.AddStep("NotifyUser", result => null); // DONE

		//var compensationFlow = new CompensationFlow();

		//compensationFlow.AddCompensator("ChargeCreditCard", async state =>
		//{
		//	var chargeId = state.Data["chargeId"]?.ToString();
		//	Console.WriteLine($"[Compensating] Refunding charge {chargeId}...");
		//	// Zavolaj refund endpoint, zapíš audit, atď.
		//});

		//compensationFlow.AddCompensator("ReserveInventory", async state =>
		//{
		//	var reservationId = state.Data["reservationId"]?.ToString();
		//	Console.WriteLine($"[Compensating] Releasing inventory reservation {reservationId}...");
		//});


		//StepRegistry.Register(new ValidateCustomerStep());
		//StepRegistry.Register(new CreateOrderStep());
		//StepRegistry.Register(new NotifyUserStep());

		//var engine = new OrchestrationEngine(flow);
		//await engine.StartAsync();
	}
}
