using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;
using System.Collections;

namespace Legion.Validation.Validators.PropertyValidators;

public class NotEqualValidator
{
	public const string DEFAULT_ValidationMessage = "Must not be equal to '{ValueToCompare}'.";

	public static string GetResourceMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.NotEqual, DEFAULT_ValidationMessage) ?? DEFAULT_ValidationMessage;
}

internal class NotEqualValidator<T, TProperty> : PropertyValidator<T, TProperty?>
{
	protected override string DefaultValidationMessage => NotEqualValidator.DEFAULT_ValidationMessage;

	public TProperty? ValueToCompare { get; }
	public IEqualityComparer? Comparer { get; }

	public NotEqualValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		TProperty? valueToCompare,
		IEqualityComparer? comparer,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.NotEqual, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
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
				? new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.OutOfRangeException.NotEqualValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.NotEqual, options?.NotEqualMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)))
				: null;

		if (ValueToCompare == null)
			return null;

		if (Comparer == null)
			return Equals(ValueToCompare, ctx.ValueToValidate)
				? new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.OutOfRangeException.NotEqualValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.NotEqual, options?.NotEqualMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)))
				: null;
		else
			return Comparer.Equals(ValueToCompare, ctx.ValueToValidate)
				? new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.OutOfRangeException.NotEqualValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.NotEqual, options?.NotEqualMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)))
				: null;
	}

	public override IValidatorDescriptor ToDescriptor()
		=> new ValidationDescriptor(
			typeof(T),
			ObjectPath,
			ValidatorType,
			GetType().ToFriendlyFullName(),
			HasServerCondition,
			ClientConditionDefinition,
			Exceptions.Internal.ErrorCodes.OutOfRangeException.NotEqualValidation(GetValidationMessage(default, default, ValidationResources.Validation.__Keys.NotEqual, null)),
			GetDisplayName())
		{
			ValueToCompare = ValueToCompare,
			Comparer = Comparer
		};
}
