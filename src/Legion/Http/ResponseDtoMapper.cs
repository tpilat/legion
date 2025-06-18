using Legion.Extensions;
using Legion.Web.Logging;

namespace Legion.Http;

public static class ResponseDtoMapper
{
	public static async Task<ResponseDto> MapAsync(
		HttpResponseMessage? httpResponse,
		IScopeContext scopeContext,
		string? error,
		long? elapsedMilliseconds,
		bool logResponseHeaders,
		bool logResponseBodyAsString,
		bool logResponseBodyAsByteArray,
		CancellationToken cancellationToken)
	{
		var response = new ResponseDto
		{
			CorrelationId = scopeContext?.CorrelationId,
			ExternalCorrelationId = scopeContext?.ExternalCorrelationId,
			ContextProperties = scopeContext?.ContextProperties?.ToDictionary(x => x.Key, x => x.Value) ?? [],
			Error = error,
			ElapsedMilliseconds = elapsedMilliseconds,
			ContentType = httpResponse?.Content?.Headers?.ContentType?.ToString()
		};

		if (httpResponse == null)
			return response;

		response.StatusCode = (int)httpResponse.StatusCode;

		if (logResponseHeaders)
		{
			try
			{
				if (httpResponse.Headers != null)
				{
					var headers = httpResponse.Headers.ToDictionary(x => x.Key, x => x.Value);

					if (httpResponse.Content?.Headers != null)
					{
						var contentHeaders = httpResponse.Content.Headers.ToDictionary(x => x.Key, x => x.Value);
						headers.AddOrReplaceRange(contentHeaders);
					}

					response.Headers = Newtonsoft.Json.JsonConvert.SerializeObject(headers);
				}
			}
			catch { }
		}

		if (logResponseBodyAsString)
		{
			if (httpResponse.Content != null)
				response.Body = await httpResponse.Content.ReadAsStringAsync(
#if NET6_0_OR_GREATER
					cancellationToken
#endif
					).ConfigureAwait(false);

			if (string.IsNullOrWhiteSpace(response.Body))
				response.Body = null;
		}

		if (logResponseBodyAsByteArray)
		{
			if (httpResponse.Content != null)
				response.BodyByteArray = await httpResponse.Content.ReadAsByteArrayAsync(
#if NET6_0_OR_GREATER
					cancellationToken
#endif
					).ConfigureAwait(false);

			if (response.BodyByteArray != null && response.BodyByteArray.Length == 0)
				response.BodyByteArray = null;
		}

		return response;
	}
}
