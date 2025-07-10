using Legion.ExpressionsSerializer.Interfaces;
using System.Reflection;

namespace Legion.ExpressionsSerializer.Internals;

internal class DefaultAssemblyLoader : IAssemblyLoader
{
	public IEnumerable<Assembly> GetAssemblies()
	{
#if NETSTANDARD1_3 || UAP10_0
            throw new NotSupportedException(
                "Please provide a custom implemention for the IAssemblyLoader, with `ExpressionExtensions.AssemblyLoader = new MyCustomLoader();`, to retrieve assemblies that have been loaded into the execution context of this application domain.\r\n" +
                "You could use the NuGet package 'System.AppDomain.NetCoreApp' which mimics the AppDomain."
            );
#else
		return AppDomain.CurrentDomain.GetAssemblies();
#endif
	}
}
