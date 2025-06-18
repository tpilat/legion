using System.Reflection;

namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public static class StepRegistry
{
	private static readonly Dictionary<string, IOrchestrationStep> _steps = new();

	public static void Register(IOrchestrationStep step) => _steps[step.Name] = step;

	public static IOrchestrationStep? Resolve(string name) =>
		_steps.TryGetValue(name, out var step) ? step : null;

	public static void LoadFromAssembly(Assembly assembly)
	{
		foreach (var type in assembly.GetTypes().Where(t =>
					 typeof(IOrchestrationStep).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface))
		{
			var step = (IOrchestrationStep)Activator.CreateInstance(type)!;
			
			//step.Name = type.Name.Replace("Step", "");
			
			Register(step);
		}
	}

	public static void LoadFromDll(string path)
	{
		var assembly = Assembly.LoadFrom(path);
		LoadFromAssembly(assembly);
	}
}

