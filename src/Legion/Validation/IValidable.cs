namespace Legion.Validation;

public interface IValidable
{
	IValidationResult Validate(Dictionary<string, object>? globalValidationState = null);
}
