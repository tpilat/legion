using Legion.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxInstance : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<InboxInstance?> Create(
		IScopeContext scopeContext,
		string name,
		string version,
		int? maxDegreeOfQueueParallelism,
		LogLevel logLevel)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxInstance?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, version))
			return result.Build();

		var InboxInstance = new InboxInstance
		{
			__IsNewObject = true,
			IdInboxInstance = EnvironmentInfo.RUNTIME_UNIQUE_KEY,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			Name = name,
			Version = version,
			MaxDegreeOfQueueParallelism = maxDegreeOfQueueParallelism ?? Environment.ProcessorCount,
			IdLogLevel = (int)logLevel
		};

		var validationResult =
			DefaultDBValidator
				.Validate(InboxInstance);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(InboxInstance).Build();
	}
}
