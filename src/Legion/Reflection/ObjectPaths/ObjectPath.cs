using Legion.Extensions;
using System.Linq.Expressions;

namespace Legion.Reflection.ObjectPaths;

public abstract class ObjectPath : IObjectPath
{
	public Guid Id { get; protected set; }
	public Type ObjectType { get; internal set; }
	public ObjectPath? Parent { get; protected set; }
	public string? PropertyName { get; protected set; }
	public ObjectPath? Descendant { get; protected set; }
	public int Depth { get; protected set; }
	public int? Index { get; set; }

	IObjectPath? IObjectPath.Parent => Parent;
	IObjectPath? IObjectPath.Descendant => Descendant;

	internal protected ObjectPath(Type objectType)
	{
		Throw.IfArgumentNull(objectType);

		ObjectType = objectType;
	}

	public static ObjectPathRoot<T> Create<T>()
		=> new();

	internal List<ObjectPath> GetParentsBottomUpPath()
		=> GetParentsPath([]);

	private List<ObjectPath> GetParentsPath(List<ObjectPath> path)
	{
		if (Parent == null)
			return path;

		path.Add(Parent);
		return Parent.GetParentsPath(path);
	}

	public IObjectPath GetRoot()
		=> GetRootInternal();

	private ObjectPath GetRootInternal()
		=> Parent != null
			? Parent.GetRootInternal()
			: this;

	public IObjectPath GetLastDescendant()
		=> GetLastDescendantInternal();

	private ObjectPath GetLastDescendantInternal()
		=> Descendant != null
			? Descendant.GetLastDescendantInternal()
			: this;

	public List<IObjectPath> GetObjectPath()
	{
		var path = new List<IObjectPath>();
		GetObjectPathInternal(path);
		path.Reverse();
		return path;
	}

	private void GetObjectPathInternal(List<IObjectPath> path)
	{
		path.Add(this);
		Parent?.GetObjectPathInternal(path);
	}

	public void SetDescendant(IObjectPath descendant, string propertyName, bool force = true)
	{
		Throw.IfArgumentNull(descendant);

		if (descendant is not ObjectPath descendantObjectPath)
			throw new ArgumentException($"{nameof(descendant)} must by type of {typeof(ObjectPath).FullName}", nameof(descendant));

		if (string.IsNullOrWhiteSpace(propertyName))
			throw new ArgumentNullException(nameof(propertyName));

		if (this is IPropertyObjectPath)
			throw new NotSupportedException($"{nameof(IPropertyObjectPath)} cannot have {nameof(Descendant)}");

		if (force || Descendant == null)
		{
			descendantObjectPath.PropertyName = propertyName;
			descendantObjectPath.IncreaseDepth(Depth + 1, []);
			descendantObjectPath.Parent = this;
			Descendant = descendantObjectPath;
		}
	}

	private void IncreaseDepth(int delta, HashSet<Guid> usedGuids)
	{
		if (!usedGuids.Add(Id))
			return;

		Depth = delta;

		if (Descendant == null)
			return;

		Descendant.IncreaseDepth(Depth + 1, usedGuids);
	}

	protected internal abstract ObjectPath CloneBase();

	public IObjectPath Clone(ObjectPathCloneMode mode)
	{
		var selfClone = CloneBase();

		if (mode == ObjectPathCloneMode.BottomUp && Parent != null)
		{
			var clonedParent = Parent.Clone(ObjectPathCloneMode.BottomUp);
			if (clonedParent != null && clonedParent is ObjectPath clonedParentObjectPath)
				clonedParentObjectPath.SetDescendant(selfClone, selfClone.PropertyName!);
			else
				throw new InvalidOperationException($"Cannot clone {this.GetType().FullName}");
		}

		return selfClone;
	}

	public PropertyObjectPath<TProperty> AddProperty<T, TProperty>(Expression<Func<T, TProperty>> expression)
	{
		Throw.IfArgumentNull(expression);

		var propertyName = expression.GetMemberName() ?? throw new ArgumentException($"propertyName == null", nameof(expression));

		var propertyObjectPath = new PropertyObjectPath<TProperty>(this, propertyName);
		return propertyObjectPath;
	}

