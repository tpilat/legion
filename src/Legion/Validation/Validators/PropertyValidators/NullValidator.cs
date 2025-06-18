using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;

namespace Legion.Validation.Validators.PropertyValidators;

public class NullValidator
{
	public const string DEFAULT_ValidationMessage = "Must be empty.";

	public static string GetResourceMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.Null, DEFAULT_ValidationMessage) ?? DEFAULT_ValidationMessage;
}

internal class NullValidator<T, TProperty> : PropertyValidator<T, TProperty?>
{
	protected override string DefaultValidationMessage => NullValidator.DEFAULT_ValidationMessage;

	public NullValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.Null, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
	{
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

		if (ctx.ValueToValidate == null)
			return null;
		else
			return
				new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.NullValueException.CustomValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.Null, options?.NullMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));
	}

	public override IValidatorDescriptor ToDescriptor()
		=> new ValidationDescriptor(
			typeof(T),
			ObjectPath,
			ValidatorType,
			GetType().ToFriendlyFullName(),
			HasServerCondition,
			ClientConditionDefinition,
			Exceptions.Internal.ErrorCodes.NullValueException.CustomValidation(GetValidationMessage(default, default, ValidationResources.Validation.__Keys.Null, null)),
			GetDisplayName())
		{
		};
}
