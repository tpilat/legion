using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;

namespace Legion.Validation.Validators.PropertyValidators;

public class NotNullValidator
{
	public const string DEFAULT_ValidationMessage = "Must not be empty.";

	public static string GetResourceMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.NotNull, DEFAULT_ValidationMessage) ?? DEFAULT_ValidationMessage;
}

internal class NotNullValidator<T, TProperty> : PropertyValidator<T, TProperty?>
{
	protected override string DefaultValidationMessage => NotNullValidator.DEFAULT_ValidationMessage;

	public NotNullValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.NotNull, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
	{
	}

	protected override IDictionary<string, object?> GetPlaceholderValues()
		=> new Dictionary<string, object?>
			{
				{ "PropertyName", GetDisplayName() }
			};

	internal override IValidationResult? Validate(ValidationContext context, ValidationOptions? options)
	{
		//if (string.IsNotNullOrWhiteSpace(ObjectPath.PropertyName))
		//	throw new InvalidOperationException($"{nameof(ObjectPath)}.{nameof(ObjectPath.PropertyName)} == null");

		if (context is not ValidationContext<T, TProperty?> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		if (ctx.ValueToValidate == null)
			return
				new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.NullValueException.NotNullCustomValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.NotNull, options?.NotNullMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));
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
			Exceptions.Internal.ErrorCodes.NullValueException.NotNullCustomValidation(GetValidationMessage(default, default, ValidationResources.Validation.__Keys.NotNull, null)),
			GetDisplayName())
		{
		};
}
