//namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

//public class OrchestrationEngineOLD
//{
//	private readonly FlowNode _root;

//	private Dictionary<string, FlowNode> _flowIndex = new();

//	public CompensationFlow CompensationFlow { get; }

//	public OrchestrationEngineOLD(FlowNode root, CompensationFlow compensationFlow)
//	{
//		_root = root;
//		CompensationFlow = compensationFlow;
//		BuildFlowIndex(_root);
//	}
//	private void BuildFlowIndex(FlowNode root)
//	{
//		_flowIndex.Clear();
//		IndexNode(root);
//	}

//	private void IndexNode(FlowNode node)
//	{
//		_flowIndex[node.Id] = node;

//		switch (node)
//		{
//			case SequentialNode seq:
//				foreach (var child in seq.Children)
//					IndexNode(child);
//				break;

//			case IfNode ifNode:
//				IndexNode(ifNode.Then);
//				if (ifNode.Else != null) IndexNode(ifNode.Else);
//				break;

//			case WhileNode whileNode:
//				IndexNode(whileNode.Body);
//				break;

//			case ParallelNode par:
//				foreach (var branch in par.Branches)
//					IndexNode(branch);
//				break;

//				// WaitForEventNode, StepNode atď. nemajú deti
//		}
//	}

//	//v1
//	public async Task StartAsync()
//	{
//		var state = new OrchestrationState
//		{
//			CorrelationId = Legion.GlobalContext.Instance.NewGuid(),
//			CurrentStep = "ValidateCustomer"
//		};

//		OrchestrationStore.Save(state);
//		await ContinueAsync(state.CorrelationId);
//	}

//	//v2
//	public async Task StartAsync(OrchestrationState state)
//	{
//		await _root.ExecuteAsync(state, this);
//	}

//	//v1
//	public async Task ContinueAsync(Guid correlationId)
//	{
//		var state = OrchestrationStore.Load(correlationId);
		
//		if (state == null || state.CurrentStep == "DONE")
//			return;

//		var step = StepRegistry.Resolve(state.CurrentStep);
		
//		if (step == null)
//			throw new Exception("Unknown step: " + state.CurrentStep);

//		try
//		{
//			var result = await step.ExecuteAsync(state);
//			state.Data[step.Name] = result.Output;

//			//Po každom úspešnom kroku zaznamenáme jeho meno
//			state.ExecutedSteps.Add(step.Name);

//			if (!string.IsNullOrEmpty(result.WaitForEvent))
//			{
//				state.WaitingForEvent = result.WaitForEvent;
//				OrchestrationStore.Save(state);
//				return; // STOP – čakáme na event
//			}

//			state.WaitingForEvent = null;
//			state.CurrentStep = _flow.GetNextStep(step.Name, result) ?? "DONE";

//			OrchestrationStore.Save(state);
//			await ContinueAsync(state.CorrelationId);
//		}
//		catch (Exception ex)
//		{
//			Console.WriteLine($"Step {step.Name} failed: {ex.Message}");
//			await StartCompensationAsync(state, _compensationFlow);
//		}
//	}

//	//v2
//	public async Task ContinueAsync(OrchestrationState state)
//	{
//		if (state.WaitingForEvent != null)
//		{
//			Console.WriteLine($"[Engine] Still waiting for event '{state.WaitingForEvent}'...");
//			return;
//		}

//		var node = _flowIndex[state.CurrentStep];
//		await node.ExecuteAsync(state, this);
//	}

//	public async Task OnEventReceivedAsync(string eventName, Guid correlationId, object payload)
//	{
//		var state = OrchestrationStore.Load(correlationId);

//		if (state == null || state.WaitingForEvent != eventName)
//			return;

//		Console.WriteLine($"[Event] Received '{eventName}' for orchestration {correlationId}");

//		state.Data[eventName] = payload;
//		state.WaitingForEvent = null;
//		OrchestrationStore.Save(state);

//		// pokračuj ďalej v orchestratione
//		await ContinueAsync(correlationId);
//	}

//	public async Task StartCompensationAsync(OrchestrationState state, CompensationFlow compensationFlow)
//	{
//		state.IsInCompensation = true;

//		for (int i = state.ExecutedSteps.Count - 1; i >= 0; i--)
//		{
//			var stepName = state.ExecutedSteps[i];
//			Console.WriteLine($"[Saga] Compensating {stepName}...");
//			await compensationFlow.CompensateAsync(stepName, state);
//		}

//		state.CurrentStep = "COMPENSATED";
//		OrchestrationStore.Save(state);
//	}
//}
