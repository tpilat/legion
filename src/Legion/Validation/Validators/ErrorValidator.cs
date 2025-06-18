using Legion.Validation.Internal;
using Legion.Validation.Results;
using Legion.Reflection.ObjectPaths;

namespace Legion.Validation.Validators;

public class ErrorValidator<T> : Validator<T>
{
	private readonly Func<T?, IValidationResult>? _validationFunction;

	public Func<T?, ParentInstance?, bool> Condition { get; }
	internal Func<T?, string?>? FailureInfoFunc { get; }

	public Func<T?, IErrorCode> ErrorCodeFunc { get; }

	public Func<T?, string> ErrorMessageFunc { get; }

	public ErrorValidator(
		IObjectPath objectPath,
		Func<T?, IValidationResult> validationFunction,
		Func<T?, string?>? failureInfoFunc)
		: base(ValidatorType.ErrorObject, objectPath, true, null)
	{
		Throw.IfArgumentNull(validationFunction);

		_validationFunction = validationFunction;
		FailureInfoFunc = failureInfoFunc;

		Condition = null!;
		ErrorCodeFunc = null!;
		ErrorMessageFunc = null!;
	}

	public ErrorValidator(
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool> serverCondition,
		Func<T?, IErrorCode> errorCode,
		Func<T?, string?>? failureInfoFunc)
		: base(ValidatorType.ErrorObject, objectPath, true, null)
	{
		Condition = serverCondition ?? throw new ArgumentNullException(nameof(serverCondition));
		FailureInfoFunc = failureInfoFunc;

		ErrorCodeFunc = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
		ErrorMessageFunc = null!;
	}

	public ErrorValidator(
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool> serverCondition,
		Func<T?, string> errorMessage,
		Func<T?, string?>? failureInfoFunc)
		: base(ValidatorType.ErrorObject, objectPath, true, null)
	{
		Condition = serverCondition ?? throw new ArgumentNullException(nameof(serverCondition));
		FailureInfoFunc = failureInfoFunc;

		ErrorCodeFunc = null!;
		ErrorMessageFunc = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
	}

	internal override IValidationResult? Validate(ValidationContext context, ValidationOptions? options)
	{
		//if (string.IsNullOrWhiteSpace(ObjectPath.PropertyName))
		//	throw new InvalidOperationException($"{nameof(ObjectPath)}.{nameof(ObjectPath.PropertyName)} == null");

		if (context is not ValidationContext<T> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		if (_validationFunction != null)
		{
			var valResult = _validationFunction(ctx.InstanceToValidate);
			return valResult;
		}

		var result = new ValidationResult();

		if (Condition.Invoke(ctx.InstanceToValidate, ctx.ParentInstance))
		{
			var errorCode = ErrorCodeFunc != null
				? ErrorCodeFunc.Invoke(ctx.InstanceToValidate)
				: Exceptions.Internal.ErrorCodes.ValidationException.ErrorValidation(ErrorMessageFunc.Invoke(ctx.InstanceToValidate));

			result.AddFailure(
				new ValidationFailure(
					ObjectPath.Clone(ObjectPathCloneMode.BottomUp),
					context,
					ValidatorType,
					HasServerCondition,
					null,
					errorCode,
					null!,
					FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));
		}

		return result;
	}
}
