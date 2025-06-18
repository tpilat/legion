using Microsoft.Extensions.Logging;

namespace Legion.ADF.ESB.Components;

public interface IESBAdapterConfig
{
	LogLevel MinLogLevel { get; }

	void SetDefaultOptions();

	ESBAdapterConfig GetDefaultOptions();

	IResult Merge(IScopeContext scopeContext, string? savedProperties);
}
