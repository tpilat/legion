namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class OrchestrationEngine
{
	private readonly FlowNode _root;
	private readonly Dictionary<string, FlowNode> _flowIndex;

	public Func<string, FlowNode?> LoadSubflow { get; set; }
	public Dictionary<string, FlowNode> FlowRegistry { get; set; }
	public CompensationFlow CompensationFlow { get; }

	public OrchestrationEngine(FlowNode root, CompensationFlow compensationFlow)
	{
		_root = root;
		CompensationFlow = compensationFlow;
		BuildFlowIndex(_root);
		_flowIndex = [];
		
		// defaultný loader: pozri do registry
		LoadSubflow = name => (FlowRegistry?.TryGetValue(name, out var flow) == true)
			? flow
			: null;

		FlowRegistry = [];
	}

	private void BuildFlowIndex(FlowNode node)
	{
		_flowIndex.Clear();
		IndexNode(node);
	}

	private void IndexNode(FlowNode node)
	{
		_flowIndex[node.Id] = node;

		switch (node)
		{
			case SequentialNode seq:
				foreach (var child in seq.Children) IndexNode(child);
				break;

			case IfNode ifNode:
				IndexNode(ifNode.Then);
				if (ifNode.Else != null) IndexNode(ifNode.Else);
				break;

			case WhileNode whileNode:
				IndexNode(whileNode.Body);
				break;

			case ForeachNode foreachNode:
				IndexNode(foreachNode.Body);
				break;

			case ParallelNode par:
				foreach (var branch in par.Branches) IndexNode(branch);
				break;

			case TryCatchNode tryCatch:
				IndexNode(tryCatch.Try);
				IndexNode(tryCatch.Catch);
				break;

			case CompensateNode compensate:
				// nič – iba zoznam názvov
				break;

			case GoToNode goTo:
				// cieľový node by mal byť v indexe už predtým
				break;
		}
	}

	public async Task StartAsync(OrchestrationState state)
	{
		state.CurrentStep = _root.Id;
		await ContinueAsync(state);
	}

	public async Task ContinueAsync(OrchestrationState state)
	{
		while (!string.IsNullOrEmpty(state.CurrentStep))
		{
			if (state.WaitingForEvent != null)
			{
				Console.WriteLine($"[Engine] Waiting for event '{state.WaitingForEvent}'...");
				return;
			}

			if (!_flowIndex.TryGetValue(state.CurrentStep, out var node))
			{
				Console.WriteLine($"[Engine] Node '{state.CurrentStep}' not found.");
				return;
			}

			Console.WriteLine($"[Engine] Executing node '{state.CurrentStep}' ({node.GetType().Name})...");
			state.CurrentStep = null;

			await node.ExecuteAsync(state, this);

			// Ak uzol nastavil nový CurrentStep (napr. GoTo), pokračujeme
		}
	}

	//Slúži na obnovenie orchestrationu po tom, čo prišiel očakávaný event (WaitForEvent)
	//Túto metódu musí volať externý komponent
	public async Task ResumeFromEvent(string eventName, Guid correlationId, object payload)
	{
		var state = OrchestrationStore.Load(correlationId);
		if (state == null)
			return;

		if (state.WaitingForEvent != eventName)
			return;

		if (!string.IsNullOrWhiteSpace(state.CorrelationProperty))
		{
			var eventProperty = payload
				.GetType()
				.GetProperty(state.CorrelationProperty!);

			var value = eventProperty?.GetValue(payload)?.ToString();

			if (value != state.CorrelationValue)
			{
				Console.WriteLine($"[Event] Correlation mismatch: expected '{state.CorrelationValue}', got '{value}'");
				return;
			}
		}

		Console.WriteLine($"[Event] Received '{eventName}' for orchestration {correlationId}");

		state.Data[eventName] = payload;
		state.WaitingForEvent = null;
		state.CorrelationProperty = null;
		state.CorrelationValue = null;
		state.WaitTimeout = null;

		OrchestrationStore.Save(state);
		await ContinueAsync(state);
	}

	//Preskočí na iný krok podľa targetNodeId
	public async Task GoToAsync(OrchestrationState state, string targetNodeId)
	{
		if (!_flowIndex.ContainsKey(targetNodeId))
			throw new Exception($"Target node '{targetNodeId}' not found");

		state.CurrentStep = targetNodeId;
		state.WaitingForEvent = null;

		OrchestrationStore.Save(state);
		await ContinueAsync(state);
	}

	//Skontroluje, či vypršal timeout čakania na udalosť (WaitForEvent)
	//potrebná pre time-based trigger, ale spúšťa sa z vonkajšieho „tick“ systému (napr. každých 5 sekúnd v background workeri)
	public async Task CheckTimeoutAsync(OrchestrationState state, DateTime lastUpdatedUtc, DateTime nowUtc)
	{
		if (state.WaitingForEvent != null && state.WaitTimeout.HasValue)
		{
			var elapsed = nowUtc - lastUpdatedUtc;
			if (elapsed > state.WaitTimeout.Value)
			{
				Console.WriteLine($"[Timeout] Wait for '{state.WaitingForEvent}' expired after {elapsed.TotalSeconds} seconds.");

				state.WaitingForEvent = null;
				state.CorrelationProperty = null;
				state.CorrelationValue = null;
				state.WaitTimeout = null;

				OrchestrationStore.Save(state);
				await ContinueAsync(state);
			}
		}
	}
}
