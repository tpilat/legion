using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;

namespace Legion.Validation.Validators.PropertyValidators;

public class LengthValidator
{
	public const string DEFAULT_Min_ValidationMessage = "Must be at least {MinLength} characters.";

	public const string DEFAULT_Max_ValidationMessage = "Must be at the most {MaxLength} characters.";

	public const string DEFAULT_Range_ValidationMessage = "Must be between {MinLength} and {MaxLength} characters.";

	public static string GetResourceMinMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.Length_Min, DEFAULT_Min_ValidationMessage) ?? DEFAULT_Min_ValidationMessage;

	public static string GetResourceMaxMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.Length_Max, DEFAULT_Max_ValidationMessage) ?? DEFAULT_Max_ValidationMessage;

	public static string GetResourceRangeMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.Length_Range, DEFAULT_Range_ValidationMessage) ?? DEFAULT_Range_ValidationMessage;
}

internal class LengthValidator<T> : PropertyValidator<T, string?>
{
	public enum LengthTypeValidatorEnum
	{
		Min,
		Max,
		Range
	}

	public LengthTypeValidatorEnum LengthTypeValidator { get; }
	public int MinLength { get; }
	public int MaxLength { get; }

	protected override string DefaultValidationMessage => LengthTypeValidator switch
	{
		LengthValidator<T>.LengthTypeValidatorEnum.Min => LengthValidator.DEFAULT_Min_ValidationMessage,
		LengthValidator<T>.LengthTypeValidatorEnum.Max => LengthValidator.DEFAULT_Max_ValidationMessage,
		_ => LengthValidator.DEFAULT_Range_ValidationMessage,
	};

	public LengthValidator(
		LengthTypeValidatorEnum lengthTypeValidator,
		Func<T, string> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		int minLength,
		int maxLength,
		Func<T?, string?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.Length, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
	{
		if (maxLength < minLength)
			throw new ArgumentOutOfRangeException(nameof(maxLength), $"{nameof(maxLength)} should be larger than {nameof(minLength)}.");

		LengthTypeValidator = lengthTypeValidator;
		MinLength = minLength;
		MaxLength = maxLength;
	}

	protected override IDictionary<string, object?> GetPlaceholderValues()
		=> new Dictionary<string, object?>
			{
				{ nameof(MinLength), MinLength },
				{ nameof(MaxLength), MaxLength },
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

		if (MinLength <= ctx.ValueToValidate.Length && ctx.ValueToValidate.Length <= MaxLength)
			return null;
		else
		{
			string resKey;
			switch (LengthTypeValidator)
			{
				case LengthValidator<T>.LengthTypeValidatorEnum.Min:
					{
						resKey = ValidationResources.Validation.__Keys.Length_Min;
					}
					break;
				case LengthValidator<T>.LengthTypeValidatorEnum.Max:
					{
						resKey = ValidationResources.Validation.__Keys.Length_Max;
					}
					break;
				case LengthValidator<T>.LengthTypeValidatorEnum.Range:
				default:
					{
						resKey = ValidationResources.Validation.__Keys.Length_Range;
					}
					break;
			}

			return new ValidationResult(
				new ValidationFailure(
					ObjectPath,
					context,
					ValidatorType,
					HasServerCondition,
					ClientConditionDefinition,
					Exceptions.Internal.ErrorCodes.OutOfRangeException.LengthValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, resKey, options?.LengthMessageGetter)),
					GetDisplayName()!,
					FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));
		}
	}

	public override IValidatorDescriptor ToDescriptor()
	{
		string resKey;
		switch (LengthTypeValidator)
		{
			case LengthValidator<T>.LengthTypeValidatorEnum.Min:
				{
					resKey = ValidationResources.Validation.__Keys.Length_Min;
				}
				break;
			case LengthValidator<T>.LengthTypeValidatorEnum.Max:
				{
					resKey = ValidationResources.Validation.__Keys.Length_Max;
				}
				break;
			case LengthValidator<T>.LengthTypeValidatorEnum.Range:
			default:
				{
					resKey = ValidationResources.Validation.__Keys.Length_Range;
				}
				break;
		}

		return new ValidationDescriptor(
			typeof(T),
			ObjectPath,
			ValidatorType,
			GetType().ToFriendlyFullName(),
			HasServerCondition,
			ClientConditionDefinition,
			Exceptions.Internal.ErrorCodes.OutOfRangeException.LengthValidation(GetValidationMessage(default, default, resKey, null)),
			GetDisplayName())
		{
			MaxLength = MaxLength,
			MinLength = MinLength
		};
	}
}
