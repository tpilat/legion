using Legion.Validation.Client;
using Legion.Validation.Internal;
using System.Linq.Expressions;

namespace Legion.Validation;

public static class ValidatorFactory
{
	public static IValidator<T> Create<T>(Action<ValidatorBuilder<T>> configure)
	{
		Throw.IfArgumentNull(configure);

		var builder = new ValidatorBuilder<T>();
		configure.Invoke(builder);
		var validator = builder.Build();
		return validator;
	}

	public static IValidator<T> WithError<T>(Func<T?, ParentInstance?, bool> condition, Func<T?, string> errorMessage, Func<T?, string>? failureInfoFunc = null)
		=> new ValidatorBuilder<T>()
			.WithError(condition, errorMessage, failureInfoFunc)
			.Build();

	public static IValidator<T> WithPropertyError<T, TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> condition,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null)
		=> new ValidatorBuilder<T>()
			.WithPropertyError(expression, condition, null, messageGetter, propertyDisplayNameGetter, failureInfoFunc)
			.Build();

	public static IValidator<T> WithPropertyError<T, TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> condition,
		Func<ClientCondition<T>, IClientConditionDefinition>? clientCondition,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null)
		=> new ValidatorBuilder<T>()
			.WithPropertyError(expression, condition, clientCondition, messageGetter, propertyDisplayNameGetter, failureInfoFunc)
			.Build();
}

public static class ValidatorFactory<T>
{
	public static IValidator<T> Create(Action<ValidatorBuilder<T>> configure)
	{
		Throw.IfArgumentNull(configure);

		var builder = new ValidatorBuilder<T>();
		configure.Invoke(builder);
		var validator = builder.Build();
		return validator;
	}

	public static IValidator<T> WithError(Func<T?, ParentInstance?, bool> condition, Func<T?, string> errorMessage, Func<T?, string>? failureInfoFunc = null)
		=> new ValidatorBuilder<T>()
			.WithError(condition, errorMessage, failureInfoFunc)
			.Build();

	public static IValidator<T> WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> condition,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null)
		=> new ValidatorBuilder<T>()
			.WithPropertyError(expression, condition, null, messageGetter, propertyDisplayNameGetter, failureInfoFunc)
			.Build();

	public static IValidator<T> WithPropertyError<TProperty>(
		Expression<Func<T, TProperty>> expression,
		Func<T?, ParentInstance?, bool> condition,
		Func<ClientCondition<T>, IClientConditionDefinition>? clientCondition,
		Func<T?, TProperty?, string, string?>? messageGetter,
		Func<string>? propertyDisplayNameGetter = null,
		Func<T?, string>? failureInfoFunc = null)
		=> new ValidatorBuilder<T>()
			.WithPropertyError(expression, condition, clientCondition, messageGetter, propertyDisplayNameGetter, failureInfoFunc)
			.Build();
}
