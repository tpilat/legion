namespace Legion.Reflection.ObjectPaths;

public interface IEnumerableObjectPath : IObjectPath
{
}

public class EnumerableObjectPath<T> : NavigationObjectPath<T>, IEnumerableObjectPath, IObjectPath
{
	internal EnumerableObjectPath(Guid id)
		: base(id)
	{
	}

	internal EnumerableObjectPath(ObjectPath parent, string propertyName)
		: base(parent, propertyName)
	{
	}

	protected internal override ObjectPath<T> CloneSelf()
		=> new EnumerableObjectPath<T>(Id)
		{
			Parent = null,
			PropertyName = PropertyName,
			Descendant = null,
			Depth = Depth,
			Index = null
		};
}
