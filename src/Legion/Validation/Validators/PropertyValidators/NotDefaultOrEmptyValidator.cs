using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;

namespace Legion.Validation.Validators.PropertyValidators;

public class NotDefaultOrEmptyValidator
{
	public const string DEFAULT_ValidationMessage = "Must not be empty.";

	public static string GetResourceMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.NotDefaultOrEmpty, DEFAULT_ValidationMessage) ?? DEFAULT_ValidationMessage;
}

internal class NotDefaultOrEmptyValidator<T, TProperty> : PropertyValidator<T, TProperty?>
{
	protected override string DefaultValidationMessage => NotDefaultOrEmptyValidator.DEFAULT_ValidationMessage;

	private readonly object? _defaultValue;
	private readonly bool _stringWhiteSpaceCheck;

	public NotDefaultOrEmptyValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		object? defaultValue,
		bool stringWhiteSpaceCheck,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.NotDefaultOrEmpty, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
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
		{
			var message = GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.NotDefaultOrEmpty, options?.NotDefaultOrEmptyMessageGetter);
			var errorCode = ValidationHelper.GetNotDefaultOrEmptyErrorCode(ctx.ValueToValidate, _defaultValue, message, _stringWhiteSpaceCheck);

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
		else
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
			Exceptions.Internal.ErrorCodes.DefaultValueException.NotDefaultCustomValidation(GetValidationMessage(default, default, ValidationResources.Validation.__Keys.NotDefaultOrEmpty, null)),
			GetDisplayName())
		{
			DefaultValue = _defaultValue
		};
}
