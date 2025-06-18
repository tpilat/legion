namespace Legion.Web;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IFormFile
{
	Guid? Id { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	Stream? Content { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	byte[]? Data { get; }

	string? FileName { get; }
	string? ContentType { get; }
	long? Length { get; }
	string? Tag { get; }
	string? Hash { get; }
	Guid? AuditOperation { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	bool HasContentData { get; }

	Stream? OpenReadStream(bool asMemoryStream = false);

	Task<Stream?> OpenReadStreamAsync(bool asMemoryStream = false, CancellationToken cancellationToken = default);

	Task CopyToAsync(Stream targetStream, CancellationToken cancellationToken = default);

	byte[]? GetByteArray();

	byte[]? ConvertContentToData();
}
