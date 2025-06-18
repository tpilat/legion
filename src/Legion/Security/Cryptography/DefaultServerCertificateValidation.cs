using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Legion.Security.Cryptography;

public static class DefaultServerCertificateValidation
{
#pragma warning disable IDE0060 // Remove unused parameter
	public static bool ServerCertificateValidation(
		object s,
		X509Certificate certificate,
		X509Chain chain,
		SslPolicyErrors sslPolicyErrors)
		=> true;
#pragma warning restore IDE0060 // Remove unused parameter
}
