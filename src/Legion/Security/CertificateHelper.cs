using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Legion.Security;

public static class CertificateHelper
{
	[return: NotNullIfNotNull(nameof(cert))]
	public static string? GetSha256Thumbprint(X509Certificate2? cert)
	{
		Throw.IfArgumentNull(cert);

		byte[] hashBytes;
		using (var alg = SHA256.Create())
		{
			hashBytes = alg.ComputeHash(cert.RawData);
		}

		string result =
			BitConverter.ToString(hashBytes)
				.Replace("-", string.Empty)
				.ToUpper();

		return result;
	}

#if NET8_0_OR_GREATER
	public static X509Certificate2 CreateSelfSignedCertificate(string subjectName, int validYears = 100, string? password = null)
	{
		Throw.IfNullOrWhiteSpace(subjectName);
		Throw.IfArgumentIsLessThanOrEqual(validYears, 0);

		// Generate RSA private key explicitly with key size
		using var rsa = RSA.Create();
		rsa.KeySize = 2048;

		// Certificate request details
		var distinguishedName = new X500DistinguishedName($"CN={subjectName}");
		var request = new CertificateRequest(
			distinguishedName,
			rsa,
			HashAlgorithmName.SHA256,
			RSASignaturePadding.Pkcs1
		);

		// Optional but recommended extensions
		request.CertificateExtensions.Add(
			new X509KeyUsageExtension(
				X509KeyUsageFlags.DigitalSignature |
				X509KeyUsageFlags.KeyEncipherment |
				X509KeyUsageFlags.DataEncipherment,
				critical: false
			)
		);

		request.CertificateExtensions.Add(
			new X509EnhancedKeyUsageExtension(
				[
					new Oid(EnhancedKeyUsage.ServerAuthentication),
                    new Oid(EnhancedKeyUsage.ClientAuthentication)
                ],
				critical: false
			)
		);

		// Create self-signed certificate valid from yesterday
		using var certificate = request.CreateSelfSigned(
			DateTimeOffset.UtcNow.AddDays(-1),
			DateTimeOffset.UtcNow.AddYears(validYears)
		);

		// Export and re-import as X509Certificate2 to ensure persistence
		var export = certificate.Export(X509ContentType.Pfx, password);
		return new X509Certificate2(
			export,
			password,
			X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable
		);
	}

	public static X509Certificate2 CreateAssemblySigningCertificate(string subjectName, int validYears = 100, string? password = null)
	{
		Throw.IfNullOrWhiteSpace(subjectName);
		Throw.IfArgumentIsLessThanOrEqual(validYears, 0);

		using var rsa = RSA.Create();
		rsa.KeySize = 2048;

		var distinguishedName = new X500DistinguishedName($"CN={subjectName}");
		var request = new CertificateRequest(
			distinguishedName,
			rsa,
			HashAlgorithmName.SHA256,
			RSASignaturePadding.Pkcs1
		);

		// Set Key Usage for Digital Signature (required for code signing)
		request.CertificateExtensions.Add(
			new X509KeyUsageExtension(
				X509KeyUsageFlags.DigitalSignature,
				critical: true
			)
		);

		// Set Enhanced Key Usage explicitly for Code Signing
		request.CertificateExtensions.Add(
			new X509EnhancedKeyUsageExtension(
				[
					new Oid(EnhancedKeyUsage.CodeSigning)
                ],
				critical: true
			)
		);

		// Mark as CA: false, since it's only for signing assemblies (not issuing other certs)
		request.CertificateExtensions.Add(
			new X509BasicConstraintsExtension(
				certificateAuthority: false,
				hasPathLengthConstraint: false,
				pathLengthConstraint: 0,
				critical: true
			)
		);

		// Generate self-signed certificate
		using var certificate = request.CreateSelfSigned(
			DateTimeOffset.UtcNow.AddDays(-1),
			DateTimeOffset.UtcNow.AddYears(validYears)
		);

		// Export certificate with private key, ready for signing assemblies
		var exported = certificate.Export(X509ContentType.Pfx, password);
		return new X509Certificate2(
			exported,
			password,
			X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable
		);
	}

