using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;

namespace Legion.Validation;

#if NET6_0_OR_GREATER
[Serializer.JsonPolymorphicConverter]
#endif
public interface IValidationFailure
{
	ValidationSeverity Severity { get; }
	IErrorCode ErrorCode { get; }
	IObjectPath ObjectPath { get; }
	string? PropertyName { get; }
	bool HasServerCondition { get; }
	string? DetailInfo { get; }
	ValidatorType Type { get; }
	IClientConditionDefinition? ClientConditionDefinition { get; }
	string MessageWithPropertyName { get; }

	string ToFullPathString(string basePath);
}
