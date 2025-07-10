using System.Security.Cryptography;
using System.Text;

namespace Legion.Cryptography;

public static class HashHelper
{
	public static string ComputeSha256Hash(Stream data, bool seekStream = false)
	{
		Throw.IfArgumentNull(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		using var sha256Hash = SHA256.Create();

		var hash = sha256Hash.ComputeHash(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
	}

	public static string ComputeSha256Hash(string data)
		=> ComputeSha256Hash(Encoding.UTF8.GetBytes(data));

	public static string ComputeSha256Hash(byte[] data)
	{
		Throw.IfArgumentNull(data);

		using var sha256Hash = SHA256.Create();

		var hash = sha256Hash.ComputeHash(data);

		return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
	}

	public static byte[] ComputeSha256HashAsBytes(Stream data, bool seekStream = false)
	{
		Throw.IfArgumentNull(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		using var sha256Hash = SHA256.Create();

		var hash = sha256Hash.ComputeHash(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		return hash;
	}

	public static byte[] ComputeSha256HashAsBytes(string data)
		=> ComputeSha256HashAsBytes(Encoding.UTF8.GetBytes(data));

	public static byte[] ComputeSha256HashAsBytes(byte[] data)
	{
		Throw.IfArgumentNull(data);

		using var sha256Hash = SHA256.Create();

		return sha256Hash.ComputeHash(data);
	}

	public static string ComputeSha512Hash(Stream data, bool seekStream = false)
	{
		Throw.IfArgumentNull(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		using var sha512Hash = SHA512.Create();

		var hash = sha512Hash.ComputeHash(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
	}

	public static string ComputeSha512Hash(string data)
		=> ComputeSha512Hash(Encoding.UTF8.GetBytes(data));

	public static string ComputeSha512Hash(byte[] data)
	{
		Throw.IfArgumentNull(data);

		using var sha512Hash = SHA512.Create();

		var hash = sha512Hash.ComputeHash(data);

		return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
	}

	public static byte[] ComputeSha512HashAsBytes(Stream data, bool seekStream = false)
	{
		Throw.IfArgumentNull(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		using var sha512Hash = SHA512.Create();

		var bytes = sha512Hash.ComputeHash(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		return bytes;
	}

	public static byte[] ComputeSha512HashAsBytes(string data)
		=> ComputeSha512HashAsBytes(Encoding.UTF8.GetBytes(data));

	public static byte[] ComputeSha512HashAsBytes(byte[] data)
	{
		Throw.IfArgumentNull(data);

		using var sha512Hash = SHA512.Create();

		return sha512Hash.ComputeHash(data);
	}

	public static string ComputeMD5Hash(Stream data, bool seekStream = false)
	{
		Throw.IfArgumentNull(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		using var md5Hash = MD5.Create();

		var hash = md5Hash.ComputeHash(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
	}

	public static string ComputeMD5Hash(string data)
		=> ComputeMD5Hash(Encoding.UTF8.GetBytes(data));

	public static string ComputeMD5Hash(byte[] data)
	{
		Throw.IfArgumentNull(data);

		using var md5Hash = MD5.Create();

		var hash = md5Hash.ComputeHash(data);
		return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
	}

	public static byte[] ComputeMD5HashAsBytes(Stream data, bool seekStream = false)
	{
		Throw.IfArgumentNull(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		using var md5Hash = MD5.Create();

		var hash = md5Hash.ComputeHash(data);

		if (seekStream && data.CanSeek)
			data.Seek(0, SeekOrigin.Begin);

		return hash;
	}

	public static byte[] ComputeMD5HashAsBytes(string data)
		=> ComputeMD5HashAsBytes(Encoding.UTF8.GetBytes(data));

	public static byte[] ComputeMD5HashAsBytes(byte[] data)
	{
		Throw.IfArgumentNull(data);

		using var md5Hash = MD5.Create();

		return md5Hash.ComputeHash(data);
	}
}
