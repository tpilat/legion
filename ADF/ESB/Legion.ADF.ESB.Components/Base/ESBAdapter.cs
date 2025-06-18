using Legion.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;

namespace Legion.ADF.ESB.Components;

public abstract class ESBAdapter : IESBAdapter
{
	public Guid IdAdapter { get; protected set; }

	public string Code { get; protected set; }

	public string Name { get; protected set; }

	public string? Description { get; protected set; }

	public Guid IdAdapterStatus { get; protected set; }

	public string Class => this.GetType().ToFriendlyFullName();

	public string? Properties { get; protected set; }

	public bool IsInbound { get; protected set; }

	public bool IsOutbound { get; protected set; }


	public abstract LogLevel MinLogLevel { get; }


	public virtual Type? InterceptorType { get; }
	public virtual string? StoreId { get; }
	public virtual IsolationLevel? TransactionIsolationLevel { get; }

	public ESBAdapter()
	{
		IdAdapterStatus = Legion.ADF.ESB.Components.Model.AdapterStatus.Offline;
	}

	public abstract ESBAdapter Default();

	public IResult<Legion.ADF.ESB.Components.Model.Adapter> ToPersistentModel(IScopeContext scopeContext)
		=> Legion.ADF.ESB.Components.Model.Adapter.CreateAdapter(scopeContext.CreateNew(), this);

	public abstract IResult MergeProperties(IScopeContext scopeContext, string? savedProperties);
}


public abstract class ESBAdapter<TOptions> : ESBAdapter, IESBAdapter
	where TOptions : class, IESBAdapterConfig
{
	protected TOptions Options { get; }

	public override LogLevel MinLogLevel => Options.MinLogLevel;

	public ESBAdapter(IOptions<TOptions> options)
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
