namespace Legion.ADF.Messaging.DTOs;

public abstract class Content
{
	public string MimeType {get; set; }
	public string? ContentEncoding {get; set; }
	public string? Name {get; set; }
	public string? Metadata {get; set; }
	public bool IsCompressed {get; set; }
	public string? EncryptionKey { get; set; }
}
