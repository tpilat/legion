using Legion.Http.Json;
using Legion.Serializer;
using System.Net.Http.Headers;

namespace Legion.Http;

public class JsonContent : ContentBase
{
	internal JsonContentNet? _jsonContent;

	public object? Content { get; set; }
	public Type? InputType { get; set; }
	public MediaTypeHeaderValue? MediaType { get; set; }
	public Newtonsoft.Json.JsonSerializerSettings? JsonSerializerSettings { get; set; }
	public string? HttpContentNameFormDataMultipartPurpose { get; set; }

	public static JsonContent FromJsonContent(JsonContentNet jsonContent)
	{
		Throw.IfArgumentNull(jsonContent);

		var result = new JsonContent
		{
			_jsonContent = jsonContent,
			MediaType = jsonContent.Headers?.ContentType
		};

		return result;
	}

	private bool contentHasBeenRead = false;
	internal async Task<JsonContent> ReadContentAsync()
	{
		if (contentHasBeenRead || _jsonContent == null)
			return this;

		if (InputType != null)
		{
			using var stream = await _jsonContent.ReadAsStreamAsync().ConfigureAwait(false);
			Content = await JsonSerializerHelper.DeserializeAsync(stream, InputType, JsonSerializerSettings);
		}
		else
		{
			Content = await _jsonContent.ReadAsStringAsync();
		}

		contentHasBeenRead = true;
		return this;
	}

	public JsonContentNet ToJsonContent()
	{
		if (_jsonContent != null)
			return _jsonContent;

		if (InputType == null)
			throw new InvalidOperationException($"{nameof(InputType)} == null");

		var content = JsonContentNet.Create(Content, InputType, MediaType, JsonSerializerSettings);

		if (ClearDefaultHeaders)
			content.Headers.Clear();

		Headers.SetHttpContentHeaders(content.Headers);

		return content;
	}

	private string? _jsonContentString = null;
	public override async Task<string?> ToStringAsync()
	{
		if (_jsonContentString != null)
			return _jsonContentString;

		if (_jsonContent == null)
			return null;

		_jsonContentString = await _jsonContent.ReadAsStringAsync();
		return _jsonContentString;
	}
}

public class JsonContent<T> : JsonContent
{
	public JsonContent()
	{
		InputType = typeof(T);
	}

	public static async Task<JsonContent<T>> SetJsonContentAsync(JsonContentNet jsonContent, bool readPayload)
	{
		var result = new JsonContent<T>
		{
			_jsonContent = jsonContent ?? throw new ArgumentNullException(nameof(jsonContent)),
			MediaType = jsonContent.Headers?.ContentType
		};

		if (readPayload)
		{
			using var stream = await result._jsonContent.ReadAsStreamAsync().ConfigureAwait(false);
			result.Content = await JsonSerializerHelper.DeserializeAsync<T>(stream, result.JsonSerializerSettings);
		}

		return result;
	}
}
