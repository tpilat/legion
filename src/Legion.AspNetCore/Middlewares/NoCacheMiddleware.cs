using Microsoft.AspNetCore.Http;

namespace Legion.AspNetCore.Middlewares;

public class NoCacheMiddleware
{
	private readonly RequestDelegate _next;
	private readonly HashSet<PathString>? _paths;

	public NoCacheMiddleware(RequestDelegate next, IEnumerable<string> pathsToDisableCache)
	{
		Throw.IfArgumentNull(next);

		_next = next;

		if (pathsToDisableCache?.Any() == true)
		{
			_paths = [];

			foreach (var path in pathsToDisableCache)
			{
				_paths.Add(new PathString(path));
			}
		}
		else
		{
			_paths = null;
		}
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (_paths != null && _paths.Any(p => context.Request.Path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
		{
			var headers = context.Response.Headers;
			headers.CacheControl = "no-store, no-cache, must-revalidate";
			headers.Pragma = "no-cache";
			headers.Expires = "0";
		}

		await _next(context);
	}
}
