using Legion.Reflection.ObjectPaths;
using Legion.Validation.Client;
using Legion.Validation.Internal;
using Legion.Validation.Validators;
using System.Linq.Expressions;

//#nullable disable

namespace Legion.Validation;

//#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
public interface IValidatorBuilder<TBuilder, T> : IValidatorDescriptorBuilder, IValidatorFactory<T>
	where TBuilder : IValidatorBuilder<TBuilder, T>
{
	TBuilder Object(Validator<T> validator);

	TBuilder ForProperty<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Action<PropertyValidator<T, TProperty>> propertyValidatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<ClientCondition<T>, IClientConditionDefinition>? clientCondition = null,
		Func<T?, string>? failureInfoFunc = null);

	TBuilder ForNavigation<TNavigation>(
		Expression<Func<T, TNavigation>> expression,
		Action<ValidatorBuilder<TNavigation>> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null);

	TBuilder ForNavigation<TNavigation>(
		Expression<Func<T, TNavigation?>> expression,
		Action<ValidatorBuilder<TNavigation>, Dictionary<string, object>?, Dictionary<string, object>?> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null);

	TBuilder ForEach<TItem>(
		Expression<Func<T, IEnumerable<TItem>>> expression,
		Action<ValidatorBuilder<TItem>> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? detailInfoFunc = null);

	TBuilder ForEach<TItem>(
		Expression<Func<T, IEnumerable<TItem>?>> expression,
		Action<ValidatorBuilder<TItem>, Dictionary<string, object>?, Dictionary<string, object>?> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? detailInfoFunc = null,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null);

	TBuilder ForChildInheritance<TNext>(
		Action<ValidatorBuilder<TNext>> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null)
		where TNext : class, T;

	TBuilder ForChildInheritance<TNext>(
		Action<ValidatorBuilder<TNext>, Dictionary<string, object>?, Dictionary<string, object>?> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
		where TNext : class, T;

	TBuilder ForChildImplementation<TNext>(
		Action<ValidatorBuilder<TNext>> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null)
		where TNext : class, T;

	TBuilder ForChildImplementation<TNext>(
		Action<ValidatorBuilder<TNext>, Dictionary<string, object>?, Dictionary<string, object>?> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
		where TNext : class, T;

	TBuilder Custom(Func<T?, AbstractValidator<T>, IValidationResult> validationFunction);

	TBuilder Custom(Func<T?, AbstractValidator<T>, IValidationResult> validationFunction, Func<IObjectPath<T>, IObjectPath>? objectPathBuilder);

	TBuilder WithError(Func<T?, ParentInstance?, bool> serverCondition, Func<T?, IErrorCode> errorCode, Func<T?, string>? failureInfoFunc = null);

	TBuilder WithError(Func<T?, ParentInstance?, bool> serverCondition, Func<T?, string> errorMessage, Func<T?, string>? failureInfoFunc = null);

	TBuilder WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, IValidationResult> validationFunction,
		Func<T?, string>? failureInfoFunc = null);

	TBuilder WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> serverCondition,
		Func<T?, TProperty?, IErrorCode>? errorCodeGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null);

	TBuilder WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> serverCondition,
		Func<ClientCondition<T>, IClientConditionDefinition>? clientCondition,
		Func<T?, TProperty?, IErrorCode>? errorCodeGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null);

	TBuilder WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> serverCondition,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null);

	TBuilder WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> serverCondition,
		Func<ClientCondition<T>, IClientConditionDefinition>? clientCondition,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null);
}

