using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Legion.AspNetCore.WebApi.Configurations;

public class TrimPrefixFromPathsFilter : IDocumentFilter
{
	private readonly string _prefixToRemove;

	public TrimPrefixFromPathsFilter(string version)
	{
		_prefixToRemove = $"/api/{version}";
	}

	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		var newPaths = new OpenApiPaths();

		foreach (var path in swaggerDoc.Paths)
		{
			var trimmedPath = path.Key.StartsWith(_prefixToRemove)
				? path.Key.Substring(_prefixToRemove.Length)
				: path.Key;

			if (!trimmedPath.StartsWith("/"))
				trimmedPath = "/" + trimmedPath;

			newPaths.Add(trimmedPath, path.Value);
		}

		swaggerDoc.Paths = newPaths;
	}
}

