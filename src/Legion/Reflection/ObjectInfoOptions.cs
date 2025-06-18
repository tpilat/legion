using System.Reflection;

namespace Legion.Reflection;

public class ObjectInfoOptions
{
	public BindingFlags BindingFlags { get; set; } =
		BindingFlags.Public |
		BindingFlags.NonPublic |
		BindingFlags.Static |
		BindingFlags.Instance |
		BindingFlags.FlattenHierarchy;

	public bool ReadIndexers { get; set; }
	public bool ReadEvents { get; set; }
}
