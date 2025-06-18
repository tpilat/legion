using System.Linq.Expressions;

namespace Legion.Validation.Results;

public static class ValidationResultFactory
{
	public static IValidationResult Success()
		=> new ValidationResult();

	public static IValidationResult Failure<T>(
		T instanceToValidate,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorObject)
		=> Failure<T>(
			errorCode,
			propertyName,
			detailLogInfo,
			type);

	public static IValidationResult Failure<T>(
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorObject)
	{
		Throw.IfArgumentNull(errorCode);

		var validationResult = new ValidationResult();

		validationResult.AddFailure(
			ValidationFailureFactory<T>.CreateError(
				errorCode,
				propertyName,
				detailLogInfo,
				type));

		return validationResult;
	}

	public static IValidationResult Failure<T, TProperty>(
		T instanceToValidate,
		Expression<Func<T, TProperty>> expression,
		Dictionary<int, int>? objectPathIndexes,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorProperty)
		=> Failure(
			expression,
			objectPathIndexes,
			errorCode,
			propertyName,
			detailLogInfo,
			type);

	public static IValidationResult Failure<T, TProperty>(
		Expression<Func<T, TProperty>> expression,
		Dictionary<int, int>? objectPathIndexes,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorProperty)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(errorCode);

		var validationResult = new ValidationResult();

		validationResult.AddFailure(
			ValidationFailureFactory<T>.CreateError(
				expression,
				objectPathIndexes,
				errorCode,
				propertyName,
				detailLogInfo,
				type));

		return validationResult;
	}

	public static IValidationResult Warning<T>(
		T instanceToValidate,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorObject)
		=> Warning<T>(
			errorCode,
			propertyName,
			detailLogInfo,
			type);

	public static IValidationResult Warning<T>(
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorObject)
	{
		Throw.IfArgumentNull(errorCode);

		var validationResult = new ValidationResult();

		validationResult.AddFailure(
			ValidationFailureFactory<T>.CreateWarning(
				errorCode,
				propertyName,
				detailLogInfo,
				type));

		return validationResult;
	}

	public static IValidationResult Warning<T, TProperty>(
		T instanceToValidate,
		Expression<Func<T, TProperty>> expression,
		Dictionary<int, int>? objectPathIndexes,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorProperty)
		=> Warning(
			expression,
			objectPathIndexes,
			errorCode,
			propertyName,
			detailLogInfo,
			type);

	public static IValidationResult Warning<T, TProperty>(
		Expression<Func<T, TProperty>> expression,
		Dictionary<int, int>? objectPathIndexes,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorProperty)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(errorCode);

		var validationResult = new ValidationResult();

		validationResult.AddFailure(
			ValidationFailureFactory<T>.CreateWarning(
				expression,
				objectPathIndexes,
				errorCode,
				propertyName,
				detailLogInfo,
				type));

		return validationResult;
	}
}
