using Legion.Exceptions.Internal;
using Legion.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Legion.Exceptions;

public class ArgOutOfRangeException : ArgException, ILegionException
{
	public ArgOutOfRangeException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgOutOfRangeException.Default, detail, scopeContext)
	{
	}

	public ArgOutOfRangeException(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgOutOfRangeException.Default, paramName, detail, scopeContext)
	{
	}

	public ArgOutOfRangeException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgOutOfRangeException.Default, detail, innerException, scopeContext)
	{
	}

	[System.Diagnostics.StackTraceHidden]
	public static new void ThrowIf<T>(bool condition, T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
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
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.NonZero(value, paramName), detail, scopeContext);
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
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.NonNegative(value, paramName), detail, scopeContext);
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
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.NonNegativeNonZero(value, paramName), detail, scopeContext);
	}

	public static void ThrowIfEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
	{
		if (EqualityComparer<T>.Default.Equals(value, other))
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.NotEqual(value, other, paramName), detail, scopeContext);
	}

	public static void ThrowIfNotEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
	{
		if (!EqualityComparer<T>.Default.Equals(value, other))
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.Equal(value, other, paramName), detail, scopeContext);
	}

	public static void ThrowIfContainsIn<T>(T value, IEnumerable<T> others, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
	{
		if (others?.Any(x => EqualityComparer<T>.Default.Equals(value, x)) == true)
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.NotIn(value, others, paramName), detail, scopeContext);
	}

	public static void ThrowIfNotContainsIn<T>(T value, IEnumerable<T> others, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
	{
		if (others?.Any(x => EqualityComparer<T>.Default.Equals(value, x)) != true)
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.In(value, others, paramName), detail, scopeContext);
	}

	public static void ThrowIfGreaterThan<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
	{
		if (value.CompareTo(other) > 0)
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.LessOrEqual(value, other, paramName), detail, scopeContext);
	}

	public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
	{
		if (value.CompareTo(other) >= 0)
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.Less(value, other, paramName), detail, scopeContext);
	}

	public static void ThrowIfLessThan<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
	{
		if (value.CompareTo(other) < 0)
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.GreaterOrEqual(value, other, paramName), detail, scopeContext);
	}

	public static void ThrowIfLessThanOrEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
	{
		if (value.CompareTo(other) <= 0)
			Throw(paramName, errorCode ?? ErrorCodes.ArgOutOfRangeException.Greater(value, other, paramName), detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static new void Throw(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		=> throw new ArgOutOfRangeException(paramName, errorCode, detail, scopeContext);
}
