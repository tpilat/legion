using Microsoft.Extensions.Logging;

namespace Legion.ADF.ESB.Components;

public abstract class ESBAdapterConfig : IESBAdapterConfig
{
	public LogLevel MinLogLevel { get; set; }

	public abstract void SetDefaultOptions();

	public abstract ESBAdapterConfig GetDefaultOptions();

	public abstract IResult Merge(IScopeContext scopeContext, string? savedProperties);
}
