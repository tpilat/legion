namespace Legion.Razor.UI.Routing;

public interface IRoutesCollection
{
	string RootPath { get; }
	IEnumerable<string> GetAllRoutes();

	IDictionary<string, string> GetRoutesMap(string newRootPath);
}
