using System.Security.Cryptography;

namespace Legion.Security.Cryptography;

public static class KeyManager
{
	public static string GenerateRandomAesKey(int keySize = 256)
	{
		Throw.IfArgumentIsLessThanOrEqual(keySize, 0);

		using var aes = Aes.Create();
		aes.KeySize = keySize;
		aes.GenerateKey();
		return Convert.ToBase64String(aes.Key);
	}

#if NET8_0_OR_GREATER

	public static (string PublicKey, string PrivateKey) GenerateRandomRsaKeyPair(int keySizeInBits = 2048)
	{
		Throw.IfArgumentIsLessThanOrEqual(keySizeInBits, 0);

		using var rsa = RSA.Create(keySizeInBits);

		var publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
		var privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());

		return (publicKey, privateKey);
	}

#endif
}
