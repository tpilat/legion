using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using System.Collections;
using System.Text;

namespace Legion.Validation;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IValidatorDescriptor
{
	Type ObjectType { get; }

	IObjectPath ObjectPath { get; }

	ValidatorType ValidatorType { get; }

	string ValidatorTypeInfo { get; }

	bool HasServerCondition { get; }

	IClientConditionDefinition? ClientConditionDefinition { get; }

	IReadOnlyList<IValidatorDescriptor> Validators { get; }

	//DefaultOrEmpty, NotDefaultOrEmpty
	object? DefaultValue { get; }

	//Equal, NotEqual, GreaterThan, GreaterThanOrEqual, LessThan, LessThanOrEqual
	object? ValueToCompare { get; }

	//MultiEqual, MultiNotEqual
	IEnumerable<object?>? ValuesToCompare { get; }

	//Equal, NotEqual
	IEqualityComparer? Comparer { get; }

	//ExclusiveBetween, InclusiveBetween
	IComparable? From { get; }

	bool InclusiveFrom { get; }

	//ExclusiveBetween, InclusiveBetween
	IComparable? To { get; }

	bool InclusiveTo { get; }

	//Length
	int MinLength { get; }

	//Length
	int MaxLength { get; }

	//PrecisionScaleDecimal
	int Scale { get; }

	//PrecisionScaleDecimal
	int Precision { get; }

	//PrecisionScaleDecimal
	bool IgnoreTrailingZeros { get; }

	//RegEx
	string? Pattern { get; }

	IErrorCode? ErrorCode { get; }

	string? PropertyName { get; }

	bool IsEqualTo(IValidatorDescriptor other);

	string Print();

	void PrintInternal(StringBuilder sb, int indent);
}
