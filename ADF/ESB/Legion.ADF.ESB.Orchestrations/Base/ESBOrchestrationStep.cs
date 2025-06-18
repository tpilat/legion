using Microsoft.Extensions.Options;
using System.Data;

namespace Legion.ADF.ESB.Orchestrations;

public abstract class ESBOrchestrationStep : IESBOrchestrationStep
{
	public Guid IdOrchestrationStep { get; protected set; }

	public bool IsMainEntry { get; protected set; }

	public string Code { get; protected set; }

	public string Name { get; protected set; }

	public string? Description { get; protected set; }

	public string? Properties { get; protected set; }

	public int Order { get; protected set; }



	public virtual Type? InterceptorType { get; }
	public virtual string? StoreId { get; }
	public virtual IsolationLevel? TransactionIsolationLevel { get; }



	public ESBOrchestrationStep()
	{
	}

	public abstract ESBOrchestrationStep Default();
}

public abstract class ESBOrchestrationStep<TOptions> : ESBOrchestrationStep, IESBOrchestrationStep
	where TOptions : class
{
	protected TOptions Options { get; }



	public ESBOrchestrationStep(IOptions<TOptions> options)
		: base()
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(options.Value);

		Options = options.Value;
	}

	protected string SerializeProperties()
	{
		return Newtonsoft.Json.JsonConvert.SerializeObject(
				Options,
				Newtonsoft.Json.Formatting.Indented,
				new Newtonsoft.Json.JsonSerializerSettings
				{
					NullValueHandling = Newtonsoft.Json.NullValueHandling.Include
				});
	}
}
