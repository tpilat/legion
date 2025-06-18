using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;
using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using System.Collections;

namespace Legion.Validation.Validators.PropertyValidators;

public class EqualValidator
{
	public const string DEFAULT_ValidationMessage = "Must be equal to '{ValueToCompare}'.";

	public static string GetResourceMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.Equal, DEFAULT_ValidationMessage) ?? DEFAULT_ValidationMessage;
}

internal class EqualValidator<T, TProperty> : PropertyValidator<T, TProperty?>
{
	protected override string DefaultValidationMessage => EqualValidator.DEFAULT_ValidationMessage;

	public TProperty? ValueToCompare { get; }
	public IEqualityComparer? Comparer { get; }

	public EqualValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		TProperty? valueToCompare,
		IEqualityComparer? comparer,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.Equal, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
	{
		ValueToCompare = valueToCompare;
		Comparer = comparer;
	}

	protected override IDictionary<string, object?> GetPlaceholderValues()
		=> new Dictionary<string, object?>
			{
				{ nameof(ValueToCompare), ValueToCompare },
				{ "PropertyName", GetDisplayName() }
			};

	internal override IValidationResult? Validate(ValidationContext context, ValidationOptions? options)
	{
		//if (string.IsNullOrWhiteSpace(ObjectPath.PropertyName))
		//	throw new InvalidOperationException($"{nameof(ObjectPath)}.{nameof(ObjectPath.PropertyName)} == null");

		if (context is not ValidationContext<T, TProperty?> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		if (ctx.ValueToValidate == null)
			return ValueToCompare == null
				? null
				: new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.OutOfRangeException.EqualValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.Equal, options?.EqualMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));

		if (ValueToCompare == null)
			return 
				new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.OutOfRangeException.EqualValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.Equal, options?.EqualMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));

		if (Comparer == null)
			return Equals(ValueToCompare, ctx.ValueToValidate)
				? null
				: new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.OutOfRangeException.EqualValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.Equal, options?.EqualMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));
		else
			return Comparer.Equals(ValueToCompare, ctx.ValueToValidate)
				? null
				: new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.OutOfRangeException.EqualValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.Equal, options?.EqualMessageGetter)),
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
			Exceptions.Internal.ErrorCodes.OutOfRangeException.EqualValidation(GetValidationMessage(default, default, ValidationResources.Validation.__Keys.Equal, null)),
			GetDisplayName())
		{
			ValueToCompare = ValueToCompare,
			Comparer = Comparer
		};
}
