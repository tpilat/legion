using Legion.Razor.UI.Routing;

namespace Legion.ADF.Logs.UI;

public class RouteConstants : RoutesCollectionBase, IRoutesCollection
{
	public const string _rootPath = "/LegionLogsUI";
	public const string MyComponent1Route = $"{_rootPath}/component1";

	public override string RootPath => _rootPath;

	public override IEnumerable<string> GetAllRoutes()
	{
		yield return MyComponent1Route;
	}
}

