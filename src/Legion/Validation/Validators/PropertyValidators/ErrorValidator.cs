using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;

namespace Legion.Validation.Validators.PropertyValidators;

internal class ErrorValidator<T, TProperty> : PropertyValidator<T, TProperty?>
{
	private readonly Func<T?, TProperty?, IErrorCode>? _errorCodeGetter;
	private readonly Func<T?, ParentInstance?, IValidationResult>? _validationFunction;

	public ErrorValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, IValidationResult> validationFunction,
		Func<T?, string?>? failureInfoFunc)
		: base(ValidatorType.ErrorProperty, valueGetter, objectPath, (x, parent) => true, null, failureInfoFunc, null, null)
	{
		Throw.IfArgumentNull(validationFunction);

		_validationFunction = validationFunction;
	}

	public ErrorValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool> serverCondition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.ErrorProperty, valueGetter, objectPath, serverCondition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
	{
		Throw.IfArgumentNull(serverCondition);
	}

	public ErrorValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool> serverCondition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		Func<T?, TProperty?, IErrorCode>? errorCodeGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.ErrorProperty, valueGetter, objectPath, serverCondition, clientConditionDefinition, failureInfoFunc, null, propertyDisplayNameGetter)
	{
		Throw.IfArgumentNull(serverCondition);

		_errorCodeGetter = errorCodeGetter;
	}

	protected override IDictionary<string, object?> GetPlaceholderValues()
		=> new Dictionary<string, object?>
			{
				{ "PropertyName", GetDisplayName() }
			};

	internal override IValidationResult? Validate(ValidationContext context, ValidationOptions? options)
	{
		//if (string.IsNullOrWhiteSpace(ObjectPath.PropertyName))
		//	throw new InvalidOperationException($"{nameof(ObjectPath)}.{nameof(ObjectPath.PropertyName)} == null");

		if (context is not ValidationContext<T> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		if (_validationFunction != null)
		{
			var result = _validationFunction(ctx.InstanceToValidate, ctx.ParentInstance);
			return result;
		}
		else if (Condition != null && Condition.Invoke(ctx.InstanceToValidate, ctx.ParentInstance))
		{
			var value = ctx.InstanceToValidate != null
				? ValueGetter(ctx.InstanceToValidate)
				: default;

			var errorCode = _errorCodeGetter != null
				? _errorCodeGetter(ctx.InstanceToValidate, value)
				: Exceptions.Internal.ErrorCodes.ValidationException.ErrorValidation(GetValidationMessage(ctx.InstanceToValidate, value, string.Empty, null));

			return new ValidationResult(
				new ValidationFailure(
					ObjectPath,
					context,
					ValidatorType,
					HasServerCondition,
					ClientConditionDefinition,
					errorCode,
					GetDisplayName()!,
					FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));
		}
		else if (ClientConditionDefinition != null && ClientConditionDefinition.Execute(ctx.InstanceToValidate))
		{
			var value = ctx.InstanceToValidate != null
				? ValueGetter(ctx.InstanceToValidate)
				: default;

			var errorCode = _errorCodeGetter != null
				? _errorCodeGetter(ctx.InstanceToValidate, value)
				: Exceptions.Internal.ErrorCodes.ValidationException.ErrorValidation(GetValidationMessage(ctx.InstanceToValidate, value, string.Empty, null));

			return new ValidationResult(
				new ValidationFailure(
					ObjectPath,
					context,
					ValidatorType,
					HasServerCondition,
					ClientConditionDefinition,
					errorCode,
					GetDisplayName()!,
					FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));
		}

		return null;
	}

	public override IValidatorDescriptor ToDescriptor()
		=> new ValidationDescriptor(
			typeof(T),
			ObjectPath,
			ValidatorType,
			GetType().ToFriendlyFullName(),
			HasServerCondition,
			ClientConditionDefinition,
			Exceptions.Internal.ErrorCodes.ValidationException.ErrorValidation(GetValidationMessage(default, default, string.Empty, null)),
			GetDisplayName());
}
