using System.Linq.Expressions;

namespace Legion.Reflection.ObjectPaths;

public interface IObjectPath
{
	Guid Id { get; }
	Type ObjectType { get; }
	IObjectPath? Parent { get; }
	string? PropertyName { get; }
	IObjectPath? Descendant { get; }
	int Depth { get; }
	int? Index { get; set; }

	IObjectPath GetRoot();
	IObjectPath GetLastDescendant();
	List<IObjectPath> GetObjectPath();
	IObjectPath Clone(ObjectPathCloneMode mode);

	void SetDescendant(IObjectPath descendant, string propertyName, bool force = true);

	PropertyObjectPath<TProperty> AddProperty<T, TProperty>(Expression<Func<T, TProperty>> expression);

	PropertyObjectPath<TProperty> AddProperty<TProperty>(string propertyName);

	NavigationObjectPath<TNavigation> AddNavigation<T, TNavigation>(Expression<Func<T, TNavigation>> expression);

	NavigationObjectPath<TNavigation> AddNavigation<TNavigation>(string navigationName);

	EnumerableObjectPath<TItem> AddEnumerable<T, TItem>(Expression<Func<T, IEnumerable<TItem>?>> expression);

	EnumerableObjectPath<TItem> AddEnumerable<TItem>(string enumerableName);

	IObjectPath<T> ToGenericObjectPath<T>();

	IObjectPath CloneAndSetIndexes(ObjectPathCloneMode mode, Dictionary<int, int>? objectPathIndexes);

	string GetParentPath(Dictionary<int, int>? objectPathIndexes);

	void SetChildInheritance<TNext>();

	void SetChildImplementation<TNext>();
}

public interface IObjectPath<T> : IObjectPath
{
	PropertyObjectPath<TProperty> AddProperty<TProperty>(Expression<Func<T, TProperty>> expression);

	NavigationObjectPath<TNavigation> AddNavigation<TNavigation>(Expression<Func<T, TNavigation>> expression);

	EnumerableObjectPath<TItem> AddEnumerable<TItem>(Expression<Func<T, IEnumerable<TItem>?>> expression);

	new IObjectPath<T> Clone(ObjectPathCloneMode mode);
}
