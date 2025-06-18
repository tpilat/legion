using Legion.Exceptions;

namespace Legion.Validation;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IValidationResult
{
	IReadOnlyList<IValidationFailure> Failures { get; }
	bool HasFailure { get; }
	bool HasError { get; }
	bool HasWarning { get; }
	bool Interrupted { get; }

	IResult ToResult(IScopeContext scopeContext, bool clientMessageWithPropertyName = true);

	ValidationException? ToException(IScopeContext scopeContext, IErrorCode? errorCode = null, bool clientMessageWithPropertyName = true, bool withErrorMessageDetails = false);
}