public abstract class ValidatorBuilderBase<TBuilder, T> : IValidatorBuilder<TBuilder, T>, IValidatorDescriptorBuilder, IValidatorFactory<T>
	where TBuilder : ValidatorBuilderBase<TBuilder, T>
{
	protected readonly TBuilder _builder;
	protected Validator<T> _validator;

	public Type ObjectType { get; } = typeof(T);

	protected ValidatorBuilderBase(Validator<T> validator)
	{
		_validator = validator;
		_builder = (TBuilder)this;
	}

	public virtual TBuilder Object(Validator<T> validator)
	{
		_validator = validator;
		return _builder;
	}

	public IValidator<T> Build()
		=> _validator;

	public TBuilder ForProperty<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Action<PropertyValidator<T, TProperty>> propertyValidatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<ClientCondition<T>, IClientConditionDefinition>? clientCondition = null,
		Func<T?, string>? failureInfoFunc = null)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(propertyValidatorBuilder);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp).AddProperty(expression);

		var cc = new ClientCondition<T>();
		var clientConditionDefinition = clientCondition?.Invoke(cc);

		var propertyValidator = new PropertyValidator<T, TProperty>(ValidatorType.PropertyValidator, PropertyAccessor.GetCachedAccessor(expression), newObjectPath, serverCondition, clientConditionDefinition, failureInfoFunc, null, null);

		((IValidator)_validator).AddValidatorInternal(propertyValidator);
		propertyValidatorBuilder.Invoke(propertyValidator);

		return _builder;
	}

	public TBuilder ForNavigation<TNavigation>(
		Expression<Func<T, TNavigation>> expression,
		Action<ValidatorBuilder<TNavigation>> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(validatorBuilder);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp).AddNavigation(expression);

		var navigationValidator = new NavigationValidator<T, TNavigation>(PropertyAccessor.GetCachedAccessor(expression), newObjectPath, serverCondition, failureInfoFunc);

		((IValidator)_validator).AddValidatorInternal(navigationValidator);
		validatorBuilder.Invoke(navigationValidator!);
		return _builder;
	}

	public TBuilder ForNavigation<TNavigation>(
		Expression<Func<T, TNavigation?>> expression,
		Action<ValidatorBuilder<TNavigation>, Dictionary<string, object>?, Dictionary<string, object>?> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(validatorBuilder);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp).AddNavigation(expression);

		var navigationValidator = new NavigationValidator<T, TNavigation?>(PropertyAccessor.GetCachedAccessor(expression), newObjectPath, serverCondition, failureInfoFunc);

		((IValidator)_validator).AddValidatorInternal(navigationValidator);
		validatorBuilder.Invoke(navigationValidator!, globalValidationState, localValidationState);
		return _builder;
	}

	public TBuilder ForEach<TItem>(
		Expression<Func<T, IEnumerable<TItem>>> expression,
		Action<ValidatorBuilder<TItem>> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? detailInfoFunc = null)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(validatorBuilder);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp).AddEnumerable(expression);

		var enumerableValidator = new EnumerableValidator<T, TItem>(PropertyAccessor.GetCachedAccessor(expression), newObjectPath, serverCondition, detailInfoFunc);

		((IValidator)_validator).AddValidatorInternal(enumerableValidator);
		validatorBuilder.Invoke(enumerableValidator!);
		return _builder;
	}

	public TBuilder ForEach<TItem>(
		Expression<Func<T, IEnumerable<TItem>?>> expression,
		Action<ValidatorBuilder<TItem>, Dictionary<string, object>?, Dictionary<string, object>?> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? detailInfoFunc = null,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(validatorBuilder);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp).AddEnumerable(expression);

		var enumerableValidator = new EnumerableValidator<T, TItem>(PropertyAccessor.GetCachedAccessor(expression), newObjectPath, serverCondition, detailInfoFunc);

		((IValidator)_validator).AddValidatorInternal(enumerableValidator);
		validatorBuilder.Invoke(enumerableValidator!, globalValidationState, localValidationState);
		return _builder;
	}

	public TBuilder ForChildInheritance<TChild>(
		Action<ValidatorBuilder<TChild>> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null)
		where TChild : class, T
	{
		Throw.IfArgumentNull(validatorBuilder);

		_validator.ObjectPath.SetChildInheritance<TChild>();
		var inherImplValidator = new InherImplValidator<T, TChild>(_validator.ObjectPath, serverCondition, failureInfoFunc);

		((IValidator)_validator).AddValidatorInternal(inherImplValidator);
		validatorBuilder.Invoke(inherImplValidator!);
		return _builder;
	}

	public TBuilder ForChildInheritance<TChild>(
		Action<ValidatorBuilder<TChild>, Dictionary<string, object>?, Dictionary<string, object>?> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
		where TChild : class, T
	{
		Throw.IfArgumentNull(validatorBuilder);

		_validator.ObjectPath.SetChildInheritance<TChild>();
		var inherImplValidator = new InherImplValidator<T, TChild>(_validator.ObjectPath, serverCondition, failureInfoFunc);

		((IValidator)_validator).AddValidatorInternal(inherImplValidator);
		validatorBuilder.Invoke(inherImplValidator!, globalValidationState, localValidationState);
		return _builder;
	}

	public TBuilder ForChildImplementation<TChild>(
		Action<ValidatorBuilder<TChild>> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null)
		where TChild : class, T
	{
		Throw.IfArgumentNull(validatorBuilder);

		_validator.ObjectPath.SetChildImplementation<TChild>();
		var inherImplValidator = new InherImplValidator<T, TChild>(_validator.ObjectPath, serverCondition, failureInfoFunc);

		((IValidator)_validator).AddValidatorInternal(inherImplValidator);
		validatorBuilder.Invoke(inherImplValidator!);
		return _builder;
	}

	public TBuilder ForChildImplementation<TChild>(
		Action<ValidatorBuilder<TChild>, Dictionary<string, object>?, Dictionary<string, object>?> validatorBuilder,
		Func<T?, ParentInstance?, bool>? serverCondition = null,
		Func<T?, string>? failureInfoFunc = null,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
		where TChild : class, T
	{
		Throw.IfArgumentNull(validatorBuilder);

		_validator.ObjectPath.SetChildImplementation<TChild>();
		var inherImplValidator = new InherImplValidator<T, TChild>(_validator.ObjectPath, serverCondition, failureInfoFunc);

		((IValidator)_validator).AddValidatorInternal(inherImplValidator);
		validatorBuilder.Invoke(inherImplValidator!, globalValidationState, localValidationState);
		return _builder;
	}

	public TBuilder Custom(Func<T?, AbstractValidator<T>, IValidationResult> validationFunction)
	{
		Throw.IfArgumentNull(validationFunction);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp);

		var validator = new AbstractValidator<T>(newObjectPath, validationFunction);
		((IValidator)_validator).AddValidatorInternal(validator);

		return _builder;
	}

	public TBuilder Custom(Func<T?, AbstractValidator<T>, IValidationResult> validationFunction, Func<IObjectPath<T>, IObjectPath>? objectPathBuilder)
	{
		Throw.IfArgumentNull(validationFunction);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp);
		newObjectPath = objectPathBuilder?.Invoke(newObjectPath.ToGenericObjectPath<T>());

		Throw.IfNull(newObjectPath);

		var validator = new AbstractValidator<T>(newObjectPath, validationFunction);
		((IValidator)_validator).AddValidatorInternal(validator);

		return _builder;
	}

	public TBuilder WithError(Func<T?, ParentInstance?, bool> serverCondition, Func<T?, IErrorCode> errorCode, Func<T?, string>? failureInfoFunc = null)
	{
		Throw.IfArgumentNull(serverCondition);
		Throw.IfArgumentNull(errorCode);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp);

		var validator = new ErrorValidator<T>(newObjectPath, serverCondition, errorCode, failureInfoFunc);
		((IValidator)_validator).AddValidatorInternal(validator);

		return _builder;
	}

	public TBuilder WithError(Func<T?, ParentInstance?, bool> serverCondition, Func<T?, string> errorMessage, Func<T?, string>? failureInfoFunc = null)
	{
		Throw.IfArgumentNull(serverCondition);
		Throw.IfArgumentNull(errorMessage);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp);

		var validator = new ErrorValidator<T>(newObjectPath, serverCondition, errorMessage, failureInfoFunc);
		((IValidator)_validator).AddValidatorInternal(validator);

		return _builder;
	}

	public TBuilder WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, IValidationResult> validationFunction,
		Func<T?, string>? failureInfoFunc = null)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(validationFunction);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp).AddProperty(expression);

		var cc = new ClientCondition<T>();

		var errorValidator =
			new Validators.PropertyValidators.ErrorValidator<T, TProperty>(
				PropertyAccessor.GetCachedAccessor(expression),
				newObjectPath,
				validationFunction,
				failureInfoFunc);

		((IValidator)_validator).AddValidatorInternal(errorValidator);

		return _builder;
	}

	public TBuilder WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> serverCondition,
		Func<T?, TProperty?, IErrorCode>? errorCodeGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null)
		=> WithPropertyError(expression, serverCondition, null, errorCodeGetter, propertyDisplayNameGetter, failureInfoFunc);

	public TBuilder WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> serverCondition,
		Func<ClientCondition<T>, IClientConditionDefinition>? clientCondition,
		Func<T?, TProperty?, IErrorCode>? errorCodeGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(serverCondition);
		Throw.IfArgumentNull(errorCodeGetter);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp).AddProperty(expression);

		var cc = new ClientCondition<T>();
		var clientConditionDefinition = clientCondition?.Invoke(cc);

		var errorValidator =
			new Validators.PropertyValidators.ErrorValidator<T, TProperty>(
				PropertyAccessor.GetCachedAccessor(expression),
				newObjectPath,
				serverCondition,
				clientConditionDefinition,
				failureInfoFunc,
				errorCodeGetter,
				propertyDisplayNameGetter);

		((IValidator)_validator).AddValidatorInternal(errorValidator);

		return _builder;
	}

	public TBuilder WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> serverCondition,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null)
		=> WithPropertyError(expression, serverCondition, null, messageGetter, propertyDisplayNameGetter, failureInfoFunc);

	public TBuilder WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> serverCondition,
		Func<ClientCondition<T>, IClientConditionDefinition>? clientCondition,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null)
	{
		Throw.IfArgumentNull(expression);
		Throw.IfArgumentNull(serverCondition);
		Throw.IfArgumentNull(messageGetter);

		var newObjectPath = _validator.ObjectPath.Clone(ObjectPathCloneMode.BottomUp).AddProperty(expression);

		var cc = new ClientCondition<T>();
		var clientConditionDefinition = clientCondition?.Invoke(cc);

		var errorValidator =
			new Validators.PropertyValidators.ErrorValidator<T, TProperty>(
				PropertyAccessor.GetCachedAccessor(expression),
				newObjectPath,
				serverCondition,
				clientConditionDefinition,
				failureInfoFunc,
				messageGetter,
				propertyDisplayNameGetter);

		((IValidator)_validator).AddValidatorInternal(errorValidator);

		return _builder;
	}

	public virtual IValidatorDescriptor ToDescriptor(IServiceProvider serviceProvider)
		=> Build().ToDescriptor();

	public virtual IValidatorDescriptor ToDescriptor(IServiceProvider serviceProvider, object? state = null)
		=> Build().ToDescriptor();
}

public class ValidatorBuilder<T> : ValidatorBuilderBase<ValidatorBuilder<T>, T>, IValidatorBuilder<ValidatorBuilder<T>, T>, IValidatorDescriptorBuilder, IValidatorFactory<T>, IValidatorBuilderFactory<T>
{
	public ValidatorBuilder()
		: base(new Validator<T>(ValidatorType.Validator, ObjectPath<T>.Create(), hasServerCondition: false, clientConditionDefinition: null, nestedValidators: null))
	{
	}

	internal ValidatorBuilder(List<Validator> nestedValidators)
		: base(new Validator<T>(ValidatorType.Validator, ObjectPath<T>.Create(), hasServerCondition: false, clientConditionDefinition: null, nestedValidators))
	{
	}

	internal protected ValidatorBuilder(Validator<T> validator)
		: base(validator)
	{
	}

	public ValidatorBuilder<T> GetBuilder()
		=> this;

	public static implicit operator Validator<T>?(ValidatorBuilder<T> builder)
	{
		if (builder == null)
			return null;

		return builder._validator;
	}

	public static implicit operator ValidatorBuilder<T>?(Validator<T> validator)
	{
		if (validator == null)
			return null;

		return new ValidatorBuilder<T>(validator);
	}
}
//#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
