using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Legion.Security.Cryptography;

public class Encryptor
{
	private const int _symmetricAesKeySize = 256;
	private const int _symmetricAesBlockSize = 128;

	public static EncryptionResult EncryptWithSymmetricAes(string? plainText, string? base64Key = null, Encoding? dataEncoding = null)
	{
		// Generate a random key and IV
		using var aes = Aes.Create();
		aes.KeySize = _symmetricAesKeySize;
		aes.BlockSize = _symmetricAesBlockSize;

		var useRandomKey = string.IsNullOrWhiteSpace(base64Key);

		if (useRandomKey)
			aes.GenerateKey();
		else
			aes.Key = Convert.FromBase64String(base64Key!);

		aes.GenerateIV(); //Initialization Vector

		if (plainText == null)
		{
			return EncryptionResult.CreateEncryptedData(
				null,
				aes.IV, //Initialization Vector
				Convert.ToBase64String(aes.Key),
				certificateThumbprint: null,
				dataEncoding
			);
		}
		else if (string.IsNullOrEmpty(plainText))
		{
			return EncryptionResult.CreateEncryptedData(
				[], //empty array
				aes.IV, //Initialization Vector
				Convert.ToBase64String(aes.Key),
				certificateThumbprint: null,
				dataEncoding
			);
		}

		byte[] encryptedData;

		// Create encryptor and encrypt the data
		using (var encryptor = aes.CreateEncryptor())
		using (var msEncrypt = new MemoryStream())
		{
			using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
			using (var swEncrypt = new StreamWriter(csEncrypt, dataEncoding ?? GlobalCache.UTF8NoBOM))
			{
				swEncrypt.Write(plainText);
			}

			encryptedData = msEncrypt.ToArray();
		}

		// Package everything together, storing IV with the encrypted data
		var result = EncryptionResult.CreateEncryptedData(
			encryptedData,
			aes.IV, //Initialization Vector
			Convert.ToBase64String(aes.Key),
			certificateThumbprint: null,
			dataEncoding
		);

		return result;
	}

	public static EncryptionResult EncryptWithSymmetricAes(byte[]? plainData, string? base64Key = null, Encoding? dataEncoding = null)
	{
		// Generate a random key and IV
		using var aes = Aes.Create();
		aes.KeySize = _symmetricAesKeySize;
		aes.BlockSize = _symmetricAesBlockSize;

		var useRandomKey = string.IsNullOrWhiteSpace(base64Key);

		if (useRandomKey)
			aes.GenerateKey();
		else
			aes.Key = Convert.FromBase64String(base64Key!);

		aes.GenerateIV(); //Initialization Vector

		if (plainData == null)
		{
			return EncryptionResult.CreateEncryptedData(
				null,
				aes.IV, //Initialization Vector
				Convert.ToBase64String(aes.Key),
				certificateThumbprint: null,
				dataEncoding
			);
		}
		else if (plainData.Length == 0)
		{
			return EncryptionResult.CreateEncryptedData(
				[], //empty array
				aes.IV, //Initialization Vector
				Convert.ToBase64String(aes.Key),
				certificateThumbprint: null,
				dataEncoding
			);
		}

		byte[] encryptedData;

		// Create encryptor and encrypt the data
		using (var encryptor = aes.CreateEncryptor())
		using (var msEncrypt = new MemoryStream())
		{
			using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
			{
				csEncrypt.Write(plainData, 0, plainData.Length);
			}

			encryptedData = msEncrypt.ToArray();
		}

		// Package everything together, storing IV with the encrypted data
		var result = EncryptionResult.CreateEncryptedData(
			encryptedData,
			aes.IV, //Initialization Vector
			Convert.ToBase64String(aes.Key),
			certificateThumbprint: null,
			dataEncoding
		);

		return result;
	}

