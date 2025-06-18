using Legion.Reflection.ObjectPaths;
using Legion.Validation.Internal;

namespace Legion.Validation.Validators;

public class AbstractValidator<T> : Validator<T>
{
	private readonly Func<T?, AbstractValidator<T>, IValidationResult> _validationFunction;

	public AbstractValidator(
		IObjectPath objectPath,
		Func<T?, AbstractValidator<T>, IValidationResult> validationFunction)
		: base(ValidatorType.AbstractValidator, objectPath, true, null)
	{
		Throw.IfArgumentNull(validationFunction);

		_validationFunction = validationFunction;
	}

	internal override IValidationResult? Validate(ValidationContext context, ValidationOptions? options)
	{
		if (context is not ValidationContext<T> ctx)
			throw new ArgumentException($"{nameof(context)} must be type of {typeof(ValidationContext<T>).FullName}", nameof(context));

		var valResult = _validationFunction(ctx.InstanceToValidate, this);
		return valResult;
	}
}