	public PropertyObjectPath<TProperty> AddProperty<TProperty>(string propertyName)
	{
		Throw.IfArgumentNullOrWhiteSpace(propertyName);

		var propertyObjectPath = new PropertyObjectPath<TProperty>(this, propertyName);
		return propertyObjectPath;
	}

	public NavigationObjectPath<TNavigation> AddNavigation<T, TNavigation>(Expression<Func<T, TNavigation>> expression)
	{
		Throw.IfArgumentNull(expression);

		var navigationName = expression.GetMemberName() ?? throw new ArgumentException($"navigationName == null", nameof(expression));

		var navigationObjectPath = new NavigationObjectPath<TNavigation>(this, navigationName);
		return navigationObjectPath;
	}

	public NavigationObjectPath<TNavigation> AddNavigation<TNavigation>(string navigationName)
	{
		Throw.IfArgumentNullOrWhiteSpace(navigationName);

		var navigationObjectPath = new NavigationObjectPath<TNavigation>(this, navigationName);
		return navigationObjectPath;
	}

	public EnumerableObjectPath<TItem> AddEnumerable<T, TItem>(Expression<Func<T, IEnumerable<TItem>?>> expression)
	{
		Throw.IfArgumentNull(expression);

		var enumerableName = expression.GetMemberName() ?? throw new ArgumentException($"enumerableName == null", nameof(expression));

		var enumerableObjectPath = new EnumerableObjectPath<TItem>(this, enumerableName);
		return enumerableObjectPath;
	}

	public EnumerableObjectPath<TItem> AddEnumerable<TItem>(string enumerableName)
	{
		Throw.IfArgumentNullOrWhiteSpace(enumerableName);

		var enumerableObjectPath = new EnumerableObjectPath<TItem>(this, enumerableName);
		return enumerableObjectPath;
	}

	public override string? ToString()
		=> string.Join(".", GetObjectPath().Select(x => !string.IsNullOrWhiteSpace(x.PropertyName)
			? (!x.Index.HasValue ? x.PropertyName : $"{x.PropertyName}[{x.Index}]")
			: "_"));

	public virtual IObjectPath<T> ToGenericObjectPath<T>()
	{
		if (this is PropertyObjectPath<T> propertyObjectPath)
			return propertyObjectPath;

		if (this is NavigationObjectPath<T> navigationObjectPath)
			return navigationObjectPath;

		if (this is EnumerableObjectPath<T> enumerableObjectPath)
			return enumerableObjectPath;

		if (this is ObjectPath<T> objectPath)
			return objectPath;

		return null!;
	}

	public IObjectPath CloneAndSetIndexes(ObjectPathCloneMode mode, Dictionary<int, int>? objectPathIndexes)
	{
		var clone = Clone(mode);

		if (0 < objectPathIndexes?.Count)
		{
			var currentObjectPath = clone;
			while (currentObjectPath != null)
			{
				if (objectPathIndexes.TryGetValue(currentObjectPath.Depth, out var index))
					currentObjectPath.Index = index;

				currentObjectPath = currentObjectPath.Parent;
			}
		}

		return clone;
	}

	public string GetParentPath(Dictionary<int, int>? objectPathIndexes)
	{
		var clone = Clone(ObjectPathCloneMode.BottomUp);

		if (0 < objectPathIndexes?.Count)
		{
			var currentObjectPath = clone;
			while (currentObjectPath != null)
			{
				if (objectPathIndexes.TryGetValue(currentObjectPath.Depth, out var index))
					currentObjectPath.Index = index;

				currentObjectPath = currentObjectPath.Parent;
			}
		}

		var path = clone.ToString()!;

		if (-1 < path.IndexOf("."))
			path = path.Substring(0, path.LastIndexOf("."));

		return path;
	}

	public void SetChildInheritance<TNext>()
	{
		var newObjectType = typeof(TNext);

		if (newObjectType?.Inherits(ObjectType) != true)
			Throw.InvalidOperationException($"Type {newObjectType?.FullName} must inherit from {ObjectType?.FullName}");

		ObjectType = newObjectType;
	}

	public void SetChildImplementation<TNext>()
	{
		var newObjectType = typeof(TNext);

		if (newObjectType?.Implements(ObjectType) != true)
			Throw.InvalidOperationException($"Type {newObjectType?.FullName} must inherit from {ObjectType?.FullName}");

		ObjectType = newObjectType;
	}
}
