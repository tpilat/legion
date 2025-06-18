using Legion.Exceptions.Internal;

namespace Legion.Validation.Internal;

internal class ValidatorManager : IValidatorManager
{
	private readonly Dictionary<Type, Dictionary<Type, IValidatorDescriptorBuilder>> _descriptorsRegister = []; //Dictionary<TDto, Dictionary<TCommand, IValidatorDescriptorBuilder>>

	public Dictionary<Type, IValidatorDescriptorBuilder>? GetValidatorDescriptorBuilderFor<T>()
		=> GetValidatorDescriptorBuilderFor(typeof(T));

	public Dictionary<Type, IValidatorDescriptorBuilder>? GetValidatorDescriptorBuilderFor(Type objectType)
	{
		if (!_descriptorsRegister.TryGetValue(objectType, out Dictionary<Type, IValidatorDescriptorBuilder>? commandDescriptorBuilders))
			return null;

		return commandDescriptorBuilders.ToDictionary(k => k.Key, v => v.Value);
	}

	public IValidatorDescriptorBuilder? GetValidatorDescriptorBuilderFor<T, TCommand>()
		=> GetValidatorDescriptorBuilderFor(typeof(T), typeof(TCommand));

	public IValidatorDescriptorBuilder? GetValidatorDescriptorBuilderFor(Type objectType, Type commandType)
	{
		if (!_descriptorsRegister.TryGetValue(objectType, out Dictionary<Type, IValidatorDescriptorBuilder>? commandValidatorDescriptorBuilders))
			return null;

		if (!commandValidatorDescriptorBuilders.TryGetValue(commandType, out IValidatorDescriptorBuilder? builder))
			return null;

		return builder;
	}

	public bool RegisterValidatorDescriptorFor<T, TCommand>(IValidatorDescriptorBuilder builder)
		=> RegisterValidatorDescriptorFor(typeof(T), typeof(TCommand), builder);

	public bool RegisterValidatorDescriptorFor(Type objectType, Type commandType, IValidatorDescriptorBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		if (!_descriptorsRegister.TryGetValue(objectType, out Dictionary<Type, IValidatorDescriptorBuilder>? commandValidatorDescriptorBuilders))
		{
			commandValidatorDescriptorBuilders = [];
			_descriptorsRegister.Add(objectType, commandValidatorDescriptorBuilders);
		}

		commandValidatorDescriptorBuilders[commandType] = builder;

		return true;
	}

	public IValidatorDescriptor? GetValidatorDescriptorFor(Type objectType, Type commandType, IServiceProvider serviceProvider, IScopeContext scopeContext, object? state = null)
	{
		var validatorDescriptorBuilder = GetValidatorDescriptorBuilderFor(objectType, commandType);
		if (validatorDescriptorBuilder == null)
			throw new InvalidOperationException($"No {nameof(IValidatorDescriptorBuilder)} found for <{objectType?.FullName ?? "NULL"}, {commandType?.FullName ?? "NULL"}>");

		IValidatorDescriptor? descriptor = null;
		try
		{
			descriptor = validatorDescriptorBuilder.ToDescriptor(serviceProvider);
		}
		catch (Exception ex)
		{
			Throw.ConfigurationException(ErrorCodes.ValidationException.ValidationMessage($"Cannot create {nameof(IValidatorDescriptor)} for <{objectType?.FullName ?? "NULL"}, {commandType?.FullName ?? "NULL"}>"), scopeContext, innerException: ex);
		}

		if (descriptor == null)
			throw new InvalidOperationException($"{nameof(validatorDescriptorBuilder.ToDescriptor)} for <{objectType?.FullName ?? "NULL"}, {commandType?.FullName ?? "NULL"}> returns null.");

		return descriptor;
	}
}
