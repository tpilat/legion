namespace Legion.ADF.Messaging.DTOs;

public class FileRelativePath : Content
{
	public string RelativePath {get; set; }

	public FileRelativePath()
	{
		MimeType = Legion.Net.MimeTypes.octet_stream;
		ContentEncoding = null;
		Name = null;
		Metadata = null;
		IsCompressed = false;
		EncryptionKey = null;
	}

	public FileRelativePath(string relativePath)
		: this()
	{
		Throw.IfArgumentNullOrWhiteSpace(relativePath);

		RelativePath = relativePath;
	}
}
