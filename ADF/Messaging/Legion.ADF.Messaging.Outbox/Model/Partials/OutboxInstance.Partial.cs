using Legion.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxInstance : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<OutboxInstance?> Create(
		IScopeContext scopeContext,
		string name,
		string version,
		int? maxDegreeOfQueueParallelism,
		LogLevel logLevel)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxInstance?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, version))
			return result.Build();

		var OutboxInstance = new OutboxInstance
		{
			__IsNewObject = true,
			IdOutboxInstance = EnvironmentInfo.RUNTIME_UNIQUE_KEY,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			Name = name,
			Version = version,
			MaxDegreeOfQueueParallelism = maxDegreeOfQueueParallelism ?? Environment.ProcessorCount,
			IdLogLevel = (int)logLevel
		};

		var validationResult =
			DefaultDBValidator
				.Validate(OutboxInstance);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(OutboxInstance).Build();
	}
}
