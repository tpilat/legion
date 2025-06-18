namespace Legion.ADF.Messaging.DTOs;

public class JsonContent : Content
{
	public string Json {get; set; }

	public JsonContent()
	{
		MimeType = Legion.Net.MimeTypes.json;
		ContentEncoding = GlobalCache.UTF8NoBOM.ToString();
		Name = null;
		Metadata = null;
		IsCompressed = false;
		EncryptionKey = null;
	}

	public JsonContent(string json)
		: this()
	{
		Throw.IfArgumentNullOrWhiteSpace(json);

		Json = json;
	}
}
