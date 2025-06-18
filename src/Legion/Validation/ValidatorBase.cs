using Legion.Reflection.ObjectPaths;

namespace Legion.Validation;

public abstract class ValidatorBase<T> : Validator<T>, IValidatorBuilderFactory<T>
{
	protected ValidatorBuilder<T> Builder { get; }

	public ValidatorBase(IObjectPath objectPath)
		: base(ValidatorType.Validator, objectPath)
	{
		Builder = new ValidatorBuilder<T>(this);
		SetDefaultRuels(Builder);
	}

	public ValidatorBase()
	{
		Builder = new ValidatorBuilder<T>(this);
		SetDefaultRuels(Builder);
	}

	public abstract void SetDefaultRuels(ValidatorBuilder<T> builder);

	ValidatorBuilder<T> IValidatorBuilderFactory<T>.GetBuilder()
		=> Builder;
}
