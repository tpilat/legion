using Legion.Http.Headers;

namespace Legion.Http;

public abstract class ContentBase
{
	public ContentHeaders Headers { get; }
	public bool ClearDefaultHeaders { get; set; }

	public ContentBase()
	{
		Headers = new ContentHeaders();
	}

	public abstract Task<string?> ToStringAsync();
}
