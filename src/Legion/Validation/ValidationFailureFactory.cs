using Legion.Reflection.ObjectPaths;
using Legion.Validation.Internal;
using System.Linq.Expressions;

namespace Legion.Validation;

public static class ValidationFailureFactory<T>
{
	public static IValidationFailure CreateError(
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorObject)
	{
		Throw.IfArgumentNull(errorCode);

		return
			new ValidationFailure(
				ObjectPath<T>.Create(),
				(Dictionary<int, int>?)null,
				type,
				true,
				null,
				errorCode,
				propertyName,
				detailLogInfo)
			{
				Severity = ValidationSeverity.Error
			};
	}

	public static IValidationFailure CreateError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Dictionary<int, int>? objectPathIndexes,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorProperty)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(errorCode);

		return
			new ValidationFailure(
				ObjectPath<T>.Create().AddProperty(expression),
				objectPathIndexes,
				type,
				true,
				null,
				errorCode,
				propertyName,
				detailLogInfo)
			{
				Severity = ValidationSeverity.Error
			};
	}

	public static IValidationFailure CreateWarning(
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorObject)
	{
		Throw.IfArgumentNull(errorCode);

		return
			new ValidationFailure(
				ObjectPath<T>.Create(),
				(Dictionary<int, int>?)null,
				type,
				true,
				null,
				errorCode,
				propertyName,
				detailLogInfo)
			{
				Severity = ValidationSeverity.Warning
			};
	}

	public static IValidationFailure CreateWarning<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Dictionary<int, int>? objectPathIndexes,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorProperty)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(errorCode);

		return
			new ValidationFailure(
				ObjectPath<T>.Create().AddProperty(expression),
				objectPathIndexes,
				type,
				true,
				null,
				errorCode,
				propertyName,
				detailLogInfo)
			{
				Severity = ValidationSeverity.Warning
			};
	}
}
