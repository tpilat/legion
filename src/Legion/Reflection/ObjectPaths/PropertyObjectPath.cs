namespace Legion.Reflection.ObjectPaths;

public interface IPropertyObjectPath : IObjectPath
{
}

public class PropertyObjectPath<T> : ObjectPath<T>, IPropertyObjectPath, IObjectPath
{
	internal PropertyObjectPath(Guid id)
		: base()
	{
		Id = id;
	}

	internal PropertyObjectPath(ObjectPath parent, string propertyName)
		: this(GlobalContext.Instance.NewGuid())
	{
		Throw.IfArgumentNull(parent);

		if (string.IsNullOrWhiteSpace(propertyName))
			throw new ArgumentNullException(nameof(propertyName));

		PropertyName = propertyName;
		Descendant = null;
		parent.SetDescendant(this, propertyName);
	}

	protected internal override ObjectPath<T> CloneSelf()
		=> new PropertyObjectPath<T>(Id)
		{
			Parent = null,
			PropertyName = PropertyName,
			Descendant = null,
			Depth = Depth,
			Index = null
		};
}
