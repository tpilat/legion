using Legion.Validation.Results;
using System.Linq.Expressions;

namespace Legion.Validation;

public static class IValidationResultExtensions
{
	public static IValidationResult Merge(
		this IValidationResult validationResult,
		IValidationResult otherValidationResult)
	{
		Throw.IfArgumentNull(validationResult);

		var typedValidationResult = validationResult as ValidationResult;
		if (typedValidationResult == null)
			Throw.IfNull(typedValidationResult);

		Throw.IfArgumentNull(otherValidationResult);

		if (0 < otherValidationResult.Failures?.Count)
			foreach (var failure in otherValidationResult.Failures)
				typedValidationResult.AddFailure(failure);

		return validationResult;
	}

	public static IValidationResult AddError<T>(
		this IValidationResult validationResult,
		T instanceToValidate,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorObject)
		=> AddError<T>(
			validationResult,
			errorCode,
			propertyName,
			detailLogInfo,
			type);

	public static IValidationResult AddError<T>(
		this IValidationResult validationResult,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorObject)
	{
		Throw.IfArgumentNull(validationResult);

		var typedValidationResult = validationResult as ValidationResult;
		if (typedValidationResult == null)
			Throw.IfNull(typedValidationResult);

		Throw.IfArgumentNull(errorCode);

		typedValidationResult.AddFailure(
			ValidationFailureFactory<T>.CreateError(
				errorCode,
				propertyName,
				detailLogInfo,
				type));

		return validationResult;
	}

	public static IValidationResult AddError<T, TProperty>(
		this IValidationResult validationResult,
		T instanceToValidate,
		Expression<Func<T, TProperty>> expression,
		Dictionary<int, int>? objectPathIndexes,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorProperty)
		=> AddError(
			validationResult,
			expression,
			objectPathIndexes,
			errorCode,
			propertyName,
			detailLogInfo,
			type);

	public static IValidationResult AddError<T, TProperty>(
		this IValidationResult validationResult,
		Expression<Func<T, TProperty>> expression,
		Dictionary<int, int>? objectPathIndexes,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorProperty)
	{
		Throw.IfArgumentNull(validationResult);

		var typedValidationResult = validationResult as ValidationResult;
		if (typedValidationResult == null)
			Throw.IfNull(typedValidationResult);

		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(errorCode);

		//var vc = new ValidationContext<T, TProperty>(instanceToValidate, expression.Compile().Invoke(instanceToValidate), null);

		typedValidationResult.AddFailure(
			ValidationFailureFactory<T>.CreateError(
				expression,
				objectPathIndexes,
				errorCode,
				propertyName,
				detailLogInfo,
				type));

		return validationResult;
	}

	public static IValidationResult AddWarning<T>(
		this IValidationResult validationResult,
		T instanceToValidate,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorObject)
		=> AddWarning<T>(
			validationResult,
			errorCode,
			propertyName,
			detailLogInfo,
			type);

	public static IValidationResult AddWarning<T>(
		this IValidationResult validationResult,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorObject)
	{
		Throw.IfArgumentNull(validationResult);

		var typedValidationResult = validationResult as ValidationResult;
		if (typedValidationResult == null)
			Throw.IfNull(typedValidationResult);

		Throw.IfArgumentNull(errorCode);

		typedValidationResult.AddFailure(
			ValidationFailureFactory<T>.CreateWarning(
				errorCode,
				propertyName,
				detailLogInfo,
				type));

		return validationResult;
	}

	public static IValidationResult AddWarning<T, TProperty>(
		this IValidationResult validationResult,
		T instanceToValidate,
		Expression<Func<T, TProperty>> expression,
		Dictionary<int, int>? objectPathIndexes,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorProperty)
		=> AddWarning(
			validationResult,
			expression,
			objectPathIndexes,
			errorCode,
			propertyName,
			detailLogInfo,
			type);

	public static IValidationResult AddWarning<T, TProperty>(
		this IValidationResult validationResult,
		Expression<Func<T, TProperty>> expression,
		Dictionary<int, int>? objectPathIndexes,
		IErrorCode errorCode,
		string? propertyName = null,
		string? detailLogInfo = null,
		ValidatorType type = ValidatorType.ErrorProperty)
	{
		Throw.IfArgumentNull(validationResult);

		var typedValidationResult = validationResult as ValidationResult;
		if (typedValidationResult == null)
			Throw.IfNull(typedValidationResult);

		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(errorCode);

		//var vc = new ValidationContext<T, TProperty>(instanceToValidate, expression.Compile().Invoke(instanceToValidate), null);

		typedValidationResult.AddFailure(
			ValidationFailureFactory<T>.CreateWarning(
				expression,
				objectPathIndexes,
				errorCode,
				propertyName,
				detailLogInfo,
				type));

		return validationResult;
	}

	//public static IValidationResult Merge(this IValidationResult validationResult, IValidationResult? result)
	//{
	//	Throw.ArgumentNull(validationResult);

	//	if (result == null)
	//		return validationResult;

	//	var typedValidationResult = validationResult as ValidationResult;
	//	if (typedValidationResult == null)
	//		Throw.IfNull(typedValidationResult);

	//	typedValidationResult.Merge(result);

	//	return validationResult;
	//}
}