	public static EncryptionResult EncryptHybrid(string? plainText, X509Certificate2 keyEncryptionCertificateWithPublicKey, string? base64Key = null, Encoding? dataEncoding = null)
	{
		Throw.IfArgumentNull(keyEncryptionCertificateWithPublicKey);

		// Generate a random key and IV
		using var aes = Aes.Create();
		aes.KeySize = _symmetricAesKeySize;
		aes.BlockSize = _symmetricAesBlockSize;

		var useRandomKey = string.IsNullOrWhiteSpace(base64Key);

		if (useRandomKey)
			aes.GenerateKey();
		else
			aes.Key = Convert.FromBase64String(base64Key!);

		aes.GenerateIV(); //Initialization Vector

		if (plainText == null)
		{
			return EncryptionResult.CreateEncryptedData(
				null,
				aes.IV, //Initialization Vector
				Convert.ToBase64String(aes.Key),
				keyEncryptionCertificateWithPublicKey.Thumbprint,
				dataEncoding
			);
		}
		else if (string.IsNullOrEmpty(plainText))
		{
			return EncryptionResult.CreateEncryptedData(
				[], //empty array
				aes.IV, //Initialization Vector
				Convert.ToBase64String(aes.Key),
				keyEncryptionCertificateWithPublicKey.Thumbprint,
				dataEncoding
			);
		}

		byte[] encryptedData;

		// Create encryptor and encrypt the data
		using (var encryptor = aes.CreateEncryptor())
		using (var msEncrypt = new MemoryStream())
		{
			using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
			using (var swEncrypt = new StreamWriter(csEncrypt, dataEncoding ?? GlobalCache.UTF8NoBOM))
			{
				swEncrypt.Write(plainText);
			}

			encryptedData = msEncrypt.ToArray();
		}

		// Encrypt AES key using certificate's public key (RSA)
		using var rsa = keyEncryptionCertificateWithPublicKey.GetRSAPublicKey();

		Throw.IfNull(rsa, errorCode: null, "Certificate does not have a public key.");

		byte[] encryptedAesKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA256);

		// Package everything together, storing IV with the encrypted data
		var result = EncryptionResult.CreateEncryptedData(
			encryptedData,
			aes.IV, //Initialization Vector
			Convert.ToBase64String(encryptedAesKey),
			keyEncryptionCertificateWithPublicKey.Thumbprint,
			dataEncoding
		);

