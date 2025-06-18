namespace Legion.Logging;

public class ErrorMessage : LogMessage, IErrorMessage
{
	internal ErrorMessage(IScopeContext scopeContext, IErrorCode errorCode)
		: base(scopeContext, errorCode ?? Throw.IfArgumentNull(errorCode))
	{
	}

	public new ErrorMessageDto ToDto(params string[] ignoredPropterties)
	{
		ignoredPropterties ??= [];
		var dto = new ErrorMessageDto();

		if (!ignoredPropterties.Contains(nameof(IdLogMessage)))
			dto.IdLogMessage = IdLogMessage;

		if (!ignoredPropterties.Contains(nameof(LogLevel)))
			dto.LogLevel = LogLevel;

		if (!ignoredPropterties.Contains(nameof(ErrorCode)))
			dto.ErrorCode = new ErrorCodeDto(ErrorCode);

		if (!ignoredPropterties.Contains(nameof(CreatedUtc)))
			dto.CreatedUtc = CreatedUtc;

		if (!ignoredPropterties.Contains(nameof(IsLogged)))
			dto.IsLogged = IsLogged;

		if (!ignoredPropterties.Contains(nameof(IsValidationError)))
			dto.IsValidationError = IsValidationError;

		if (!ignoredPropterties.Contains(nameof(ScopeContext)))
			dto.ScopeContext = ScopeContext;

		if (!ignoredPropterties.Contains(nameof(OperationName)))
			dto.OperationName = OperationName;

		if (!ignoredPropterties.Contains(nameof(AggregateName)))
			dto.AggregateName = AggregateName;

		if (!ignoredPropterties.Contains(nameof(AggregateIdentifier)))
			dto.AggregateIdentifier = AggregateIdentifier;

		if (!ignoredPropterties.Contains(nameof(ClientMessage)))
			dto.ClientMessage = ClientMessage;

		if (!ignoredPropterties.Contains(nameof(InternalMessage)))
			dto.InternalMessage = InternalMessage;

		if (!ignoredPropterties.Contains(nameof(Exception)))
			dto.Exception = Exception;

		if (!ignoredPropterties.Contains(nameof(StackTrace)))
			dto.StackTrace = StackTrace;

		if (!ignoredPropterties.Contains(nameof(Detail)))
			dto.Detail = Detail;

		if (!ignoredPropterties.Contains(nameof(PropertyName)))
			dto.PropertyName = PropertyName;

		if (!ignoredPropterties.Contains(nameof(ValidationFailure)))
			dto.ValidationFailure = ValidationFailure;

		if (!ignoredPropterties.Contains(nameof(DisplayPropertyName)))
			dto.DisplayPropertyName = DisplayPropertyName;

		if (!ignoredPropterties.Contains(nameof(SourceContext)))
			dto.SourceContext = SourceContext;

		return dto;
	}

	public new ErrorMessageDto ToClientDto()
		=> new()
		{
			IdLogMessage = IdLogMessage,
			LogLevel = LogLevel,
			ErrorCode = new ErrorCodeDto(ErrorCode),
			CreatedUtc = CreatedUtc,
			IsLogged = IsLogged,
			IsValidationError = IsValidationError,
			//OperationName = OperationName,
			//AggregateName = AggregateName,
			//AggregateIdentifier = AggregateIdentifier,
			ClientMessage = ClientMessage,
			PropertyName = PropertyName
		};
}
