using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;
using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Text;

namespace Legion.Validation.Validators;

public interface IPropertyValidator<T> { }

public class PropertyValidator<T, TProperty> : Validator<T>, IPropertyValidator<T>, IValidator<T>
{
	internal Func<T, TProperty> ValueGetter { get; }
	public Func<T?, ParentInstance?, bool>? Condition { get; }
	internal Func<T?, string?>? FailureInfoFunc { get; }

	protected Func<T?, TProperty?, string, string?>? MessageGetter { get; }
	protected Func<string>? PropertyDisplayNameGetter { get; }
	protected virtual string DefaultValidationMessage { get; }
	protected TemplateFormatter TemplateFormatter { get; }

	internal PropertyValidator(
		ValidatorType validatorType,
		Func<T, TProperty> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? serverCondition,
		IClientConditionDefinition? clientConditionDefinition,
		Func<T?, string?>? failureInfoFunc,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter)
		: base(validatorType, objectPath, serverCondition != null, clientConditionDefinition)
	{
		ValueGetter = valueGetter ?? throw new ArgumentNullException(nameof(valueGetter));
		Condition = serverCondition;
		FailureInfoFunc = failureInfoFunc;

		DefaultValidationMessage = "";
		MessageGetter = messageGetter;
		PropertyDisplayNameGetter = propertyDisplayNameGetter;
		TemplateFormatter = new TemplateFormatter();
	}

	internal override IValidationResult? Validate(ValidationContext context, ValidationOptions? options)
	{
		var result = new ValidationResult();

		//if (string.IsNullOrWhiteSpace(ObjectPath.PropertyName))
		//	throw new InvalidOperationException($"{nameof(ObjectPath)}.{nameof(ObjectPath.PropertyName)} == null");

		if (context is not ValidationContext<T> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		if (Condition != null)
		{
			if (!Condition.Invoke(ctx.InstanceToValidate, ctx.ParentInstance))
				result.SkipNestedValidation = true;
		}
		else if (ClientConditionDefinition != null)
		{
			if (!ClientConditionDefinition.Execute(ctx.InstanceToValidate))
				result.SkipNestedValidation = true;
		}

		if (result.SkipNestedValidation)
			return result;

		if (ctx.InstanceToValidate == null)
			return null;

		var propertyValue = ValueGetter(ctx.InstanceToValidate);

		var propertyCtx = new ValidationContext<T, TProperty>(ctx.InstanceToValidate, propertyValue, new ParentInstance(ObjectPath.GetParentPath(context?.Indexes), ctx.InstanceToValidate), context);
			//.SetObjectPath(ObjectPath);

		foreach (var validator in NestedValidators)
		{
			var nestedValidationResult = validator.Validate(propertyCtx, options);
			result.Merge(nestedValidationResult);
			if (nestedValidationResult?.Interrupted == true)
				return result;
		}

		return result;
	}

	protected virtual IDictionary<string, object?> GetPlaceholderValues()
		=> new Dictionary<string, object?>();

	protected Func<T?, TProperty?, string, string?>? GetMessageGetter(Func<object?, object?, string, string?>? func)
	{
		if (MessageGetter != null)
			return MessageGetter;

		if (func == null)
			return default;

		return (instanceToValidate, valueToValidate, resourceKey) => func(instanceToValidate, valueToValidate, resourceKey);
	}

	protected string GetValidationMessage(T? instanceToValidate, TProperty? valueToValidate, string resourceKey, Func<object?, object?, string, string?>? optionsMessageGetterFunc)
		=> GetFormattedMessage(
			instanceToValidate,
			valueToValidate,
			GetMessageGetter(optionsMessageGetterFunc),
			resourceKey,
			DefaultValidationMessage,
			GetPlaceholderValues());

	protected string GetFormattedMessage(
		T? instanceToValidate,
		TProperty? valueToValidate,
		Func<T?, TProperty?, string, string?>? resourceGetter,
		string resourceKey,
		string defaultMessage,
		IDictionary<string, object?>? placeholderValues = null)
	{
		string? template = null;

		if (resourceGetter != null)
			template = resourceGetter.Invoke(instanceToValidate, valueToValidate, resourceKey);

		if (string.IsNullOrWhiteSpace(template))
			template = ValidatorConfiguration.Localizer?.GetLocalizedString(resourceKey, defaultMessage) ?? defaultMessage;

		return TemplateFormatter.Format(template!, placeholderValues) ?? "?Error";
	}

	protected string? GetDisplayName()
		=> PropertyDisplayNameGetter?.Invoke() ?? ObjectPath?.PropertyName;
}
