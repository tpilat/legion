using System.Reflection;

namespace Legion.Reflection;

public class ObjectWrapper<T> : IObjectWrapper
{
	private T? _currentInstance;

	public TypeWrapper<T> TypeWrapper { get; }

	ITypeWrapper IObjectWrapper.TypeWrapper => TypeWrapper;

	public object? this[string propertyFieldName]
	{
		get { return GetValueInternal(propertyFieldName); }
		set { SetValueInternal(propertyFieldName, value); }
	}

	public ObjectWrapper()
		: this(
			null,
			default,
			BindingFlags.Public |
			BindingFlags.NonPublic |
			BindingFlags.Static |
			BindingFlags.Instance |
			BindingFlags.FlattenHierarchy)
	{
	}

	public ObjectWrapper(T? instance)
		: this(
			null,
			instance,
			BindingFlags.Public |
			BindingFlags.NonPublic |
			BindingFlags.Static |
			BindingFlags.Instance |
			BindingFlags.FlattenHierarchy)
	{
	}

	public ObjectWrapper(BindingFlags bindingFlags)
		: this(null, default, bindingFlags)
	{
	}

	public ObjectWrapper(T? instance, BindingFlags bindingFlags)
		: this(null, instance, bindingFlags)
	{
	}

	internal ObjectWrapper(TypeWrapper<T>? typeManager, BindingFlags bindingFlags)
		: this(typeManager, default, bindingFlags)
	{
	}

	internal ObjectWrapper(TypeWrapper<T>? typeManager, T? instance, BindingFlags bindingFlags)
	{
		TypeWrapper = typeManager ?? TypeWrapper<T>.Create(bindingFlags);
		_currentInstance = instance;
	}

	public ObjectWrapper<T> SetInstance(T? instance)
	{
		_currentInstance = instance;
		return this;
	}

	public object? GetValue(string memberName)
		=> GetValueInternal(memberName);

	public object? GetNonStaticValue(string memberName)
	{
		Throw.IfArgumentNullOrWhiteSpace(memberName);

		if (_currentInstance == null)
			throw new InvalidOperationException("No instance was set.");

		if (TypeWrapper.Getters.TryGetValue(memberName, out Func<T?, object?>? getter))
			return getter(_currentInstance);

		throw new InvalidOperationException($"No getter for {memberName} was found.");
	}

	public object? GetStaticValue(string memberName)
	{
		Throw.IfArgumentNullOrWhiteSpace(memberName);

		if (TypeWrapper.StaticGetters.TryGetValue(memberName, out Func<T?, object?>? staticGetter))
			return staticGetter(default);

		throw new InvalidOperationException($"No static getter for {memberName} was found.");
	}

	private object? GetValueInternal(string memberName)
	{
		Throw.IfArgumentNullOrWhiteSpace(memberName);

		if (TypeWrapper.StaticGetters.TryGetValue(memberName, out Func<T?, object?>? staticGetter))
			return staticGetter(default);

		if (_currentInstance == null)
			throw new InvalidOperationException("No instance was set.");

		if (TypeWrapper.Getters.TryGetValue(memberName, out Func<T?, object?>? getter))
			return getter(_currentInstance);

		throw new InvalidOperationException($"No getter for {memberName} was found.");
	}

	public void SetValue(string memberName, object? value)
		=> SetValueInternal(memberName, value);

	public void SetNonStaticValue(string memberName, object? value)
	{
		Throw.IfArgumentNullOrWhiteSpace(memberName);

		if (_currentInstance == null)
			throw new InvalidOperationException("No instance was set.");

		if (TypeWrapper.Setters.TryGetValue(memberName, out Action<T?, object?>? setter))
		{
			setter(_currentInstance, value);
			return;
		}

		throw new InvalidOperationException($"No setter for {memberName} was found.");
	}

	public void SetStaticValue(string memberName, object? value)
	{
		Throw.IfArgumentNullOrWhiteSpace(memberName);

		if (TypeWrapper.StaticSetters.TryGetValue(memberName, out Action<T?, object?>? staticSetter))
		{
			staticSetter(default, value);
			return;
		}

		throw new InvalidOperationException($"No static setter for {memberName} was found.");
	}

	private void SetValueInternal(string memberName, object? value)
	{
		Throw.IfArgumentNullOrWhiteSpace(memberName);

		if (TypeWrapper.StaticSetters.TryGetValue(memberName, out Action<T?, object?>? staticSetter))
		{
			staticSetter(default, value);
			return;
		}

		if (_currentInstance == null)
			throw new InvalidOperationException("No instance was set.");

		if (TypeWrapper.Setters.TryGetValue(memberName, out Action<T?, object?>? setter))
		{
			setter(_currentInstance, value);
			return;
		}

		throw new InvalidOperationException($"No setter for {memberName} was found.");
	}
}
