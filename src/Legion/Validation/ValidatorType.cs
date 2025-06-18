namespace Legion.Validation;

public enum ValidatorType
{
	Validator = 0,
	NavigationValidator,
	EnumerableValidator,
	PropertyValidator,
	Email,
	DefaultOrEmpty,
	NotDefaultOrEmpty,
	Equal,
	NotEqual,
	MultiEqual,
	MultiNotEqual,
	Length,
	Range,
	Null,
	NotNull,
	PrecisionScale,
	RegEx,
	AbstractValidator,
	ErrorObject,
	ErrorProperty,
	ExactLength,
	InherImplValidator
}
