using System.Text;

namespace Legion.ADF.Audit.DTOs;

public class StringContent : Content
{
	public string String { get; set; }

	public StringContent()
	{
		MimeType = Legion.Net.MimeTypes.txt;
		ContentEncoding = null;
		Name = null;
		Metadata = null;
		IsCompressed = false;
		EncryptionKey = null;
	}

	public StringContent(string @string, Encoding? encoding)
		: this()
	{
		Throw.IfArgumentNullOrWhiteSpace(@string);

		String = @string;
		ContentEncoding = encoding?.ToString();
	}
}