	public static X509Certificate2 CreateTimestampingCertificate(string subjectName, int validYears = 100, string? password = null)
	{
		Throw.IfNullOrWhiteSpace(subjectName);
		Throw.IfArgumentIsLessThanOrEqual(validYears, 0);

		// Create RSA key pair with 2048-bit length
		using var rsa = RSA.Create();
		rsa.KeySize = 2048;

		// Define the certificate's distinguished name (Subject)
		var distinguishedName = new X500DistinguishedName($"CN={subjectName}");

		// Create Certificate Request
		var request = new CertificateRequest(
			distinguishedName,
			rsa,
			HashAlgorithmName.SHA256,
			RSASignaturePadding.Pkcs1
		);

		// Key Usage: Digital Signature (required for timestamping)
		request.CertificateExtensions.Add(
			new X509KeyUsageExtension(
				X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation,
				critical: true
			)
		);

		// Enhanced Key Usage: Time Stamping (OID: 1.3.6.1.5.5.7.3.8)
		request.CertificateExtensions.Add(
			new X509EnhancedKeyUsageExtension(
				[
					new Oid(EnhancedKeyUsage.TimeStamping)
                ],
				critical: true
			)
		);

		// Basic Constraints: Not a Certificate Authority
		request.CertificateExtensions.Add(
			new X509BasicConstraintsExtension(
				certificateAuthority: false,
				hasPathLengthConstraint: false,
				pathLengthConstraint: 0,
				critical: true
			)
		);

		// Create self-signed certificate, valid immediately
		using var certificate = request.CreateSelfSigned(
			DateTimeOffset.UtcNow.AddDays(-1),
			DateTimeOffset.UtcNow.AddYears(validYears)
		);

		// Export the certificate as PFX (including private key)
		byte[] certPfxBytes = certificate.Export(X509ContentType.Pfx, password);

		// Re-import as X509Certificate2 to persist keys
		return new X509Certificate2(
			certPfxBytes,
			password,
			X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable
		);
	}
#endif

	/// <summary>
	/// Stores certificates with private keys and certificates chains securely in encrypted files, file format: .pfx
	/// </summary>
	/// <param name="certificate"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	public static byte[] ExportPrivateAndPublicKeysAsPfx(X509Certificate2 certificate, string? password = null)
	{
		Throw.IfArgumentNull(certificate);

		return certificate.Export(X509ContentType.Pfx, password);
	}

	/// <summary>
	/// Stores certificates with private keys and certificates chains securely in encrypted files, file format: .p12
	/// </summary>
	/// <param name="certificate"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	public static byte[] ExportPrivateAndPublicKeysAsP12(X509Certificate2 certificate, string? password = null)
		=> ExportPrivateAndPublicKeysAsPfx(certificate, password);

	/// <summary>
	/// Public certificate, binary DER, file format: .der
	/// </summary>
	/// <param name="certificate"></param>
	/// <returns></returns>
	public static byte[] ExportPublicKeyAsDer(X509Certificate2 certificate)
	{
		Throw.IfArgumentNull(certificate);

		return certificate.Export(X509ContentType.Cert);
	}

	/// <summary>
	/// Public certificate, binary DER, file format: .cer
	/// </summary>
	/// <param name="certificate"></param>
	/// <returns></returns>
	public static byte[] ExportPublicKeyAsCer(X509Certificate2 certificate)
		=> ExportPublicKeyAsDer(certificate);

	/// <summary>
	/// Public certificate, text PEM, file format: .pem
	/// </summary>
	/// <param name="certificate"></param>
	/// <returns></returns>
	public static string ExportPublicKeyAsPem(X509Certificate2 certificate)
	{
		Throw.IfArgumentNull(certificate);

		var builder = new StringBuilder();

		builder.AppendLine("-----BEGIN CERTIFICATE-----");
		builder.AppendLine(
			Convert.ToBase64String(certificate.Export(X509ContentType.Cert),
			Base64FormattingOptions.InsertLineBreaks));
		builder.AppendLine("-----END CERTIFICATE-----");

		return builder.ToString();
	}

	/// <summary>
	/// Public certificate, text PEM, file format: .crt
	/// </summary>
	/// <param name="certificate"></param>
	/// <returns></returns>
	public static string ExportPublicKeyAsCrt(X509Certificate2 certificate)
		=> ExportPublicKeyAsPem(certificate);

#if NET8_0_OR_GREATER
	/// <summary>
	/// Format for storing private keys (often RSA keys), file formats: .pem, .der, .key
	/// </summary>
	/// <param name="certificate"></param>
	/// <returns></returns>
	public static string ExportPrivateKeyAsPem(X509Certificate2 certificate)
	{
		Throw.IfArgumentNull(certificate);

		using RSA? rsa = certificate.GetRSAPrivateKey();

		Throw.IfNull(rsa, errorCode: null, "No private key.");

		var privateKeyBytes = rsa.ExportPkcs8PrivateKey();

		var builder = new StringBuilder();
		builder.AppendLine("-----BEGIN PRIVATE KEY-----");
		builder.AppendLine(
			Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks));
		builder.AppendLine("-----END PRIVATE KEY-----");
		return builder.ToString();
	}
#endif
}