		return result;
	}

	public static EncryptionResult EncryptHybrid(byte[]? plainData, X509Certificate2 keyEncryptionCertificateWithPublicKey, string? base64Key = null, Encoding? dataEncoding = null)
	{
		Throw.IfArgumentNull(keyEncryptionCertificateWithPublicKey);

		// Generate a random key and IV
		using var aes = Aes.Create();
		aes.KeySize = _symmetricAesKeySize;
		aes.BlockSize = _symmetricAesBlockSize;

		var useRandomKey = string.IsNullOrWhiteSpace(base64Key);

		if (useRandomKey)
			aes.GenerateKey();
		else
			aes.Key = Convert.FromBase64String(base64Key!);

		aes.GenerateIV(); //Initialization Vector

		if (plainData == null)
		{
			return EncryptionResult.CreateEncryptedData(
				null,
				aes.IV, //Initialization Vector
				Convert.ToBase64String(aes.Key),
				keyEncryptionCertificateWithPublicKey.Thumbprint,
				dataEncoding
			);
		}
		else if (plainData.Length == 0)
		{
			return EncryptionResult.CreateEncryptedData(
				[], //empty array
				aes.IV, //Initialization Vector
				Convert.ToBase64String(aes.Key),
				keyEncryptionCertificateWithPublicKey.Thumbprint,
				dataEncoding
			);
		}

		byte[] encryptedData;

		// Create encryptor and encrypt the data
		using (var encryptor = aes.CreateEncryptor())
		using (var msEncrypt = new MemoryStream())
		{
			using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
			{
				csEncrypt.Write(plainData, 0, plainData.Length);
			}

			encryptedData = msEncrypt.ToArray();
		}

		// Encrypt AES key using certificate's public key (RSA)
		using var rsa = keyEncryptionCertificateWithPublicKey.GetRSAPublicKey();

		Throw.IfNull(rsa, errorCode: null, "Certificate does not have a public key.");

		byte[] encryptedAesKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA256);

		// Package everything together, storing IV with the encrypted data
		var result = EncryptionResult.CreateEncryptedData(
			encryptedData,
			aes.IV, //Initialization Vector
			Convert.ToBase64String(encryptedAesKey),
			keyEncryptionCertificateWithPublicKey.Thumbprint,
			dataEncoding
		);

		return result;
	}

	[return: NotNullIfNotNull(nameof(plainText))]
	public static string? EncryptWithAsymmetricRsa(string? plainText, X509Certificate2 certificateWithPublicKey, Encoding? dataEncoding = null)
	{
		Throw.IfArgumentNull(certificateWithPublicKey);

		if (plainText == null)
		{
			return null;
		}
		else if (string.IsNullOrEmpty(plainText))
		{
			return string.Empty;
		}

		using var rsa = certificateWithPublicKey.GetRSAPublicKey();

		Throw.IfNull(rsa, errorCode: null, "Certificate does not have a public key.");

		byte[] plainBytes = (dataEncoding ?? GlobalCache.UTF8NoBOM).GetBytes(plainText);
		byte[] encryptedBytes = rsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA256);

		return Convert.ToBase64String(encryptedBytes);
	}

	[return: NotNullIfNotNull(nameof(plainData))]
	public static byte[]? EncryptWithAsymmetricRsa(byte[]? plainData, X509Certificate2 certificateWithPublicKey)
	{
		Throw.IfArgumentNull(certificateWithPublicKey);

		if (plainData == null)
		{
			return null;
		}
		else if (plainData.Length == 0)
		{
			return [];
		}

		using var rsa = certificateWithPublicKey.GetRSAPublicKey();

		Throw.IfNull(rsa, errorCode: null, "Certificate does not have a public key.");

		byte[] encryptedBytes = rsa.Encrypt(plainData, RSAEncryptionPadding.OaepSHA256);

		return encryptedBytes;
	}

#if NET8_0_OR_GREATER

	[return: NotNullIfNotNull(nameof(plainText))]
	public static string? EncryptWithAsymmetricRsa(string? plainText, string base64PublicKey, Encoding? dataEncoding = null)
	{
		Throw.IfArgumentNullOrWhiteSpace(base64PublicKey);

		if (plainText == null)
		{
			return null;
		}
		else if (string.IsNullOrEmpty(plainText))
		{
			return string.Empty;
		}

		byte[] publicKeyBytes = Convert.FromBase64String(base64PublicKey);

		using var rsa = RSA.Create();
		rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

		byte[] dataBytes = (dataEncoding ?? GlobalCache.UTF8NoBOM).GetBytes(plainText);
		byte[] encryptedBytes = rsa.Encrypt(dataBytes, RSAEncryptionPadding.OaepSHA256);

		return Convert.ToBase64String(encryptedBytes);
	}

	[return: NotNullIfNotNull(nameof(plainData))]
	public static byte[]? EncryptWithAsymmetricRsa(byte[]? plainData, string base64PublicKey)
	{
		Throw.IfArgumentNullOrWhiteSpace(base64PublicKey);

		if (plainData == null)
		{
			return null;
		}
		else if (plainData.Length == 0)
		{
			return [];
		}

		byte[] publicKeyBytes = Convert.FromBase64String(base64PublicKey);

		using var rsa = RSA.Create();
		rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

		byte[] encryptedBytes = rsa.Encrypt(plainData, RSAEncryptionPadding.OaepSHA256);

		return encryptedBytes;
	}

#endif
}

