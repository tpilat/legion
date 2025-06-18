using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;

namespace Legion.Validation.Internal;

internal class ValidationFailure : IValidationFailure
{
	public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;
	public IErrorCode ErrorCode { get; set; }
	public IObjectPath ObjectPath { get; }
	public string? PropertyName { get; internal set; }
	public bool HasServerCondition { get; }
	public string? DetailInfo { get; internal set; }
	public ValidatorType Type { get; }
	public IClientConditionDefinition? ClientConditionDefinition { get; }
	public string MessageWithPropertyName =>
		string.IsNullOrWhiteSpace(PropertyName)
			? ErrorCode.Message
			: $"{PropertyName} - {ErrorCode.Message}";

	public ValidationFailure(
		IObjectPath objectPath,
		Dictionary<int, int>? objectPathIndexes,
		ValidatorType type,
		bool hasServerCondition,
		IClientConditionDefinition? clientConditionDefinition,
		IErrorCode errorCode,
		string? propertyName,
		string? detailInfo)
	{
		Throw.IfArgumentNull(objectPath);
		Throw.IfArgumentNull(errorCode);

		ObjectPath = objectPath.CloneAndSetIndexes(ObjectPathCloneMode.BottomUp, objectPathIndexes);

		Type = type;
		HasServerCondition = hasServerCondition;
		ClientConditionDefinition = clientConditionDefinition;

		ErrorCode = errorCode;
		PropertyName = propertyName;
		DetailInfo = detailInfo;
	}

	public ValidationFailure(
		IObjectPath objectPath,
		ValidationContext context,
		ValidatorType type,
		bool hasServerCondition,
		IClientConditionDefinition? clientConditionDefinition,
		IErrorCode errorCode,
		string? propertyName,
		string? detailInfo)
		: this(
			objectPath,
			context?.Indexes,
			type,
			hasServerCondition,
			clientConditionDefinition,
			errorCode,
			propertyName,
			detailInfo)
	{
	}

	public string ToValidatorString()
		=> $"{ObjectPath}: {Type}: {(Severity == ValidationSeverity.Error ? MessageWithPropertyName : $"{Severity}: {MessageWithPropertyName}")}";

	public string ToFullPathString(string basePath)
		=> $"{(string.IsNullOrWhiteSpace(basePath) ? "" : $"{basePath.TrimPostfix(".")}.")}{ObjectPath.ToString()?.TrimPrefix("_").TrimPrefix(".")}: {(Severity == ValidationSeverity.Error ? ErrorCode.Message : $"{Severity}: {ErrorCode.Message}")}";

	public override string ToString()
		=> Severity == ValidationSeverity.Error ? MessageWithPropertyName : $"{Severity}: {MessageWithPropertyName}";
}
