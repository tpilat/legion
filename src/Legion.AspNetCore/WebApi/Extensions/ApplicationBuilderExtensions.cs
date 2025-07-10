using Legion.AspNetCore.WebApi.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Legion.AspNetCore.WebApi;

public static class ApplicationBuilderExtensions
{
	public static IApplicationBuilder UseWebApi(this IApplicationBuilder app)
	{
		Throw.IfArgumentNull(app);

		var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
		using var scope = serviceScopeFactory.CreateScope();
		var serviceProvider = scope.ServiceProvider;

		var webApiOptions = serviceProvider.GetRequiredService<IOptions<WebApiOptions>>().Value;

		if (webApiOptions.EnableSwagger && 0 < webApiOptions.SwaggerEndpoints?.Count)
		{
			app.UseSwagger();

			app.UseSwaggerUI(options =>
			{
				foreach (var swaggerEndpoint in webApiOptions.SwaggerEndpoints)
					options.SwaggerEndpoint(swaggerEndpoint.SwaggerDocumentUrl, swaggerEndpoint.Name);
			});
		}

		return app;
	}
}
