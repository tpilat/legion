namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class RetryNode : FlowNode
{
	public FlowNode Body { get; set; } = default!;
	public int RetryCount { get; set; } = 3;
	public TimeSpan? Delay { get; set; }
	public TimeSpan? Timeout { get; set; }

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		int attempt = 0;
		
		ExecutionTracker.Log(state, this.Id, NodeStatus.Started);

		while (true)
		{
			try
			{
				Task execTask = Body.ExecuteAsync(state, engine);

				if (Timeout.HasValue)
				{
					var timeoutTask = Task.Delay(Timeout.Value);
					var completed = await Task.WhenAny(execTask, timeoutTask);
					if (completed == timeoutTask)
						throw new TimeoutException($"[RetryNode] Timeout after {Timeout.Value.TotalSeconds} seconds.");

					await execTask;
				}
				else
				{
					await execTask;
				}

				// Ak vnútorný uzol bol StepNode, zachytíme jeho výsledok (akýkoľvek výstup je už v state.Data[StepName])
				if (Body is StepNode stepNode && state.Data.ContainsKey(stepNode.StepName))
				{
					state.Data[$"{stepNode.StepName}_RetryResult"] = state.Data[stepNode.StepName];
				}

				ExecutionTracker.Log(state, this.Id, NodeStatus.Succeeded);
				return; // success
			}
			catch (Exception ex)
			{
				ExecutionTracker.Log(state, this.Id, NodeStatus.Failed, ex.Message);

				attempt++;
				if (attempt > RetryCount)
					throw;

				Console.WriteLine($"[RetryNode] Retry {attempt}/{RetryCount} due to: {ex.Message}");
				if (Delay.HasValue)
					await Task.Delay(Delay.Value);
			}
		}
	}
}
