using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;

namespace Legion.Razor.UI;

class DynamicLinqCustomTypeProvider : IDynamicLinkCustomTypeProvider
{
	static readonly HashSet<Type> empty = [];
	public HashSet<Type> GetCustomTypes() => empty;
	public Dictionary<Type, List<MethodInfo>> GetExtensionMethods() => throw new NotSupportedException();
	public Type ResolveType(string typeName) => throw new NotSupportedException();
	public Type ResolveTypeBySimpleName(string simpleTypeName) => throw new NotSupportedException();
	public static ParsingConfig ParsingConfig = new() { CustomTypeProvider = new DynamicLinqCustomTypeProvider() };
}
