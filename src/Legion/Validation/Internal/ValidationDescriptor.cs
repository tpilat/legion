using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using System.Collections;
using System.Text;

namespace Legion.Validation.Internal;

internal class ValidationDescriptor : IValidatorDescriptor
{
	public Type ObjectType { get; }

	public IObjectPath ObjectPath { get; }

	public ValidatorType ValidatorType { get; }

	public string ValidatorTypeInfo { get; }

	public bool HasServerCondition { get; }

	public IClientConditionDefinition? ClientConditionDefinition { get; }

	public List<IValidatorDescriptor> Validators { get; }

	IReadOnlyList<IValidatorDescriptor> IValidatorDescriptor.Validators => Validators;

	//DefaultOrEmpty, NotDefaultOrEmpty
	public object? DefaultValue { get; set; }

	//Equal, NotEqual, GreaterThan, GreaterThanOrEqual, LessThan, LessThanOrEqual
	public object? ValueToCompare { get; set; }

	//MultiEqual, MultiNotEqual
	public IEnumerable<object?>? ValuesToCompare { get; set; }

	//Equal, NotEqual
	public IEqualityComparer? Comparer { get; set; }

	//ExclusiveBetween, InclusiveBetween
	public IComparable? From { get; set; }

	public bool InclusiveFrom { get; set; }

	//ExclusiveBetween, InclusiveBetween
	public IComparable? To { get; set; }

	public bool InclusiveTo { get; set; }

	//Length
	public int MinLength { get; set; }

	//Length
	public int MaxLength { get; set; }

	//PrecisionScaleDecimal
	public int Scale { get; set; }

	//PrecisionScaleDecimal
	public int Precision { get; set; }

	//PrecisionScaleDecimal
	public bool IgnoreTrailingZeros { get; set; }

	public IErrorCode? ErrorCode { get; }

	public string? PropertyName { get; }

	//RegEx
	public string? Pattern { get; set; }

	public ValidationDescriptor(
		Type objectType,
		IObjectPath objectPath,
		ValidatorType validatorType,
		string validatorTypeInfo,
		bool hasServerCondition,
		IClientConditionDefinition? clientConditionDefinition,
		IErrorCode? errorCode,
		string? propertyName)
	{
		ObjectType = objectType ?? throw new ArgumentNullException(nameof(objectType));
		ObjectPath = objectPath?.Clone(ObjectPathCloneMode.BottomUp) ?? throw new ArgumentNullException(nameof(objectPath));
		ValidatorType = validatorType;
		ValidatorTypeInfo = validatorTypeInfo;
		HasServerCondition = hasServerCondition;
		ClientConditionDefinition = clientConditionDefinition;
		ErrorCode = errorCode;
		PropertyName = propertyName;
		Validators = [];
	}

	public ValidationDescriptor AddValidators(IEnumerable<Validator>? validators)
	{
		if (validators == null)
			return this;

		foreach (var validator in validators)
		{
			var desc = validator.ToDescriptor();
			if (desc != null)
				Validators.Add(desc);
		}

		return this;
	}

	public bool IsEqualTo(IValidatorDescriptor other)
	{
		if (other == null)
			return false;

		if (ObjectType != other.ObjectType
			|| ObjectPath.ToString() != other.ObjectPath.ToString()
			|| ValidatorType != other.ValidatorType
			|| HasServerCondition != other.HasServerCondition
			|| !Equals(ValueToCompare, other.ValueToCompare)
			|| !Equals(ValuesToCompare, other.ValuesToCompare) //TODO uprav
			|| !Equals(Comparer, other.Comparer)
			|| !Equals(From, other.From)
			|| !Equals(To, other.To)
			|| MinLength != other.MinLength
			|| MaxLength != other.MaxLength
			|| Scale != other.Scale
			|| Precision != other.Precision
			|| IgnoreTrailingZeros != other.IgnoreTrailingZeros
			|| Pattern != other.Pattern
			|| Validators.Count != other.Validators.Count)
			return false;

		for (int i = 0; i < Validators.Count; i++)
			if (!Validators[i].IsEqualTo(other.Validators[i]))
				return false;

		return true;
	}

	public string Print()
	{
		var sb = new StringBuilder();

		sb.AppendLine("_____________________________________________");
		PrintInternal(sb, 0);
		sb.AppendLine("_____________________________________________");

		return sb.ToString();
	}

	private void PrintInternal(StringBuilder sb, int indent)
	{
		if (0 < indent)
			sb.Append(string.Join("", Enumerable.Range(1, indent).Select(x => "    ")));

		sb.Append($"{ValidatorType}<{ObjectType?.FullName?.GetLastSplitSubstring(".")}> | {ObjectPath} | HasServerCondition={HasServerCondition} | Validators={Validators.Count}");

		sb.AppendLine();

		foreach (var validator in Validators)
			validator.PrintInternal(sb, indent + 1);
	}

	void IValidatorDescriptor.PrintInternal(StringBuilder sb, int indent)
		=> PrintInternal(sb, indent);

	public override string? ToString()
	{
		return $"{ValidatorType}<{ObjectType?.FullName?.GetLastSplitSubstring(".")}> | {ObjectPath} | HasServerCondition={HasServerCondition} | Validators={Validators.Count}";
	}
}
