using Microsoft.Extensions.Logging;

namespace Legion.ADF.ESB.Components;

public interface IESBAdapter
{
	Guid IdAdapter { get; }

	string Code { get; }

	string Name { get; }

	string? Description { get; }

	Guid IdAdapterStatus { get; }

	string Class { get; }

	string? Properties { get; }

	bool IsInbound { get; }

	bool IsOutbound { get; }


	LogLevel MinLogLevel { get; }


	IResult<Legion.ADF.ESB.Components.Model.Adapter> ToPersistentModel(IScopeContext scopeContext);

	IResult MergeProperties(IScopeContext scopeContext, string? savedProperties);
}
