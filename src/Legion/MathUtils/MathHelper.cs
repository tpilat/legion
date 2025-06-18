using System.Globalization;

namespace Legion.MathUtils;

public static class MathHelper
{
	public static string? GetDecimalSeparator()
		=> GetDecimalSeparator(Thread.CurrentThread?.CurrentCulture!);

	public static string? GetDecimalSeparator(CultureInfo cultureInfo)
	{
		Throw.IfArgumentNull(cultureInfo);

		if (cultureInfo.NumberFormat != null
			&& !string.IsNullOrWhiteSpace(cultureInfo.NumberFormat.NumberDecimalSeparator))
		{
			return cultureInfo.NumberFormat.NumberDecimalSeparator;
		}
		else
		{
			return null;
		}
	}

	public static int? IntParseSafe(string? text)
	{
		if (string.IsNullOrWhiteSpace(text))
			return null;

		if (int.TryParse(text, out int value))
			return value;

		return null;
	}

	public static long? LongParseSafe(string? text)
	{
		if (string.IsNullOrWhiteSpace(text))
			return null;

		if (long.TryParse(text, out long value))
			return value;

		return null;
	}
}
