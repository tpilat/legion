using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Results;

namespace Legion.Validation;

public abstract class Validator : IValidator
{
	protected internal List<Validator> NestedValidators { get; }
	public ValidatorType ValidatorType { get; }
	public IObjectPath ObjectPath { get; private set; }

	internal Validator(ValidatorType validatorType, IObjectPath objectPath, List<Validator>? nestedValidators)
	{
		ValidatorType = validatorType;
		ObjectPath = objectPath ?? throw new ArgumentNullException(nameof(objectPath));
		NestedValidators = nestedValidators ?? [];
	}

	internal abstract IValidationResult? Validate(ValidationContext context, ValidationOptions? options);

	void IValidator.AddValidatorInternal(Validator validator)
	{
		Throw.IfArgumentNull(validator);

		NestedValidators.Add(validator);
	}

	//void IValidator.AttachValidator(ValidatorBase validator)
	//{
	//	if (validator == null)
	//		throw new ArgumentNullException(nameof(validator));

	//	var clonedObjectPath = ObjectPath.Clone(ObjectPathCloneMode.BottomUp);
	//	var clonedValidatorObjectPath = validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp);
	//	clonedObjectPath.SetDescendant(clonedValidatorObjectPath, clonedValidatorObjectPath.PropertyName!, true);
	//	validator.ObjectPath = clonedObjectPath.Descendant!;
	//	NestedValidators.Add(validator);
	//}

	public abstract IValidatorDescriptor ToDescriptor();
}

public class Validator<T> : Validator, IValidator<T>
{
	public bool HasServerCondition { get; }
	public IClientConditionDefinition? ClientConditionDefinition { get; }

	public Validator(ValidatorType validatorType, IObjectPath objectPath)
		: base(validatorType, objectPath, nestedValidators: null)
	{
		HasServerCondition = false;
		ClientConditionDefinition = null;
	}

	public Validator()
		: base(ValidatorType.Validator, ObjectPath<T>.Create(), nestedValidators: null)
	{
		HasServerCondition = false;
		ClientConditionDefinition = null;
	}

	internal Validator(
		ValidatorType validatorType,
		IObjectPath objectPath,
		bool hasServerCondition,
		IClientConditionDefinition? clientConditionDefinition,
		List<Validator>? nestedValidators = null)
		: base(validatorType, objectPath, nestedValidators)
	{
		HasServerCondition = hasServerCondition;
		ClientConditionDefinition = clientConditionDefinition;
	}

	public virtual IValidationResult Validate(T? obj, int? index, ValidationOptions? options = null)
	{
		Dictionary<int, int>? indexes = null;
		if (index.HasValue)
		{
			indexes = [];
			indexes[ObjectPath.Depth] = index.Value;
		}

		var ctx = new ValidationContext<T>(obj, indexes, null);
		return Validate(ctx, options) ?? new ValidationResult();
	}

	public virtual IValidationResult Validate(T? obj, Dictionary<int, int>? indexes = null, ValidationOptions? options = null)
	{
		var ctx = new ValidationContext<T>(obj, indexes, null);
		return Validate(ctx, options) ?? new ValidationResult();
	}

	internal override IValidationResult? Validate(ValidationContext context, ValidationOptions? options)
	{
		var result = new ValidationResult();

		foreach (var validator in NestedValidators)
		{
			var nestedValidationResult = validator.Validate(context, options);
			result.Merge(nestedValidationResult);
			if (nestedValidationResult?.Interrupted == true)
				return result;
		}

		return result;
	}

	public override IValidatorDescriptor ToDescriptor()
		=> new ValidationDescriptor(
			typeof(T),
			ObjectPath,
			ValidatorType,
			GetType().ToFriendlyFullName(),
			HasServerCondition,
			ClientConditionDefinition,
			null,
			null)
			.AddValidators(NestedValidators);

	public override string? ToString()
		=> $"{ValidatorType}<{typeof(T).FullName?.GetLastSplitSubstring(".")}> | {ObjectPath} | Conditional={HasServerCondition} | Validators={NestedValidators?.Count ?? 0}";
}
