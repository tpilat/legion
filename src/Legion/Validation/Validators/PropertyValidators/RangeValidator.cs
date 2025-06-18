using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;

namespace Legion.Validation.Validators.PropertyValidators;

public class RangeValidator
{
	public const string DEFAULT_LessThan = "Must be less than '{ValueToCompare}'.";
	public const string DEFAULT_LessThanOrEqual = "Must be less than or equal to '{ValueToCompare}'.";

	public const string DEFAULT_GreaterThan = "Must be greater than '{ValueToCompare}'.";
	public const string DEFAULT_GreaterThanOrEqual = "Must be greater than or equal to '{ValueToCompare}'.";

	public const string DEFAULT_InclusiveBetween = "Must be between {From} and {To}..";
	public const string DEFAULT_InclusiveExclusiveBetween = "Must be greater than or equal to {From} and less than {To}.";

	public const string DEFAULT_ExclusiveBetween = "Must be between {From} and {To} (exclusive).";
	public const string DEFAULT_ExclusiveInclusiveBetween = "Must be greater than {From} and less than or equal to {To}.";

	public static string GetResourceLessThanMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.LessThan, DEFAULT_LessThan) ?? DEFAULT_LessThan;

	public static string GetResourceLessThanOrEqualMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.LessThanOrEqual, DEFAULT_LessThanOrEqual) ?? DEFAULT_LessThanOrEqual;

	public static string GetResourceGreaterThanMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.GreaterThan, DEFAULT_GreaterThan) ?? DEFAULT_GreaterThan;

	public static string GetResourceGreaterThanOrEqualMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.GreaterThanOrEqual, DEFAULT_GreaterThanOrEqual) ?? DEFAULT_GreaterThanOrEqual;

	public static string GetResourceInclusiveBetweenMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.InclusiveBetween, DEFAULT_InclusiveBetween) ?? DEFAULT_InclusiveBetween;

	public static string GetResourceInclusiveExclusiveBetweenMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.InclusiveExclusiveBetween, DEFAULT_InclusiveExclusiveBetween) ?? DEFAULT_InclusiveExclusiveBetween;

	public static string GetResourceExclusiveBetweenMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.ExclusiveBetween, DEFAULT_ExclusiveBetween) ?? DEFAULT_ExclusiveBetween;

	public static string GetResourceExclusiveInclusiveBetweenMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.ExclusiveInclusiveBetween, DEFAULT_ExclusiveInclusiveBetween) ?? DEFAULT_ExclusiveInclusiveBetween;
}

internal class RangeValidator<T, TProperty> : PropertyValidator<T, TProperty?>
{
	protected override string DefaultValidationMessage { get; }

	private readonly Func<string, IErrorCode> _errorCodeDelegate;
	private readonly string _validationResourceKey;

	private readonly object? _defaultValue;

	public IComparable? From { get; }
	public bool InclusiveFrom { get; }
	public IComparable? To { get; }
	public bool InclusiveTo { get; }

