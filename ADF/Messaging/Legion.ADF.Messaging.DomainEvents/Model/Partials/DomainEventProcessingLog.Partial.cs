namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventProcessingLog : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<DomainEventProcessingLog?> Create(
		IScopeContext scopeContext,
		Guid idDomainEvent,
		Guid IdDomainEventProcessingStatus,
		Guid? idLogMessage,
		string code,
		string? detail)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DomainEventProcessingLog?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, code))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var domainEventProcessingLog = new DomainEventProcessingLog
		{
			__IsNewObject = true,
			IdDomainEventProcessingLog = id,
			IdDomainEvent = idDomainEvent,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdDomainEventProcessingStatus = IdDomainEventProcessingStatus,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			IdLogMessage = idLogMessage,
			Code = code,
			Detail = detail
		};

		var validationResult =
			DefaultDBValidator
				.Validate(domainEventProcessingLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(domainEventProcessingLog).Build();
	}
}
