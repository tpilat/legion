using Legion.Extensions;

namespace Legion.Razor.UI.Routing;

public abstract class RoutesCollectionBase : IRoutesCollection
{
	public abstract string RootPath { get; }

	public abstract IEnumerable<string> GetAllRoutes();

	public IDictionary<string, string> GetRoutesMap(string newRootPath)
	{
		Throw.IfArgumentNullOrWhiteSpace(newRootPath);

		newRootPath = $"{newRootPath.TrimPostfix("/")}/";

		return GetAllRoutes()
			.Select(oldRoute =>
			{
				if (!oldRoute.StartsWith(RootPath, StringComparison.OrdinalIgnoreCase))
					Throw.InvalidOperationException(null, $"Invalid rootPath. | Route = {oldRoute} | Expected {nameof(RootPath)} = {RootPath}"); //TODO ErrorCode

				return new KeyValuePair<string, string>(oldRoute, $"{newRootPath}{oldRoute.TrimPrefix(RootPath, true)}");
			})
			.ToDictionary();
	}
}

