namespace Legion.Security;

public class EnhancedKeyUsage
{
	/// <summary>
	/// TLS/SSL web servers, HTTPS
	/// </summary>
	public const string ServerAuthentication = "1.3.6.1.5.5.7.3.1";

	/// <summary>
	/// Client-side authentication
	/// </summary>
	public const string ClientAuthentication = "1.3.6.1.5.5.7.3.2";

	/// <summary>
	/// Signing executable code (.exe, .dll, etc.)
	/// </summary>
	public const string CodeSigning = "1.3.6.1.5.5.7.3.3";

	/// <summary>
	/// Secure Email - S/MIME email signing and encryption
	/// </summary>
	public const string EmailProtection = "1.3.6.1.5.5.7.3.4";

	/// <summary>
	/// Timestamp Authorities (TSAs)
	/// </summary>
	public const string TimeStamping = "1.3.6.1.5.5.7.3.8";

	/// <summary>
	/// Online Certificate Status Protocol (OCSP)
	/// </summary>
	public const string OCSPSigning = "1.3.6.1.5.5.7.3.9";

	/// <summary>
	/// Signing documents (Microsoft Office, Adobe PDF)
	/// </summary>
	public const string DocumentSigning = "1.3.6.1.4.1.311.10.3.12";

	/// <summary>
	/// IPSec end-point authentication
	/// </summary>
	public const string IPSecEndSystem = "1.3.6.1.5.5.8.2.2";

	/// <summary>
	/// IPSec tunnel mode endpoint authentication
	/// </summary>
	public const string IPSecTunnel = "1.3.6.1.5.5.8.2.3";

	/// <summary>
	/// IPSec user authentication
	/// </summary>
	public const string IPSecUser = "1.3.6.1.5.5.8.2.4";

	/// <summary>
	/// (Microsoft) - Smart Card authentication on Windows
	/// </summary>
	public const string SmartCardLogon = "1.3.6.1.4.1.311.20.2.2";

	/// <summary>
	/// Allows the certificate for any purpose
	/// </summary>
	public const string AnyExtendedKeyUsage = "2.5.29.37.0";
}
