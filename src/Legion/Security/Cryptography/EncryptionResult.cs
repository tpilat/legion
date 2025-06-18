using System.Text;

namespace Legion.Security.Cryptography;

public class EncryptionResult
{
	public string? EncryptedData { get; set; }
	public Encoding? DataEncoding { get; set; }
	public string Key { get; set; }
	public string? CertificateThumbprint { get; set; }

	/// <summary>
	/// Creates an <see cref="EncryptionResult"/> by combining the provided IV and encrypted data.
	/// </summary>
	/// <param name="data">The encrypted data.</param>
	/// <param name="iv">The initialization vector (IV) used for encryption.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="certificateThumbprint">Certificate thumbprint that was used to encryot the <see cref="Key"/></param>
	/// <param name="dataEncoding">Encoding</param>
	/// <returns>An <see cref="EncryptionResult"/> containing the combined IV and encrypted data, and the encryption key.</returns>
	public static EncryptionResult CreateEncryptedData(byte[]? data, byte[] iv, string key, string? certificateThumbprint, Encoding? dataEncoding)
	{
		Throw.IfArgumentNull(iv);
		Throw.IfArgumentNullOrWhiteSpace(key);

		if (data == null)
		{
			return new EncryptionResult
			{
				EncryptedData = null!,
				Key = key,
				CertificateThumbprint = certificateThumbprint,
				DataEncoding = dataEncoding
			};
		}
		else if (data.Length == 0)
		{
			return new EncryptionResult
			{
				EncryptedData = string.Empty, //empty string
				Key = key,
				CertificateThumbprint = certificateThumbprint,
				DataEncoding = dataEncoding
			};
		}

		// Combine IV and encrypted data
		var combined = new byte[iv.Length + data.Length];
		Array.Copy(iv, 0, combined, 0, iv.Length);
		Array.Copy(data, 0, combined, iv.Length, data.Length);

		return new EncryptionResult
		{
			EncryptedData = Convert.ToBase64String(combined),
			Key = key,
			CertificateThumbprint = certificateThumbprint,
			DataEncoding = dataEncoding
		};
	}

	public (byte[]? iv, byte[]? encryptedData) GetIVAndEncryptedData()
	{
		if (EncryptedData == null)
		{
			return (null, null);
		}
		else if (string.IsNullOrEmpty(EncryptedData))
		{
			return (null, []); //empty array
		}

		var combined = Convert.FromBase64String(EncryptedData);

		// Extract IV and data
		var iv = new byte[16]; // Initialization Vector - AES block size is 16 bytes (128 / 8)
		var encryptedData = new byte[combined.Length - 16];

		Array.Copy(combined, 0, iv, 0, 16);
		Array.Copy(combined, 16, encryptedData, 0, encryptedData.Length);

		return (iv, encryptedData);
	}
}
