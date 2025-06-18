using Legion.Validation.Internal;
using Legion.Validation.Results;
using Legion.Reflection.ObjectPaths;

namespace Legion.Validation.Validators;

public interface INavigationValidator<T> { }

public class NavigationValidator<T, TNavigation> : Validator<TNavigation>, INavigationValidator<T>, IValidator<TNavigation>
{
	internal Func<T, TNavigation> ValueGetter { get; }
	public Func<T?, ParentInstance?, bool>? Condition { get; }
	internal Func<T?, string?>? FailureInfoFunc { get; }

	public NavigationValidator(
		Func<T, TNavigation> valueGetter,
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? serverCondition,
		Func<T?, string?>? failureInfoFunc)
		: base(ValidatorType.NavigationValidator, objectPath, serverCondition != null, null)
	{
		Throw.IfArgumentNull(valueGetter);

		ValueGetter = valueGetter;
		Condition = serverCondition;
		FailureInfoFunc = failureInfoFunc;
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

		var navigationValue = ValueGetter(ctx.InstanceToValidate);

		var navigationCtx = new ValidationContext<TNavigation>(navigationValue, new ParentInstance(ObjectPath.GetParentPath(context?.Indexes), ctx.InstanceToValidate), context);
			//.SetObjectPath(ObjectPath);

		foreach (var validator in NestedValidators)
		{
			var nestedValidationResult = validator.Validate(navigationCtx, options);
			result.Merge(nestedValidationResult);
			if (nestedValidationResult?.Interrupted == true)
				return result;
		}

		return result;
	}
}
