namespace Legion.ADF.Audit.DTOs;

public class DbOidContent : Content
{
	public long DbOid {get; set; }

	public DbOidContent()
	{
		MimeType = Legion.Net.MimeTypes.octet_stream;
		ContentEncoding = null;
		Name = null;
		Metadata = null;
		IsCompressed = false;
		EncryptionKey = null;
	}

	public DbOidContent(long dbOid)
		: this()
	{
		Throw.IfArgumentIsLessThanOrEqual(dbOid, 0);

		DbOid = dbOid;
	}
}
