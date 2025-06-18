using Legion.Exceptions;
using Legion.Exceptions.Internal;
using Legion.Validation;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Legion;

public static class Throw
{
	//TODO: REMINDER: dopln vsetko aj do IResultExtensions

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use ArgumentNullOrEmpty or ArgumentNullOrWhiteSpace instead. Do not use with nameof()"
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_ArgNullEx_Str")]
#else
	)]
#endif
	public static string? IfArgumentNull([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use ArgumentNullOrEmpty or ArgumentNullOrWhiteSpace instead. Do not use with nameof()"
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_ArgNullEx_Str")]
#else
	)]
#endif
	public static string? IfArgumentNull([NotNull] string? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgNullException.ThrowIfNull(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T IfArgumentNull<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T IfArgumentNull<T>([NotNull] T? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgNullException.ThrowIfNull(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static object? IfArgumentNull([NotNull] object? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static object? IfArgumentNull([NotNull] object? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgNullException.ThrowIfNull(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static unsafe void IfArgumentNull([NotNull] void* argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static unsafe void IfArgumentNull([NotNull] void* argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgNullException.ThrowIfNull(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static unsafe void IfArgumentNull(IntPtr argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static unsafe void IfArgumentNull(IntPtr argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgNullException.ThrowIfNull(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentDefault<T>([NotNull] T argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> ArgDefaultValueException.ThrowIfDefault(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentDefault<T>([NotNull] T argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> ArgDefaultValueException.ThrowIfDefault(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentDefault<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> ArgDefaultValueException.ThrowIfDefault(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentDefault<T>([NotNull] T? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> ArgDefaultValueException.ThrowIfDefault(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T? IfArgumentNullOrDefault<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> ArgDefaultValueException.ThrowIfNullOrDefault(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T? IfArgumentNullOrDefault<T>([NotNull] T? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> ArgDefaultValueException.ThrowIfNullOrDefault(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static string? IfArgumentNullOrEmpty([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgEmptyValueException.ThrowIfNullOrEmpty(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static string? IfArgumentNullOrEmpty([NotNull] string? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgEmptyValueException.ThrowIfNullOrEmpty(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static string? IfArgumentNullOrWhiteSpace([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgEmptyValueException.ThrowIfNullOrWhiteSpace(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static string? IfArgumentNullOrWhiteSpace([NotNull] string? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgEmptyValueException.ThrowIfNullOrWhiteSpace(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static ICollection? IfArgumentNullOrEmpty([NotNull] ICollection? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgEmptyValueException.ThrowIfNullOrEmpty(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static ICollection? IfArgumentNullOrEmpty([NotNull] ICollection? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgEmptyValueException.ThrowIfNullOrEmpty(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static Array? IfArgumentNullOrEmpty([NotNull] Array? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgEmptyValueException.ThrowIfNullOrEmpty(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static Array? IfArgumentNullOrEmpty([NotNull] Array? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgEmptyValueException.ThrowIfNullOrEmpty(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static IEnumerable? IfArgumentNullOrEmpty([NotNull] IEnumerable? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgEmptyValueException.ThrowIfNullOrEmpty(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static IEnumerable? IfArgumentNullOrEmpty([NotNull] IEnumerable? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> ArgEmptyValueException.ThrowIfNullOrEmpty(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T? IfArgumentNullOrEmpty<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEnumerable?
		=> ArgEmptyValueException.ThrowIfNullOrEmpty(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T? IfArgumentNullOrEmpty<T>([NotNull] T? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEnumerable?
		=> ArgEmptyValueException.ThrowIfNullOrEmpty(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsZero<T>(T argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> ArgOutOfRangeException.ThrowIfZero(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsZero<T>(T argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> ArgOutOfRangeException.ThrowIfZero(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsNegative<T>(T argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> ArgOutOfRangeException.ThrowIfNegative(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsNegative<T>(T argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> ArgOutOfRangeException.ThrowIfNegative(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsNegativeOrZero<T>(T argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> ArgOutOfRangeException.ThrowIfNegativeOrZero(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsNegativeOrZero<T>(T argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> ArgOutOfRangeException.ThrowIfNegativeOrZero(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsEqual<T>(T argument, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEquatable<T>?
		=> ArgOutOfRangeException.ThrowIfEqual(argument, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsEqual<T>(T argument, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEquatable<T>?
		=> ArgOutOfRangeException.ThrowIfEqual(argument, other, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsNotEqual<T>(T argument, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEquatable<T>?
		=> ArgOutOfRangeException.ThrowIfNotEqual(argument, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsNotEqual<T>(T argument, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEquatable<T>?
		=> ArgOutOfRangeException.ThrowIfNotEqual(argument, other, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentContainsIn<T>(T argument, IEnumerable<T> others, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEquatable<T>?
		=> ArgOutOfRangeException.ThrowIfContainsIn(argument, others, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentContainsIn<T>(T argument, IEnumerable<T> others, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEquatable<T>?
		=> ArgOutOfRangeException.ThrowIfContainsIn(argument, others, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentNotContainsIn<T>(T argument, IEnumerable<T> others, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEquatable<T>?
		=> ArgOutOfRangeException.ThrowIfNotContainsIn(argument, others, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentNotContainsIn<T>(T argument, IEnumerable<T> others, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEquatable<T>?
		=> ArgOutOfRangeException.ThrowIfNotContainsIn(argument, others, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsGreaterThan<T>(T argument, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IComparable<T>
		=> ArgOutOfRangeException.ThrowIfGreaterThan(argument, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsGreaterThan<T>(T argument, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IComparable<T>
		=> ArgOutOfRangeException.ThrowIfGreaterThan(argument, other, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsGreaterThanOrEqual<T>(T argument, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IComparable<T>
		=> ArgOutOfRangeException.ThrowIfGreaterThanOrEqual(argument, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsGreaterThanOrEqual<T>(T argument, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IComparable<T>
		=> ArgOutOfRangeException.ThrowIfGreaterThanOrEqual(argument, other, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsLessThan<T>(T argument, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IComparable<T>
		=> ArgOutOfRangeException.ThrowIfLessThan(argument, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsLessThan<T>(T argument, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IComparable<T>
		=> ArgOutOfRangeException.ThrowIfLessThan(argument, other, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsLessThanOrEqual<T>(T argument, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IComparable<T>
		=> ArgOutOfRangeException.ThrowIfLessThanOrEqual(argument, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfArgumentIsLessThanOrEqual<T>(T argument, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IComparable<T>
		=> ArgOutOfRangeException.ThrowIfLessThanOrEqual(argument, other, errorCode: null, detail: null, scopeContext, paramName);




	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ArgumentException<T>(T value, IErrorCode? errorCode, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.ArgException.Throw(paramName, errorCode, detail: null, scopeContext);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ArgumentException<T>(T value, IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.ArgException.Throw(paramName, errorCode, detail, scopeContext);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ArgumentException<T>(T value, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.ArgException.Throw(paramName, ErrorCodes.ArgException.Default, detail, scopeContext);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ArgumentException<T>(T value, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.ArgException.Throw(paramName, ErrorCodes.ArgException.Default, detail: null, scopeContext);




	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ArgumentOutOfRangeException<T>(T value, IErrorCode? errorCode, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.ArgOutOfRangeException.Throw(paramName, errorCode, detail: null, scopeContext);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ArgumentOutOfRangeException<T>(T value, IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.ArgOutOfRangeException.Throw(paramName, errorCode, detail, scopeContext);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ArgumentOutOfRangeException<T>(T value, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.ArgOutOfRangeException.Throw(paramName, ErrorCodes.OutOfRangeException.Default, detail, scopeContext);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ArgumentOutOfRangeException<T>(T value, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.ArgOutOfRangeException.Throw(paramName, ErrorCodes.OutOfRangeException.Default, detail: null, scopeContext);





	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use IfNullOrEmpty or IfNullOrWhiteSpace instead. Do not use with nameof()"
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_NullEx_Str")]
#else
	)]
#endif
	public static string? IfNull([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use IfNullOrEmpty or IfNullOrWhiteSpace instead. Do not use with nameof()"
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_NullEx_Str")]
#else
	)]
#endif
	public static string? IfNull([NotNull] string? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> NullValueException.ThrowIfNull(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T? IfNull<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T? IfNull<T>([NotNull] T? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> NullValueException.ThrowIfNull(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static object? IfNull([NotNull] object? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static object? IfNull([NotNull] object? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> NullValueException.ThrowIfNull(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static unsafe void IfNull([NotNull] void* argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static unsafe void IfNull([NotNull] void* argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> NullValueException.ThrowIfNull(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static unsafe void IfNull(IntPtr argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static unsafe void IfNull(IntPtr argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> NullValueException.ThrowIfNull(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfDefault<T>([NotNull] T argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> DefaultValueException.ThrowIfDefault(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfDefault<T>([NotNull] T argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> DefaultValueException.ThrowIfDefault(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfDefault<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> DefaultValueException.ThrowIfDefault(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfDefault<T>([NotNull] T? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> DefaultValueException.ThrowIfDefault(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T? IfNullOrDefault<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> DefaultValueException.ThrowIfNullOrDefault(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T? IfNullOrDefault<T>([NotNull] T? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
		=> DefaultValueException.ThrowIfNullOrDefault(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static string? IfNullOrEmpty([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> EmptyValueException.ThrowIfNullOrEmpty(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static string? IfNullOrEmpty([NotNull] string? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> EmptyValueException.ThrowIfNullOrEmpty(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static string? IfNullOrWhiteSpace([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> EmptyValueException.ThrowIfNullOrWhiteSpace(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static string? IfNullOrWhiteSpace([NotNull] string? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> EmptyValueException.ThrowIfNullOrWhiteSpace(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static ICollection? IfNullOrEmpty([NotNull] ICollection? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> EmptyValueException.ThrowIfNullOrEmpty(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static ICollection? IfNullOrEmpty([NotNull] ICollection? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> EmptyValueException.ThrowIfNullOrEmpty(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static Array? IfNullOrEmpty([NotNull] Array? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> EmptyValueException.ThrowIfNullOrEmpty(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static Array? IfNullOrEmpty([NotNull] Array? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> EmptyValueException.ThrowIfNullOrEmpty(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static IEnumerable? IfNullOrEmpty([NotNull] IEnumerable? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> EmptyValueException.ThrowIfNullOrEmpty(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static IEnumerable? IfNullOrEmpty([NotNull] IEnumerable? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		=> EmptyValueException.ThrowIfNullOrEmpty(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T? IfNullOrEmpty<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEnumerable?
		=> EmptyValueException.ThrowIfNullOrEmpty(argument, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static T? IfNullOrEmpty<T>([NotNull] T? argument, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEnumerable?
		=> EmptyValueException.ThrowIfNullOrEmpty(argument, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfZero<T>(T value, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> Exceptions.OutOfRangeException.ThrowIfZero(value, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfZero<T>(T value, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> Exceptions.OutOfRangeException.ThrowIfZero(value, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfNegative<T>(T value, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> Exceptions.OutOfRangeException.ThrowIfNegative(value, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfNegative<T>(T value, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> Exceptions.OutOfRangeException.ThrowIfNegative(value, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfNegativeOrZero<T>(T value, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> Exceptions.OutOfRangeException.ThrowIfNegativeOrZero(value, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfNegativeOrZero<T>(T value, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
#else
		where T : IComparable<T>
#endif
		=> Exceptions.OutOfRangeException.ThrowIfNegativeOrZero(value, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
		=> Exceptions.OutOfRangeException.ThrowIfEqual(value, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfEqual<T>(T value, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
		=> Exceptions.OutOfRangeException.ThrowIfEqual(value, other, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfNotEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
		=> Exceptions.OutOfRangeException.ThrowIfNotEqual(value, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfNotEqual<T>(T value, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
		=> Exceptions.OutOfRangeException.ThrowIfNotEqual(value, other, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfContainsIn<T>(T value, IEnumerable<T> others, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
		=> Exceptions.OutOfRangeException.ThrowIfContainsIn(value, others, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfContainsIn<T>(T value, IEnumerable<T> others, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
		=> Exceptions.OutOfRangeException.ThrowIfContainsIn(value, others, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfNotContainsIn<T>(T value, IEnumerable<T> others, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
		=> Exceptions.OutOfRangeException.ThrowIfNotContainsIn(value, others, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfNotContainsIn<T>(T value, IEnumerable<T> others, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IEquatable<T>?
		=> Exceptions.OutOfRangeException.ThrowIfNotContainsIn(value, others, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfGreaterThan<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
		=> Exceptions.OutOfRangeException.ThrowIfGreaterThan(value, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfGreaterThan<T>(T value, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
		=> Exceptions.OutOfRangeException.ThrowIfGreaterThan(value, other, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfGreaterThanOrEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
		=> Exceptions.OutOfRangeException.ThrowIfGreaterThanOrEqual(value, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfGreaterThanOrEqual<T>(T value, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
		=> Exceptions.OutOfRangeException.ThrowIfGreaterThanOrEqual(value, other, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfLessThan<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
		=> Exceptions.OutOfRangeException.ThrowIfLessThan(value, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfLessThan<T>(T value, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
		=> Exceptions.OutOfRangeException.ThrowIfLessThan(value, other, errorCode: null, detail: null, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfLessThanOrEqual<T>(T value, T other, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
		=> Exceptions.OutOfRangeException.ThrowIfLessThanOrEqual(value, other, errorCode, detail, scopeContext, paramName);

	[System.Diagnostics.StackTraceHidden]
	public static void IfLessThanOrEqual<T>(T value, T other, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		where T : IComparable<T>
		=> Exceptions.OutOfRangeException.ThrowIfLessThanOrEqual(value, other, errorCode: null, detail: null, scopeContext, paramName);




	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void OutOfRangeException<T>(T value, IErrorCode? errorCode, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.OutOfRangeException.Throw(paramName, errorCode, detail: null, scopeContext);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void OutOfRangeException<T>(T value, IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.OutOfRangeException.Throw(paramName, errorCode, detail, scopeContext);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void OutOfRangeException<T>(T value, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.OutOfRangeException.Throw(paramName, ErrorCodes.OutOfRangeException.Default, detail, scopeContext);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void OutOfRangeException<T>(T value, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.OutOfRangeException.Throw(paramName, ErrorCodes.OutOfRangeException.Default, detail: null, scopeContext);





	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void AuthenticationException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.AuthenticationException.Throw(errorCode, detail: null, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void AuthenticationException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.AuthenticationException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void AuthenticationException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.AuthenticationException.Throw(ErrorCodes.AuthenticationException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void AuthenticationException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.AuthenticationException.Throw(ErrorCodes.AuthenticationException.Default, detail: null, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void UnauthorizedException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.UnauthorizedException.Throw(errorCode, detail: null, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void UnauthorizedException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.UnauthorizedException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void UnauthorizedException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.UnauthorizedException.Throw(ErrorCodes.UnauthorizedException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void UnauthorizedException(
		Func<Identity.LegionIdentity, bool> permissonDelegate,
		IScopeContext? scopeContext = null,
		Exception? innerException = null,
		[CallerArgumentExpression(nameof(permissonDelegate))] string? permissonDelegateName = null)
		=> Exceptions.UnauthorizedException.Throw(ErrorCodes.UnauthorizedException.Default, permissonDelegateName, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void UnauthorizedException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.UnauthorizedException.Throw(ErrorCodes.UnauthorizedException.Default, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ConfigurationException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ConfigurationException.Throw(errorCode, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ConfigurationException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ConfigurationException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ConfigurationException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ConfigurationException.Throw(ErrorCodes.ConfigurationException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ConfigurationException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.ConfigurationException.Throw(ErrorCodes.ConfigurationException.Default, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void DecorationException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.DecorationException.Throw(errorCode, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void DecorationException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.DecorationException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void DecorationException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.DecorationException.Throw(ErrorCodes.DecorationException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void DecorationException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.DecorationException.Throw(ErrorCodes.DecorationException.Default, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void InitializationException<T>(T value, IScopeContext? scopeContext, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.InitializationException.Throw(paramName!, detail: null, scopeContext);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void InitializationException<T>(T value, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(value))] string? paramName = null)
		=> Exceptions.InitializationException.Throw(paramName!, detail, scopeContext);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void InitializationException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.InitializationException.Throw(errorCode, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void InitializationException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.InitializationException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void InitializationException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.InitializationException.Throw(ErrorCodes.InitializationException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void InitializationException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.InitializationException.Throw(ErrorCodes.InitializationException.Default, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void InvalidOperationException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.InvalidOpException.Throw(errorCode, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void InvalidOperationException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.InvalidOpException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void InvalidOperationException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.InvalidOpException.Throw(ErrorCodes.InvalidOpException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void InvalidOperationException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.InvalidOpException.Throw(ErrorCodes.InvalidOpException.Default, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void OperationCanceledException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.OpCanceledException.Throw(errorCode, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void OperationCanceledException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.OpCanceledException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void OperationCanceledException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.OpCanceledException.Throw(ErrorCodes.OpCanceledException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void OperationCanceledException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.OpCanceledException.Throw(ErrorCodes.OpCanceledException.Default, detail: null, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	public static void IfCancellationRequested(CancellationToken cancellationToken, string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
	{
		if (cancellationToken == default || !cancellationToken.IsCancellationRequested)
			return;

		Exceptions.OpCanceledException.Throw(ErrorCodes.OpCanceledException.Default, detail, scopeContext, innerException);
	}

	[System.Diagnostics.StackTraceHidden]
	public static void IfCancellationRequested(CancellationToken cancellationToken, IScopeContext? scopeContext, Exception? innerException = null)
	{
		if (cancellationToken == default || !cancellationToken.IsCancellationRequested)
			return;

		Exceptions.OpCanceledException.Throw(ErrorCodes.OpCanceledException.Default, detail: null, scopeContext, innerException);
	}


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ObjectDisposedException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ObjDisposedException.Throw(errorCode, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ObjectDisposedException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ObjDisposedException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ObjectDisposedException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ObjDisposedException.Throw(ErrorCodes.ObjDisposedException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ObjectDisposedException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.ObjDisposedException.Throw(ErrorCodes.ObjDisposedException.Default, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void NotSupportedException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.NotSupportedException.Throw(errorCode, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void NotSupportedException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.NotSupportedException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void NotSupportedException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.NotSupportedException.Throw(ErrorCodes.NotSupportedException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void NotSupportedException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.NotSupportedException.Throw(ErrorCodes.NotSupportedException.Default, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void NotImplementedException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.NotImplementedException.Throw(errorCode, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void NotImplementedException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.NotImplementedException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void NotImplementedException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.NotImplementedException.Throw(ErrorCodes.NotImplementedException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void NotImplementedException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.NotImplementedException.Throw(ErrorCodes.NotImplementedException.Default, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ResultException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ResultException.Throw(errorCode, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ResultException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ResultException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ResultException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ResultException.Throw(ErrorCodes.ResultException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ResultException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.ResultException.Throw(ErrorCodes.ResultException.Default, detail: null, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ResultExceptionIfHasError(IScopeContext scopeContext, IErrorCode errorCode, IResult result, bool dataMustBeNotNull, bool withErrorMessageDetails)
		=> ExceptionHelper.ToException(scopeContext, errorCode, result, dataMustBeNotNull, withErrorMessageDetails);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void TransactionException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.TransactionException.Throw(errorCode, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void TransactionException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.TransactionException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void TransactionException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.TransactionException.Throw(ErrorCodes.TransactionException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void TransactionException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.TransactionException.Throw(ErrorCodes.TransactionException.Default, detail: null, scopeContext, innerException);


	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ValidationException(IErrorCode? errorCode, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ValidationException.Throw(errorCode, detail: null, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ValidationException(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ValidationException.Throw(errorCode, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ValidationException(string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> Exceptions.ValidationException.Throw(ErrorCodes.ValidationException.Default, detail, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ValidationException(IScopeContext? scopeContext, Exception? innerException = null)
		=> Exceptions.ValidationException.Throw(ErrorCodes.ValidationException.Default, detail: null, scopeContext, innerException);

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void ValidationException(
		IValidationResult validationResult,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		bool clientMessageWithPropertyName = true,
		bool withErrorMessageDetails = false)
	{
		var validationException = validationResult?.ToException(scopeContext, errorCode, clientMessageWithPropertyName, withErrorMessageDetails);

		if (validationException == null)
			Exceptions.ValidationException.Throw(ErrorCodes.ValidationException.Default, detail: null, scopeContext, innerException: null);

		throw validationException;
	}


	//TODO: REMINDER: dopln vsetko aj do IResultExtensions
}
