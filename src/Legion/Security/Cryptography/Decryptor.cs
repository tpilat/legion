using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Legion.Security.Cryptography;

public class Decryptor
{
	private const int KeySize = 256;
	private const int BlockSize = 128;

	public static string? DecryptWithSymmetricAes(EncryptionResult encryptionResult)
	{
		Throw.IfArgumentNull(encryptionResult);

		if (encryptionResult.EncryptedData == null)
		{
			return null;
		}
		else if (string.IsNullOrEmpty(encryptionResult.EncryptedData))
		{
			return string.Empty;
		}

		var key = Convert.FromBase64String(encryptionResult.Key);
		var (iv, encryptedData) = encryptionResult.GetIVAndEncryptedData();

		using var aes = Aes.Create();
		aes.KeySize = KeySize;
		aes.BlockSize = BlockSize;
		aes.Key = key;
		aes.IV = iv!; //Initialization Vector

		// Create decryptor and decrypt the data
		using var decryptor = aes.CreateDecryptor();
		using var msDecrypt = new MemoryStream(encryptedData!);
		using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
		using var srDecrypt = new StreamReader(csDecrypt, encryptionResult.DataEncoding ?? GlobalCache.UTF8NoBOM);

		try
		{
			return srDecrypt.ReadToEnd();
		}
		catch (CryptographicException ex)
		{
			// Log the error securely - avoid exposing details
			throw new CryptographicException("Decryption failed", ex);
		}
	}

	public static byte[]? DecryptWithSymmetricAesAsBytes(EncryptionResult encryptionResult)
	{
		Throw.IfArgumentNull(encryptionResult);

		if (encryptionResult.EncryptedData == null)
		{
			return null;
		}
		else if (string.IsNullOrEmpty(encryptionResult.EncryptedData))
		{
			return [];
		}

		var key = Convert.FromBase64String(encryptionResult.Key);
		var (iv, encryptedData) = encryptionResult.GetIVAndEncryptedData();

		using var aes = Aes.Create();
		aes.KeySize = KeySize;
		aes.BlockSize = BlockSize;
		aes.Key = key;
		aes.IV = iv!; //Initialization Vector

		// Create decryptor and decrypt the data
		using var decryptor = aes.CreateDecryptor();
		using var msDecrypt = new MemoryStream(encryptedData!);
		using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

		try
		{
			using var memoryStream = new MemoryStream();
			csDecrypt.CopyTo(memoryStream);
			return memoryStream.ToArray();
		}
		catch (CryptographicException ex)
		{
			// Securely log the error without sensitive information
			throw new CryptographicException("Decryption failed", ex);
		}
	}

	public static string? DecryptHybrid(EncryptionResult encryptionResult, X509Certificate2 certificateWithPrivateKey)
	{
		Throw.IfArgumentNull(encryptionResult);
		Throw.IfArgumentNull(certificateWithPrivateKey);

		if (encryptionResult.EncryptedData == null)
		{
			return null;
		}
		else if (string.IsNullOrEmpty(encryptionResult.EncryptedData))
		{
			return string.Empty;
		}

		// Decrypt AES key using certificate's private RSA key
		using var rsa = certificateWithPrivateKey.GetRSAPrivateKey();

		Throw.IfNull(rsa, errorCode: null, "Certificate does not contain a private key.");

		var key = rsa.Decrypt(Convert.FromBase64String(encryptionResult.Key), RSAEncryptionPadding.OaepSHA256);
		var (iv, encryptedData) = encryptionResult.GetIVAndEncryptedData();

		using var aes = Aes.Create();
		aes.KeySize = 256;
		aes.Key = key;
		aes.IV = iv!; //Initialization Vector

		// Create decryptor and decrypt the data
		using var decryptor = aes.CreateDecryptor();
		using var msDecrypt = new MemoryStream(encryptedData!);
		using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
		using var srDecrypt = new StreamReader(csDecrypt, encryptionResult.DataEncoding ?? GlobalCache.UTF8NoBOM);

		try
		{
			return srDecrypt.ReadToEnd();
		}
		catch (CryptographicException ex)
		{
			// Log the error securely - avoid exposing details
			throw new CryptographicException("Decryption failed", ex);
		}
	}

