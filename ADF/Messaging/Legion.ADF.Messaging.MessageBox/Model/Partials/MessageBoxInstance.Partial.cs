using Legion.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageBoxInstance : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<MessageBoxInstance?> Create(
		IScopeContext scopeContext,
		string name,
		string version,
		int? maxDegreeOfQueueParallelism,
		int? maxDegreeOfTopicParallelism,
		LogLevel logLevel)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageBoxInstance?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, version))
			return result.Build();

		var MessageBoxInstance = new MessageBoxInstance
		{
			__IsNewObject = true,
			IdMessageBoxInstance = EnvironmentInfo.RUNTIME_UNIQUE_KEY,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			Name = name,
			Version = version,
			MaxDegreeOfQueueParallelism = maxDegreeOfQueueParallelism ?? Environment.ProcessorCount,
			MaxDegreeOfTopicParallelism = maxDegreeOfTopicParallelism ?? Environment.ProcessorCount,
			IdLogLevel = (int)logLevel
		};

		var validationResult =
			DefaultDBValidator
				.Validate(MessageBoxInstance);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(MessageBoxInstance).Build();
	}
}
