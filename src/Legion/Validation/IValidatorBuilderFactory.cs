namespace Legion.Validation;

public interface IValidatorBuilderFactory<T>
{
	ValidatorBuilder<T> GetBuilder();
}
