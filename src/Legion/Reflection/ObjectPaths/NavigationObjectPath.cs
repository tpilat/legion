namespace Legion.Reflection.ObjectPaths;

public interface INavigationObjectPath : IObjectPath
{
}

public class NavigationObjectPath<T> : ObjectPath<T>, INavigationObjectPath, IObjectPath
{
	internal NavigationObjectPath(Guid id)
		: base()
	{
		Id = id;
	}

	internal NavigationObjectPath(ObjectPath parent, string propertyName)
		: this(Guid.NewGuid())
	{
		Throw.IfArgumentNull(parent);

		if (string.IsNullOrWhiteSpace(propertyName))
			throw new ArgumentNullException(nameof(propertyName));

		PropertyName = propertyName;
		Descendant = null;
		parent.SetDescendant(this, propertyName);
	}

	protected internal override ObjectPath<T> CloneSelf()
		=> new NavigationObjectPath<T>(Id)
		{
			Parent = null,
			PropertyName = PropertyName,
			Descendant = null,
			Depth = Depth,
			Index = null
		};
}
