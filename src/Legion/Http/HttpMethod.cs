namespace Legion.Http;

public enum HttpMethod
{
	Get = 0,
	Post = 1,
	Put = 2,
	Delete = 3,
	Options = 4,
	Head = 5,
	Trace = 6,
#if NET6_0_OR_GREATER
	Patch = 7
#endif
}

public static class HttpApiMethodExtensions
{
	public static System.Net.Http.HttpMethod ToHttpMethod(this HttpMethod httpApiMethod)
	{
		return httpApiMethod switch
		{
			HttpMethod.Get => System.Net.Http.HttpMethod.Get,
			HttpMethod.Post => System.Net.Http.HttpMethod.Post,
			HttpMethod.Put => System.Net.Http.HttpMethod.Put,
			HttpMethod.Delete => System.Net.Http.HttpMethod.Delete,
			HttpMethod.Options => System.Net.Http.HttpMethod.Options,
			HttpMethod.Head => System.Net.Http.HttpMethod.Head,
			HttpMethod.Trace => System.Net.Http.HttpMethod.Trace,
#if NET6_0_OR_GREATER
			HttpMethod.Patch => System.Net.Http.HttpMethod.Patch,
#endif
			_ => System.Net.Http.HttpMethod.Get,
		};
	}
}
