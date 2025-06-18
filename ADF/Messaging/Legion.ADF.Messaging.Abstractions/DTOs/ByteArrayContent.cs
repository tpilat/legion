namespace Legion.ADF.Messaging.DTOs;

public class ByteArrayContent : Content
{
	public byte[] ByteArray {get; set; }

	public ByteArrayContent()
	{
		MimeType = Legion.Net.MimeTypes.octet_stream;
		ContentEncoding = null;
		Name = null;
		Metadata = null;
		IsCompressed = false;
		EncryptionKey = null;
	}

	public ByteArrayContent(byte[] byteArray)
		: this()
	{
		Throw.IfArgumentNullOrEmpty(byteArray);

		ByteArray = byteArray;
	}
}
