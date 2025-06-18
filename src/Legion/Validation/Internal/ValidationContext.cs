namespace Legion.Validation.Internal;

public class ParentInstance
{
	public string Path { get; }
	public object Instance { get; }
	public ParentInstance? Parent { get; private set; }

	public ParentInstance(string path, object instance)
	{
		Throw.IfArgumentNullOrWhiteSpace(path);
		Throw.IfArgumentNull(instance);

		Path = path;
		Instance = instance;
	}

	internal ParentInstance SetParent(ParentInstance? parent)
	{
		if (Parent != null)
			Throw.InvalidOperationException(null, "");

		Parent = parent;

		return this;
	}

	public override string ToString()
	{
		return $"{Path}: {(Instance == null ? "NULL" : "not null instance")}";
	}
}

internal class ValidationContext
{
	public ParentInstance? ParentInstance { get; }
	public Dictionary<int, int> Indexes { get; } //Dictionary<Depth, Index>

	public ValidationContext(Dictionary<int, int>? indexes, ParentInstance? parentInstance)
	{
		Indexes = indexes?.ToDictionary(x => x.Key, x => x.Value) ?? [];
		ParentInstance = parentInstance;
	}
}

internal class ValidationContext<T> : ValidationContext
{
	//private IObjectPath? _objectPath;

	public T? InstanceToValidate { get; }

	public ValidationContext(T? instanceToValidate, Dictionary<int, int>? indexes, ParentInstance? parentInstance = null)
		: base(indexes, parentInstance)
	{
		InstanceToValidate = instanceToValidate;
	}

	public ValidationContext(T? instanceToValidate, ParentInstance instance, ValidationContext? parentValidationContext)
		: this(instanceToValidate, parentValidationContext?.Indexes, instance?.SetParent(parentValidationContext?.ParentInstance))
	{
	}

	//public ValidationContext<T> SetObjectPath(IObjectPath objectPath)
	//{
	//	_objectPath = objectPath ?? throw new ArgumentNullException(nameof(objectPath));

	//	if (string.IsNullOrWhiteSpace(_objectPath.PropertyName))
	//		throw new InvalidOperationException($"{nameof(_objectPath)}.{nameof(_objectPath.PropertyName)} == null");

	//	return this;
	//}

	//public IObjectPath GetObjectPath()
	//	=> _objectPath ?? throw new InvalidOperationException($"{nameof(_objectPath)} == null");
}

internal class ValidationContext<T, TProperty> : ValidationContext<T>
{
	public TProperty? ValueToValidate { get; }

	public ValidationContext(T? instanceToValidate, TProperty? value, ParentInstance instance, ValidationContext? parentValidationContext)
		: base(instanceToValidate, instance, parentValidationContext)
	{
		ValueToValidate = value;
	}

	//public new ValidationContext<T,TProperty> SetObjectPath(IObjectPath objectPath)
	//{
	//	base.SetObjectPath(objectPath);
	//	return this;
	//}
}
