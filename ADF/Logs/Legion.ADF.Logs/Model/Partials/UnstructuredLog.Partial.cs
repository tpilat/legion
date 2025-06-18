using Legion.Extensions;

namespace Legion.ADF.Logs.Model;

public sealed partial class UnstructuredLog : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<UnstructuredLog> CreateUnstructuredLog(
		IScopeContext scopeContext,
		DTOs.UnstructuredLog dto,
		string? sourceContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<UnstructuredLog>();

		if (result.IsArgumentNull(scopeContext, dto))
			return result.Build();

		var unstructuredLog = new UnstructuredLog
		{
			__IsNewObject = true,
			IdUnstructuredLog = dto.Id ?? Guid.NewGuid(),
			CreatedUtc = dto.CreatedUtc ?? GlobalContext.Instance.UtcNow,
			IdLogLevel = (int)dto.LogLevel,
			Message = dto.Message,
			StackTrace = dto.Exception?.ToStringTrace(),
			SourceContext = sourceContext ?? dto.SourceContext,
			RuntimeUniqueKey = Legion.Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY,
			EventName = dto.EventId.Name,
			EventId = dto.EventId.Id
		};

		var validationResult =
			DefaultDBValidator
				.Validate(unstructuredLog);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(unstructuredLog).Build();
	}
}
