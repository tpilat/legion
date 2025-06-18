using Legion.Exceptions.Internal;
using Legion.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Legion.Exceptions;

public class OutOfRangeException : ValueException, ILegionException
{
	public OutOfRangeException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.OutOfRangeException.Default, detail, scopeContext)
	{
	}

	public OutOfRangeException(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.OutOfRangeException.Default, paramName, detail, scopeContext)
	{
	}

	public OutOfRangeException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.OutOfRangeException.Default, detail, innerException, scopeContext)
	{
	}

	[System.Diagnostics.StackTraceHidden]
	public static void ThrowIf<T>(bool condition, T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (condition)
			Throw(paramName, errorCode, detail, scopeContext);
	}

	public static void ThrowIfZero<T>(T value, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		if (T.IsZero(value))
#else
		where T : IComparable<T>
	{
		if (ComparableHelper.IsZero(value))
#endif
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.NonZero(value, paramName), detail, scopeContext);
	}

	public static void ThrowIfNegative<T>(T value, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		if (T.IsNegative(value))
#else
		where T : IComparable<T>
	{
		if (ComparableHelper.IsNegative(value))
#endif
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.NonNegative(value, paramName), detail, scopeContext);
	}

	public static void ThrowIfNegativeOrZero<T>(T value, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		if (T.IsNegative(value) || T.IsZero(value))
#else
		where T : IComparable<T>
	{
		if (ComparableHelper.IsNegative(value) || ComparableHelper.IsZero(value))
#endif
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.NonNegativeNonZero(value, paramName), detail, scopeContext);
	}

	public static void ThrowIfEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
	{
		if (EqualityComparer<T>.Default.Equals(value, other))
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.NotEqual(value, other, paramName), detail, scopeContext);
	}

	public static void ThrowIfNotEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
	{
		if (!EqualityComparer<T>.Default.Equals(value, other))
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.Equal(value, other, paramName), detail, scopeContext);
	}

	public static void ThrowIfContainsIn<T>(T value, IEnumerable<T> others, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
	{
		if (others?.Any(x => EqualityComparer<T>.Default.Equals(value, x)) == true)
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.NotIn(value, others, paramName), detail, scopeContext);
	}

	public static void ThrowIfNotContainsIn<T>(T value, IEnumerable<T> others, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
	{
		if (others?.Any(x => EqualityComparer<T>.Default.Equals(value, x)) != true)
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.In(value, others, paramName), detail, scopeContext);
	}

	public static void ThrowIfGreaterThan<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
	{
		if (value.CompareTo(other) > 0)
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.LessOrEqual(value, other, paramName), detail, scopeContext);
	}

	public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
	{
		if (value.CompareTo(other) >= 0)
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.Less(value, other, paramName), detail, scopeContext);
	}

	public static void ThrowIfLessThan<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
	{
		if (value.CompareTo(other) < 0)
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.GreaterOrEqual(value, other, paramName), detail, scopeContext);
	}

	public static void ThrowIfLessThanOrEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
	{
		if (value.CompareTo(other) <= 0)
			Throw(paramName, errorCode ?? ErrorCodes.OutOfRangeException.Greater(value, other, paramName), detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void Throw(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		=> throw new OutOfRangeException(paramName, errorCode, detail, scopeContext);
}
