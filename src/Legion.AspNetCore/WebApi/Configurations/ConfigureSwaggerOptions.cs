using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Legion.AspNetCore.WebApi.Configurations;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
	private readonly WebApiOptions _webApiOptions;

	public ConfigureSwaggerOptions(IOptions<WebApiOptions> webApiOptions)
	{
		Throw.IfArgumentNull(webApiOptions);
		_webApiOptions = webApiOptions.Value;
	}

	public void Configure(SwaggerGenOptions options)
	{
		if (!_webApiOptions.EnableSwagger)
			return;


		if (!string.IsNullOrWhiteSpace(_webApiOptions.ApiPrefix))
		{
			if (0 < _webApiOptions.SwaggerEndpoints?.Count)
			{
				foreach (var swaggerEndpoints in _webApiOptions.SwaggerEndpoints)
				{
					options.SwaggerDoc(swaggerEndpoints.Version, new OpenApiInfo
					{
						Title = swaggerEndpoints.Name,
						Version = swaggerEndpoints.Version
					});

					options.AddServer(new OpenApiServer
					{
						Url = $"/{_webApiOptions.ApiPrefix}/{swaggerEndpoints.Version}",
						Description = $"version {swaggerEndpoints.Version}"
					});

					options.DocumentFilter<TrimPrefixFromPathsFilter>(swaggerEndpoints.Version);
				}
			}

			// Add server only once per document
			options.DocInclusionPredicate((docName, apiDesc) =>
			{
				var path = apiDesc.RelativePath?.ToLowerInvariant() ?? "";
				return path.StartsWith($"{_webApiOptions.ApiPrefix}/{docName}");
			});
		}

		if (_webApiOptions.SwaggerDocAssemblies?.Any() == true)
		{
			foreach (var assembly in _webApiOptions.SwaggerDocAssemblies)
			{
				var xmlFileName = $"{assembly.GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
			}
		}

		options.EnableAnnotations();

		options.ExampleFilters();

		options.DocumentFilter<ServerFilter>();
		options.SchemaFilter<ExampleSchemaFilter>();
	}
}
