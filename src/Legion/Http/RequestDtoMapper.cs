using Legion.Extensions;
using Legion.Web.Logging;

namespace Legion.Http;

public static class RequestDtoMapper
{
	public static async Task<RequestDto> MapAsync(
		HttpRequestMessage httpRequest,
		string? remoteIp,
		IScopeContext scopeContext,
		bool logRequestHeaders,
		bool logRequestBodyAsString,
		bool logRequestBodyAsByteArray,
		CancellationToken cancellationToken)
	{
		Throw.IfArgumentNull(httpRequest);

		var request = new RequestDto
		{
			CorrelationId = scopeContext?.CorrelationId,
			ExternalCorrelationId = scopeContext?.ExternalCorrelationId,
			ContextProperties = scopeContext?.ContextProperties?.ToDictionary(x => x.Key, x => x.Value) ?? [],
			RemoteIp = remoteIp
		};

		try { request.Method = httpRequest.Method.Method; } catch { }
		try { request.Path = httpRequest.RequestUri?.ToString(); } catch { }
		try { request.ContentType = httpRequest.Content?.Headers?.ContentType?.ToString(); } catch { }

		if (logRequestHeaders)
		{
			try
			{
				if (httpRequest.Headers != null)
				{
					var headers = httpRequest.Headers.ToDictionary(x => x.Key, x => x.Value);

					if (httpRequest.Content?.Headers != null)
					{
						var contentHeaders = httpRequest.Content.Headers.ToDictionary(x => x.Key, x => x.Value);
						headers.AddOrReplaceRange(contentHeaders);
					}

					request.Headers = Newtonsoft.Json.JsonConvert.SerializeObject(headers);
				}
			}
			catch { }
		}

		if (logRequestBodyAsString)
		{
			if (httpRequest.Content != null)
				request.Body = await httpRequest.Content.ReadAsStringAsync(
#if NET6_0_OR_GREATER
					cancellationToken
#endif
					).ConfigureAwait(false);

			if (string.IsNullOrWhiteSpace(request.Body))
				request.Body = null;
		}

		if (logRequestBodyAsByteArray)
		{
			if (httpRequest.Content != null)
				request.BodyByteArray = await httpRequest.Content.ReadAsByteArrayAsync(
#if NET6_0_OR_GREATER
					cancellationToken
#endif
					).ConfigureAwait(false);

			if (request.BodyByteArray != null && request.BodyByteArray.Length == 0)
				request.BodyByteArray = null;
		}

		return request;
	}
}
