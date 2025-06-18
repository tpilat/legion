using Legion.Reflection.ObjectPaths;

namespace Legion.Validation;

public interface IValidator
{
	ValidatorType ValidatorType { get; }
	IObjectPath ObjectPath { get; }
	IValidatorDescriptor ToDescriptor();
	void AddValidatorInternal(Validator validator);
}

public interface IValidator<T> : IValidator
{
	IValidationResult Validate(T? obj, Dictionary<int, int>? indexes = null, ValidationOptions? options = null);
}