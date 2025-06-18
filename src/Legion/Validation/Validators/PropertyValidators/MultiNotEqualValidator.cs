using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;
using System.Collections;

namespace Legion.Validation.Validators.PropertyValidators;

public class MultiNotEqualValidator
{
	public const string DEFAULT_ValidationMessage = "Must not be equal to any of the values '[{ValuesToCompare}]'.";

	public static string GetResourceMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.MultiNotEqual, DEFAULT_ValidationMessage) ?? DEFAULT_ValidationMessage;
}

internal class MultiNotEqualValidator<T, TProperty> : PropertyValidator<T, TProperty?>
{
	protected override string DefaultValidationMessage => MultiNotEqualValidator.DEFAULT_ValidationMessage;

	public IEnumerable<object?>? ValuesToCompare { get; }
	public IEqualityComparer? Comparer { get; }

	public MultiNotEqualValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		IEnumerable<object?>? valuesToCompare,
		IEqualityComparer? comparer,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.MultiNotEqual, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
	{
		ValuesToCompare = valuesToCompare?.Distinct().ToList();
		Comparer = comparer;
	}

	protected override IDictionary<string, object?> GetPlaceholderValues()
		=> new Dictionary<string, object?>
			{
				{ nameof(ValuesToCompare), ValuesToCompare },
				{ "PropertyName", GetDisplayName() }
			};

	internal override IValidationResult? Validate(ValidationContext context, ValidationOptions? options)
	{
		//if (string.IsNullOrWhiteSpace(ObjectPath.PropertyName))
		//	throw new InvalidOperationException($"{nameof(ObjectPath)}.{nameof(ObjectPath.PropertyName)} == null");

		if (context is not ValidationContext<T, TProperty?> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		if (ctx.ValueToValidate == null)
			return (ValuesToCompare == null || ValuesToCompare.Any(x => x == null))
				? new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.OutOfRangeException.MultiNotEqualValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.MultiNotEqual, options?.MultiNotEqualMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)))
				: null;

		if (ValuesToCompare == null)
			return null;

		if (Comparer == null)
			return ValuesToCompare.Any(x => Equals(x, ctx.ValueToValidate))
				? new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.OutOfRangeException.MultiNotEqualValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.MultiNotEqual, options?.MultiNotEqualMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)))
				: null;
		else
			return ValuesToCompare.Any(x => Comparer.Equals(x, ctx.ValueToValidate))
				? new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						Exceptions.Internal.ErrorCodes.OutOfRangeException.MultiNotEqualValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.MultiNotEqual, options?.MultiNotEqualMessageGetter)),
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
			Exceptions.Internal.ErrorCodes.OutOfRangeException.MultiNotEqualValidation(GetValidationMessage(default, default, ValidationResources.Validation.__Keys.MultiNotEqual, null)),
			GetDisplayName())
		{
			ValuesToCompare = ValuesToCompare?.Cast<object>(),
			Comparer = Comparer
		};
}
