namespace Legion.Validation;

#if NET8_0_OR_GREATER
[Obsolete("for NET8+ use INumberBase<TSelf> Interface instead", DiagnosticId = "L_Compr_Num")]
#endif
public class ComparableHelper
{
	public static bool IsZero<T>(IComparable<T> value)
		=> value switch
		{
			byte val => val == 0,
			char val => val == 0,
			decimal val => val == 0,
			double val => val == 0,
			short val => val == 0,
			int val => val == 0,
			long val => val == 0,
			sbyte val => val == 0,
			float val => val == 0,
			ushort val => val == 0,
			uint val => val == 0,
			ulong val => val == 0,
			_ => false,
		};

	public static bool IsPositive<T>(IComparable<T> value)
		=> value switch
		{
			byte val => val >= 0,
			char val => val >= 0,
			decimal val => val >= 0,
			double val => val >= 0,
			short val => val >= 0,
			int val => val >= 0,
			long val => val >= 0,
			sbyte val => val >= 0,
			float val => val >= 0,
			ushort val => val >= 0,
			uint val => val >= 0,
			ulong val => val >= 0,
			_ => false,
		};

	public static bool IsNegative<T>(IComparable<T> value)
		=> value switch
		{
			byte val => val < 0,
			char val => val < 0,
			decimal val => val < 0,
			double val => val < 0,
			short val => val < 0,
			int val => val < 0,
			long val => val < 0,
			sbyte val => val < 0,
			float val => val < 0,
			ushort val => val < 0,
			uint val => val < 0,
			ulong val => val < 0,
			_ => false,
		};
}

