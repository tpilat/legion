using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;

namespace Legion.Validation.Validators.PropertyValidators;

public class DefaultOrEmptyValidator
{
	public const string DEFAULT_ValidationMessage = "Must be empty.";

	public static string GetResourceMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.DefaultOrEmpty, DEFAULT_ValidationMessage) ?? DEFAULT_ValidationMessage;
}

internal class DefaultOrEmptyValidator<T, TProperty> : PropertyValidator<T, TProperty?>
{
	protected override string DefaultValidationMessage => DefaultOrEmptyValidator.DEFAULT_ValidationMessage;

	private readonly object? _defaultValue;
	private readonly bool _stringWhiteSpaceCheck;

	public DefaultOrEmptyValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		object? defaultValue,
		bool stringWhiteSpaceCheck,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.DefaultOrEmpty, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
	{
		_defaultValue = defaultValue;
		_stringWhiteSpaceCheck = stringWhiteSpaceCheck;
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

		if (context is not ValidationContext<T, TProperty?> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		if (ValidationHelper.IsDefaultOrEmpty(ctx.ValueToValidate, _defaultValue, _stringWhiteSpaceCheck))
			return null;
		else
		{
			var message = GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.DefaultOrEmpty, options?.DefaultOrEmptyMessageGetter);
			var errorCode = ValidationHelper.GetDefaultOrEmptyErrorCode(ctx.ValueToValidate, _defaultValue, message, _stringWhiteSpaceCheck);

			return new ValidationResult(
				new ValidationFailure(
					ObjectPath,
					context,
					ValidatorType,
					HasServerCondition,
					ClientConditionDefinition,
					errorCode!,
					GetDisplayName()!,
					FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));
		}
	}

	public override IValidatorDescriptor ToDescriptor()
		=> new ValidationDescriptor(
			typeof(T),
			ObjectPath,
			ValidatorType,
			GetType().ToFriendlyFullName(),
			HasServerCondition,
			ClientConditionDefinition,
			Exceptions.Internal.ErrorCodes.DefaultValueException.CustomValidation(GetValidationMessage(default, default, ValidationResources.Validation.__Keys.DefaultOrEmpty, null)),
			GetDisplayName())
		{
			DefaultValue = _defaultValue
		};
}
