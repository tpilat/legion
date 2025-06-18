using System.Security.Cryptography.X509Certificates;

namespace Legion.Security;

public static class CertificateStoreHelper
{
	public static X509Certificate2? FindByThumbprint(string thumbprint, StoreLocation storeLocation, StoreName? storeName = null)
	{
		X509Certificate2? cert = null;

		using var store = storeName.HasValue
			? new X509Store(storeName.Value, storeLocation)
			: new X509Store(storeLocation);

		store.Open(OpenFlags.ReadOnly);

		var cers = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

		return cert;
	}

	public static X509Certificate2? FindBySerialNumber(string serialNumber, StoreLocation storeLocation, StoreName? storeName = null)
	{
		X509Certificate2? cert = null;

		using var store = storeName.HasValue
			? new X509Store(storeName.Value, storeLocation)
			: new X509Store(storeLocation);

		store.Open(OpenFlags.ReadOnly);

		var cers = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, false);

		return cert;
	}

	public static X509Certificate2? FindBySubjectName(string subjectName, StoreLocation storeLocation, StoreName? storeName = null)
	{
		X509Certificate2? cert = null;

		using var store = storeName.HasValue
			? new X509Store(storeName.Value, storeLocation)
			: new X509Store(storeLocation);

		store.Open(OpenFlags.ReadOnly);

		var cers = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, false);

		return cert;
	}
}
