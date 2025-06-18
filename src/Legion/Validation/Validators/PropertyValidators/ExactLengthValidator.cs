using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;

namespace Legion.Validation.Validators.PropertyValidators;

public class ExactLengthValidator
{
	public const string DEFAULT_ValidationMessage = "Must be {Length} characters long.";

	public static string GetResourceMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.ExactLength, DEFAULT_ValidationMessage) ?? DEFAULT_ValidationMessage;
}

internal class ExactLengthValidator<T> : PropertyValidator<T, string?>
{
	protected override string DefaultValidationMessage => ExactLengthValidator.DEFAULT_ValidationMessage;

	public int Length { get; }

	public ExactLengthValidator(
		Func<T, string> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		int length,
		Func<T?, string?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.ExactLength, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
	{
		if (length <= 0)
			throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} should be larger than 0.");

		Length = length;
	}

	protected override IDictionary<string, object?> GetPlaceholderValues()
		=> new Dictionary<string, object?>
			{
				{ nameof(Length), Length },
				{ "PropertyName", GetDisplayName() }
			};

	internal override IValidationResult? Validate(ValidationContext context, ValidationOptions? options)
	{
		//if (string.IsNullOrWhiteSpace(ObjectPath.PropertyName))
		//	throw new InvalidOperationException($"{nameof(ObjectPath)}.{nameof(ObjectPath.PropertyName)} == null");

		if (context is not ValidationContext<T, string?> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		if (ctx.ValueToValidate == null)
			return null;

		if (Length == ctx.ValueToValidate.Length)
			return null;
		else
			return new ValidationResult(
				new ValidationFailure(
					ObjectPath,
					context,
					ValidatorType,
					HasServerCondition,
					ClientConditionDefinition,
					Exceptions.Internal.ErrorCodes.OutOfRangeException.ExactLengthValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.ExactLength, options?.LengthMessageGetter)),
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
			Exceptions.Internal.ErrorCodes.OutOfRangeException.ExactLengthValidation(GetValidationMessage(default, default, ValidationResources.Validation.__Keys.ExactLength, null)),
			GetDisplayName())
		{
			MaxLength = Length,
			MinLength = Length
		};
}
