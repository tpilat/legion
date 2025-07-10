using Legion.ExpressionsSerializer.Interfaces;
using Legion.ExpressionsSerializer.Internals;
using System.Reflection;

namespace Legion.ExpressionsSerializer;

public class ExpressionContext : ExpressionContextBase
{
	private readonly IAssemblyLoader _assemblyLoader;

	public ExpressionContext()
		: this(new DefaultAssemblyLoader()) { }

	public ExpressionContext(IAssemblyLoader assemblyLoader)
	{
		_assemblyLoader = assemblyLoader
			?? throw new ArgumentNullException(nameof(assemblyLoader));
	}

	protected override IEnumerable<Assembly> GetAssemblies()
	{
		return _assemblyLoader.GetAssemblies();
	}
}
