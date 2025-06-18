namespace Legion.Validation;

public interface IValidatorFactory<T>
{
	IValidator<T> Build();
}
