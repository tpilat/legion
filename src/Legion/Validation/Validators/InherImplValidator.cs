using Legion.Validation.Internal;
using Legion.Validation.Results;
using Legion.Reflection.ObjectPaths;

namespace Legion.Validation.Validators;

public interface IInherImplValidator<T> { }

public class InherImplValidator<T, TInherImpl> : Validator<TInherImpl>, IInherImplValidator<T>, IValidator<TInherImpl>
	where TInherImpl : class, T
{
	public Func<T?, ParentInstance?, bool>? Condition { get; }
	internal Func<T?, string?>? FailureInfoFunc { get; }

	public InherImplValidator(
		IObjectPath objectPath,
		Func<T?, ParentInstance?, bool>? serverCondition,
		Func<T?, string?>? failureInfoFunc)
		: base(ValidatorType.AbstractValidator, objectPath, serverCondition != null, null)
	{
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

		var inherImplValue = (TInherImpl)ctx.InstanceToValidate;

		var inherImplCtx = new ValidationContext<TInherImpl>(inherImplValue, new ParentInstance(ObjectPath.GetParentPath(context?.Indexes), ctx.InstanceToValidate), context);
			//.SetObjectPath(ObjectPath);

		foreach (var validator in NestedValidators)
		{
			var nestedValidationResult = validator.Validate(inherImplCtx, options);
			result.Merge(nestedValidationResult);
			if (nestedValidationResult?.Interrupted == true)
				return result;
		}

		return result;
	}
}
