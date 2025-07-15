namespace Legion.Reflection.ObjectPaths;

public interface IObjectPathRoot : IObjectPath
{
}

public class ObjectPathRoot<T> : ObjectPath<T>, IObjectPathRoot, IObjectPath<T>, IObjectPath
{
	internal ObjectPathRoot(Guid id)
		: base()
	{
		Id = id;
	}

	internal ObjectPathRoot()
		: this(GlobalContext.Instance.NewGuid())
	{
		Parent = null;
		PropertyName = null;
		Descendant = null;
		Depth = 0;
		Index = null;
	}

	protected internal override ObjectPath<T> CloneSelf()
		=> new ObjectPathRoot<T>(Id)
		{
			Parent = null,
			PropertyName = null,
			Descendant = null,
			Depth = Depth,
			Index = null
		};
}