	public static byte[]? DecryptHybridAsBytes(EncryptionResult encryptionResult, X509Certificate2 certificateWithPrivateKey)
	{
		Throw.IfArgumentNull(encryptionResult);
		Throw.IfArgumentNull(certificateWithPrivateKey);

		if (encryptionResult.EncryptedData == null)
		{
			return null;
		}
		else if (string.IsNullOrEmpty(encryptionResult.EncryptedData))
		{
			return [];
		}

		// Decrypt AES key using certificate's private RSA key
		using var rsa = certificateWithPrivateKey.GetRSAPrivateKey();

		Throw.IfNull(rsa, errorCode: null, "Certificate does not contain a private key.");

		var key = rsa.Decrypt(Convert.FromBase64String(encryptionResult.Key), RSAEncryptionPadding.OaepSHA256);
		var (iv, encryptedData) = encryptionResult.GetIVAndEncryptedData();

		using var aes = Aes.Create();
		aes.KeySize = 256;
		aes.Key = key;
		aes.IV = iv!; //Initialization Vector

		// Create decryptor and decrypt the data
		using var decryptor = aes.CreateDecryptor();
		using var msDecrypt = new MemoryStream(encryptedData!);
		using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

		try
		{
			using var memoryStream = new MemoryStream();
			csDecrypt.CopyTo(memoryStream);
			return memoryStream.ToArray();
		}
		catch (CryptographicException ex)
		{
			// Securely log the error without sensitive information
			throw new CryptographicException("Decryption failed", ex);
		}
	}

	[return: NotNullIfNotNull(nameof(encryptedText))]
	public static string? DecryptWithAsymmetricRsa(string? encryptedText, X509Certificate2 certificateWithPrivateKey, Encoding? dataEncoding = null)
	{
		Throw.IfArgumentNull(certificateWithPrivateKey);

		if (encryptedText == null)
		{
			return null;
		}
		else if (string.IsNullOrEmpty(encryptedText))
		{
			return string.Empty;
		}

		using var rsa = certificateWithPrivateKey.GetRSAPrivateKey();

		Throw.IfNull(rsa, errorCode: null, "Certificate does not contain a private key.");

		byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
		byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);

		return (dataEncoding ?? GlobalCache.UTF8NoBOM).GetString(decryptedBytes);
	}

	[return: NotNullIfNotNull(nameof(encryptedData))]
	public static byte[]? DecryptWithAsymmetricRsa(byte[]? encryptedData, X509Certificate2 certificateWithPrivateKey)
	{
		Throw.IfArgumentNull(certificateWithPrivateKey);

		if (encryptedData == null)
		{
			return null;
		}
		else if (encryptedData.Length == 0)
		{
			return [];
		}

		using var rsa = certificateWithPrivateKey.GetRSAPrivateKey();

		Throw.IfNull(rsa, errorCode: null, "Certificate does not contain a private key.");

		byte[] decryptedBytes = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);

		return decryptedBytes;
	}

#if NET8_0_OR_GREATER

	[return: NotNullIfNotNull(nameof(encryptedText))]
	public static string? DecryptWithAsymmetricRsa(string? encryptedText, string base64PrivateKey, Encoding? dataEncoding = null)
	{
		Throw.IfArgumentNullOrWhiteSpace(base64PrivateKey);

		if (encryptedText == null)
		{
			return null;
		}
		else if (string.IsNullOrEmpty(encryptedText))
		{
			return string.Empty;
		}

		byte[] privateKeyBytes = Convert.FromBase64String(base64PrivateKey);

		using var rsa = RSA.Create();
		rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

		byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
		byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);

		return (dataEncoding ?? GlobalCache.UTF8NoBOM).GetString(decryptedBytes);
	}

	[return: NotNullIfNotNull(nameof(encryptedData))]
	public static byte[]? DecryptWithAsymmetricRsa(byte[]? encryptedData, string base64PrivateKey)
	{
		Throw.IfArgumentNullOrWhiteSpace(base64PrivateKey);

		if (encryptedData == null)
		{
			return null;
		}
		else if (encryptedData.Length == 0)
		{
			return [];
		}

		byte[] privateKeyBytes = Convert.FromBase64String(base64PrivateKey);

		using var rsa = RSA.Create();
		rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

		byte[] decryptedBytes = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);

		return decryptedBytes;
	}

#endif
}

