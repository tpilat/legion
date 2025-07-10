using Legion.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Legion.AspNetCore;

public static class ApplicationBuilderExtensions
{
	public static IApplicationBuilder UseNoCacheFor(
		this IApplicationBuilder app,
		IEnumerable<string> pathsToDisableCache)
	{
		return app.UseMiddleware<NoCacheMiddleware>(pathsToDisableCache);
	}
}
