using Microsoft.Extensions.Options;
using System.Data;

namespace Legion.ADF.ESB.Orchestrations;

public abstract class ESBOrchestration : IESBOrchestration
{
	public Guid IdOrchestration { get; protected set; }

	public string Code { get; protected set; }

	public string Name { get; protected set; }

	public string? Description { get; protected set; }

	public bool IsSingleton { get; protected set; }

	public string? Properties { get; protected set; }

	public int TimeoutForMessageProcessingInSeconds { get; protected set; }

	public int MaxMessageProcessingRetryCount { get; protected set; }

	public string Version { get; protected set; }

	public DateTime? ValidTo { get; protected set; }

	public List<IESBOrchestrationStep> Steps { get; }



	public virtual Type? InterceptorType { get; }
	public virtual string? StoreId { get; }
	public virtual IsolationLevel? TransactionIsolationLevel { get; }

	public ESBOrchestration()
	{
		Steps = [];
	}

	public abstract ESBOrchestration Default();

	public IResult<Legion.ADF.ESB.Orchestrations.Model.Orchestration> ToPersistentModel(IScopeContext scopeContext)
		=> Legion.ADF.ESB.Orchestrations.Model.Orchestration.CreateOrchestration(scopeContext.CreateNew(), this);
}


public abstract class ESBOrchestration<TOptions> : ESBOrchestration, IESBOrchestration
	where TOptions : class
{
	protected TOptions Options { get; }

	public ESBOrchestration(IOptions<TOptions> options)
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
