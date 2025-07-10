using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Legion.AspNetCore.WebApi.Configurations;

public class ServerFilter : IDocumentFilter
{
	private readonly WebApiOptions _webApiOptions;

	public ServerFilter(IOptions<WebApiOptions> webApiOptions)
	{
		Throw.IfArgumentNull(webApiOptions);
		_webApiOptions = webApiOptions.Value;
	}

	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		//foreach (var path in swaggerDoc.Paths.Keys)
		//{
		//	Console.WriteLine($"Generated path: {path}");
		//}

		if (0 < _webApiOptions.SwaggerEndpoints?.Count)
		{
			var swaggerEndpoint = _webApiOptions.SwaggerEndpoints
				.Where(x => x.Version == swaggerDoc.Info.Version)
				.FirstOrDefault();

			if (swaggerEndpoint?.Servers == null || swaggerEndpoint.Servers.Count == 0)
				return;

			swaggerDoc.Servers.Clear();

			foreach (var server in swaggerEndpoint.Servers)
			{
				swaggerDoc.Servers.Add(new OpenApiServer
				{
					Url = server.Url,
					Description = server.Description
				});
			}
		}
	}
}