	public RangeValidator(
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		object? defaultValue,
		IComparable? from,
		bool inclusiveFrom,
		IComparable? to,
		bool inclusiveTo,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.Range, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
	{
		_defaultValue = defaultValue;
		From = from;
		To = to;
		InclusiveFrom = inclusiveFrom;
		InclusiveTo = inclusiveTo;

		if (From == null)
		{
			if (To == null)
			{
				_errorCodeDelegate = Exceptions.Internal.ErrorCodes.OutOfRangeException.CustomValidation;
				_validationResourceKey = "RANGE";
				DefaultValidationMessage = "RANGE";
			}
			else
			{
				if (InclusiveTo)
				{
					_errorCodeDelegate = Exceptions.Internal.ErrorCodes.OutOfRangeException.LessOrEqualValidation;
					_validationResourceKey = ValidationResources.Validation.__Keys.LessThanOrEqual;
					DefaultValidationMessage = RangeValidator.DEFAULT_LessThanOrEqual;
				}
				else
				{
					_errorCodeDelegate = Exceptions.Internal.ErrorCodes.OutOfRangeException.LessValidation;
					_validationResourceKey = ValidationResources.Validation.__Keys.LessThan;
					DefaultValidationMessage = RangeValidator.DEFAULT_LessThan;
				}
			}
		}
		else
		{
			if (To == null)
			{
				if (InclusiveFrom)
				{
					_errorCodeDelegate = Exceptions.Internal.ErrorCodes.OutOfRangeException.GreaterOrEqualValidation;
					_validationResourceKey = ValidationResources.Validation.__Keys.GreaterThanOrEqual;
					DefaultValidationMessage = RangeValidator.DEFAULT_GreaterThanOrEqual;
				}
				else
				{
					_errorCodeDelegate = Exceptions.Internal.ErrorCodes.OutOfRangeException.GreaterValidation;
					_validationResourceKey = ValidationResources.Validation.__Keys.GreaterThan;
					DefaultValidationMessage = RangeValidator.DEFAULT_GreaterThan;
				}
			}
			else
			{
				if (InclusiveFrom)
				{
					if (InclusiveTo)
					{
						_errorCodeDelegate = Exceptions.Internal.ErrorCodes.OutOfRangeException.InclusiveBetweenValidation;
						_validationResourceKey = ValidationResources.Validation.__Keys.InclusiveBetween;
						DefaultValidationMessage = RangeValidator.DEFAULT_InclusiveBetween;
					}
					else
					{
						_errorCodeDelegate = Exceptions.Internal.ErrorCodes.OutOfRangeException.InclusiveExclusiveBetweenValidation;
						_validationResourceKey = ValidationResources.Validation.__Keys.InclusiveExclusiveBetween;
						DefaultValidationMessage = RangeValidator.DEFAULT_InclusiveExclusiveBetween;
					}
				}
				else
				{
					if (InclusiveTo)
					{
						_errorCodeDelegate = Exceptions.Internal.ErrorCodes.OutOfRangeException.ExclusiveInclusiveBetweenValidation;
						_validationResourceKey = ValidationResources.Validation.__Keys.ExclusiveInclusiveBetween;
						DefaultValidationMessage = RangeValidator.DEFAULT_ExclusiveInclusiveBetween;
					}
					else
					{
						_errorCodeDelegate = Exceptions.Internal.ErrorCodes.OutOfRangeException.ExclusiveBetweenalidation;
						_validationResourceKey = ValidationResources.Validation.__Keys.ExclusiveBetween;
						DefaultValidationMessage = RangeValidator.DEFAULT_ExclusiveBetween;
					}
				}
			}
		}
	}

	protected override IDictionary<string, object?> GetPlaceholderValues()
		=> new Dictionary<string, object?>
			{
				{ nameof(From), From },
				{ nameof(To), To },
				{ "ValueToCompare", From ?? To },
				{ "PropertyName", GetDisplayName() }
			};

	internal override IValidationResult? Validate(ValidationContext context, ValidationOptions? options)
	{
		//if (string.IsNullOrWhiteSpace(ObjectPath.PropertyName))
		//	throw new InvalidOperationException($"{nameof(ObjectPath)}.{nameof(ObjectPath.PropertyName)} == null");

		if (context is not ValidationContext<T, TProperty?> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		if (ctx.ValueToValidate == null || (From == null && To == null))
			return null;

		if (ctx.ValueToValidate is IComparable value)
		{
			var ok = true;

			if (From != null)
			{
				if (InclusiveFrom)
				{
					ok = 0 <= value.CompareTo(From);
				}
				else
				{
					ok = 0 < value.CompareTo(From);
				}
			}

			if (ok && To != null)
			{
				if (InclusiveTo)
				{
					ok = value.CompareTo(To) <= 0;
				}
				else
				{
					ok = value.CompareTo(To) < 0;
				}
			}

			if (ok)
				return null;
			else
				return new ValidationResult(
					new ValidationFailure(
						ObjectPath,
						context,
						ValidatorType,
						HasServerCondition,
						ClientConditionDefinition,
						_errorCodeDelegate(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, _validationResourceKey, options?.RangeMessageGetter)),
						GetDisplayName()!,
						FailureInfoFunc?.Invoke(ctx.InstanceToValidate)));
		}

		throw new InvalidOperationException($"{nameof(ctx.ValueToValidate)} must implement {nameof(IComparable)}.");
	}

	public override IValidatorDescriptor ToDescriptor()
		=> new ValidationDescriptor(
			typeof(T),
			ObjectPath,
			ValidatorType,
			GetType().ToFriendlyFullName(),
			HasServerCondition,
			ClientConditionDefinition,
			_errorCodeDelegate(GetValidationMessage(default, default, _validationResourceKey, null)),
			GetDisplayName())
		{
			From = From,
			InclusiveFrom = InclusiveFrom,
			To = To,
			InclusiveTo = InclusiveTo
		};
}
