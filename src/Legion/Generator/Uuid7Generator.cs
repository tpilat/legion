using System.Security.Cryptography;

namespace Legion.Generator;

public static class Uuid7Generator
{
	/// <summary>
	/// Generates a UUID version 7 (time-ordered, RFC 4122 variant) that works on any .NET version.
	/// </summary>
	public static Guid NewUuid7()
	{
		// Unix timestamp in milliseconds (48 bits)
		ulong unixTimeMs = (ulong)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

		byte[] uuidBytes = new byte[16];

		// Set the 48-bit timestamp (first 6 bytes)
		uuidBytes[0] = (byte)((unixTimeMs >> 40) & 0xFF);
		uuidBytes[1] = (byte)((unixTimeMs >> 32) & 0xFF);
		uuidBytes[2] = (byte)((unixTimeMs >> 24) & 0xFF);
		uuidBytes[3] = (byte)((unixTimeMs >> 16) & 0xFF);
		uuidBytes[4] = (byte)((unixTimeMs >> 8) & 0xFF);
		uuidBytes[5] = (byte)(unixTimeMs & 0xFF);

		// Fill remaining 10 bytes with random data
#if NET8_0_OR_GREATER
		RandomNumberGenerator.Fill(uuidBytes.AsSpan(6, 10));
#else
		using (var rng = RandomNumberGenerator.Create())
		{
			byte[] randomBytes = new byte[10];
			rng.GetBytes(randomBytes);

			Buffer.BlockCopy(randomBytes, 0, uuidBytes, 6, 10);
		}
#endif

		// Set UUID variant to RFC 4122 (bits 64–65)
		uuidBytes[6] &= 0x0F;       // Clear upper 4 bits
		uuidBytes[6] |= 0x70;       // set version 7 (0111xxxx)

		// Variant RFC 4122 (bits 64–65)
		uuidBytes[8] &= 0x3F;       // Clear upper 2 bits
		uuidBytes[8] |= 0x80;       // set variant (10xxxxxx)

		return CreateGuidFromRfcBytes(uuidBytes);
	}

	private static Guid CreateGuidFromRfcBytes(byte[] rfcBytes)
	{
		// Convert first 3 fields (time_low, time_mid, time_hi) to little endian for .NET Guid
		var guidBytes = new byte[16];

		// time_low (4 bytes)
		guidBytes[3] = rfcBytes[0];
		guidBytes[2] = rfcBytes[1];
		guidBytes[1] = rfcBytes[2];
		guidBytes[0] = rfcBytes[3];

		// time_mid (2 bytes)
		guidBytes[5] = rfcBytes[4];
		guidBytes[4] = rfcBytes[5];

		// time_hi_and_version (2 bytes)
		guidBytes[7] = rfcBytes[6];
		guidBytes[6] = rfcBytes[7];

		// remaining 8 bytes stay as is (big endian)
		Array.Copy(rfcBytes, 8, guidBytes, 8, 8);

		return new Guid(guidBytes);
	}
}
