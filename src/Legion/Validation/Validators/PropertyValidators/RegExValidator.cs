using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;
using System.Text.RegularExpressions;

namespace Legion.Validation.Validators.PropertyValidators;

public class RegExValidator
{
	public const string DEFAULT_ValidationMessage = "Is not in the correct format.";

	public static string GetResourceMessage()
		=> ValidatorConfiguration.Localizer?.GetLocalizedString(ValidationResources.Validation.__Keys.RegEx, DEFAULT_ValidationMessage) ?? DEFAULT_ValidationMessage;
}

internal class RegExValidator<T> : PropertyValidator<T, string?>
{
	protected override string DefaultValidationMessage => RegExValidator.DEFAULT_ValidationMessage;

	private readonly Regex? _regex;
	public string? Pattern { get; }

	public RegExValidator(
		Func<T, string> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? condition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		string? pattern,
		Func<T?, string?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(ValidatorType.RegEx, valueGetter, objectPath, condition, clientConditionDefinition, failureInfoFunc, messageGetter, propertyDisplayNameGetter)
	{
		Pattern = pattern;
		if (Pattern != null)
			_regex = new Regex(Pattern, RegexOptions.None, TimeSpan.FromSeconds(2.0));
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

		if (context is not ValidationContext<T, string?> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		if (ctx.ValueToValidate == null || _regex == null || _regex.IsMatch(ctx.ValueToValidate))
			return null;
		else
			return new ValidationResult(
				new ValidationFailure(
					ObjectPath,
					context,
					ValidatorType,
					HasServerCondition,
					ClientConditionDefinition,
					Exceptions.Internal.ErrorCodes.OutOfRangeException.RegExValidation(GetValidationMessage(ctx.InstanceToValidate, ctx.ValueToValidate, ValidationResources.Validation.__Keys.RegEx, options?.RegExMessageGetter)),
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
			Exceptions.Internal.ErrorCodes.OutOfRangeException.RegExValidation(GetValidationMessage(default, default, ValidationResources.Validation.__Keys.RegEx, null)),
			GetDisplayName())
		{
			Pattern = Pattern
		};
}
