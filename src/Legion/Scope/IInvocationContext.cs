using Legion.Identity;
using Legion.Web;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Legion;

public interface IInvocationContext : IScopeContext
{
	IServiceProvider? ServiceProvider { get; }
	Func<IInvocationResult, CancellationToken, Task>? InvocationResultAsyncCallback { get; }
	Action<IInvocationResult>? InvocationResultSyncCallback { get; }
	IErrorCode? UnhandledErrorCode { get; }
	string? DefaultClientErrorMessage { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	string? TargetStoreId { get; }
	bool ShouldSerializeTargetStoreId();


	IInvocationContext InvocationAddContextProperty(
		string key,
		string? value,
		bool force = false,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase);

	IInvocationContext InvocationRemoveContextProperty(
		string key,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase);

	IInvocationContext InvocationAppendTraceFrame(
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrame(TraceFrame traceFrame, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithPrincipal(
		LegionPrincipal? principal,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithPrincipal(TraceFrame traceFrame, LegionPrincipal? principal, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithIduser(
		Guid? idUser,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithIduser(TraceFrame traceFrame, Guid? idUser, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithRequestMetadata(
		IRequestMetadata? requestMetadata,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithRequestMetadata(TraceFrame traceFrame, IRequestMetadata? requestMetadata, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithCultureInfo(
		CultureInfo? cultureInfo,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithCultureInfo(TraceFrame traceFrame, CultureInfo? cultureInfo, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithTargetStoreId(
		string? targetStoreId,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithTargetStoreId(TraceFrame traceFrame, string? targetStoreId, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithBusinessProcess(
		string businessProcess,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithBusinessProcess(TraceFrame traceFrame, string businessProcess, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithComponent(
		string component,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithComponent(TraceFrame traceFrame, string component, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithTenantIdentifier(
		Guid? tenantIdentifier,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithTenantIdentifier(TraceFrame traceFrame, Guid? tenantIdentifier, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithIdApplicationEntry(
		Guid? idApplicationEntry,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithIdApplicationEntry(TraceFrame traceFrame, Guid? idApplicationEntry, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithCustomCorrelationId(
		string? customCorrelationId,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithCustomCorrelationId(TraceFrame traceFrame, string? customCorrelationId, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithLogger(
		ILogger? logger,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithLogger(TraceFrame traceFrame, ILogger? logger, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrameWithCancellationToken(
		CancellationToken? cancellationToken,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrameWithCancellationToken(TraceFrame traceFrame, CancellationToken? cancellationToken, bool force, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationAppendTraceFrame(
		LegionPrincipal? principal,
		Guid? idUser,
		string? targetStoreId,
		string? businessProcess,
		string? component,
		Guid? tenantIdentifier,
		string? customCorrelationId,
		ILogger? logger,
		CultureInfo? cultureInfo,
		IRequestMetadata? requestMetadata,
		CancellationToken? cancellationToken,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationAppendTraceFrame(
		TraceFrame traceFrame,
		LegionPrincipal? principal,
		Guid? idUser,
		string? targetStoreId,
		string? businessProcess,
		string? component,
		Guid? tenantIdentifier,
		string? customCorrelationId,
		ILogger? logger,
		CultureInfo? cultureInfo,
		IRequestMetadata? requestMetadata,
		CancellationToken? cancellationToken,
		bool force,
		bool removePreviousSameMethodFrame = true);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithPrincipal instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetPrincipal(LegionPrincipal? principal, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithIduser instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetIduser(Guid? idUser, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithRequestMetadata instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetRequestMetadata(IRequestMetadata? requestMetadata, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithCultureInfo instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetCultureInfo(CultureInfo? cultureInfo, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithTargetStoreId instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetTargetStoreId(string? targetStoreId, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithBusinessProcess instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetBusinessProcess(string businessProcess, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithComponent instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetComponent(string component, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithTenantIdentifier instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetTenantIdentifier(Guid? tenantIdentifier, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithCustomCorrelationId instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetCustomCorrelationId(string? customCorrelationId, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithLogger instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetLogger(ILogger? logger, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithCancellationToken instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSetCancellationToken(CancellationToken? cancellationToken, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrame instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IInvocationContext InvocationSet(
		LegionPrincipal? principal,
		Guid? idUser,
		string? targetStoreId,
		string? businessProcess,
		string? component,
		Guid? tenantIdentifier,
		string? customCorrelationId,
		ILogger? logger,
		CultureInfo? cultureInfo,
		IRequestMetadata? requestMetadata,
		CancellationToken? cancellationToken,
		bool force);


	IInvocationContext InvocationCreateNew(
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNew(TraceFrame traceFrame, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithPrincipal(
		LegionPrincipal? principal,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithPrincipal(TraceFrame traceFrame, LegionPrincipal? principal, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithIduser(
		Guid? idUser,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithIduser(TraceFrame traceFrame, Guid? idUser, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithRequestMetadata(
		IRequestMetadata? requestMetadata,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithRequestMetadata(TraceFrame traceFrame, IRequestMetadata? requestMetadata, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithCultureInfo(
		CultureInfo? cultureInfo,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithCultureInfo(TraceFrame traceFrame, CultureInfo? cultureInfo, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithTargetStoreId(
		string? targetStoreId,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithTargetStoreId(TraceFrame traceFrame, string? targetStoreId, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithBusinessProcess(
		string businessProcess,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithBusinessProcess(TraceFrame traceFrame, string businessProcess, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithComponent(
		string component,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithComponent(TraceFrame traceFrame, string component, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithTenantIdentifier(
		Guid? tenantIdentifier,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithTenantIdentifier(TraceFrame traceFrame, Guid? tenantIdentifier, bool removePreviousSameMethodFrame = true);


	IInvocationContext InvocationCreateNewWithCustomCorrelationId(
		string? customCorrelationId,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithCustomCorrelationId(TraceFrame traceFrame, string? customCorrelationId, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithLogger(
		ILogger? logger,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithLogger(TraceFrame traceFrame, ILogger? logger, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithCancellationToken(
		CancellationToken? cancellationToken,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithCancellationToken(TraceFrame traceFrame, CancellationToken? cancellationToken, bool removePreviousSameMethodFrame = true);

	IInvocationContext InvocationCreateNewWithContextProperty(
		string contextPropertyKey,
		string? contextPropertyValue,
		bool removePreviousSameMethodFrame = true,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWithContextProperty(
		TraceFrame traceFrame,
		string contextPropertyKey,
		string? contextPropertyValue,
		bool removePreviousSameMethodFrame = true,
		bool force = false,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase);

	IInvocationContext InvocationCreateNewWith(
		Guid? correlationId = null,
		LegionPrincipal? principal = null,
		Guid? idUser = null,
		string? targetStoreId = null,
		string? businessProcess = null,
		string? component = null,
		Guid? tenantIdentifier = null,
		string? externalCorrelationId = null,
		string? customCorrelationId = null,
		ILogger? logger = null,
		CultureInfo? cultureInfo = null,
		IRequestMetadata? requestMetadata = null,
		CancellationToken? cancellationToken = null,
		bool removePreviousSameMethodFrame = true,
		bool forceNullValues = false,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IInvocationContext InvocationCreateNewWith(
		TraceFrame traceFrame,
		Guid? correlationId = null,
		LegionPrincipal? principal = null,
		Guid? idUser = null,
		string? targetStoreId = null,
		string? businessProcess = null,
		string? component = null,
		Guid? tenantIdentifier = null,
		string? externalCorrelationId = null,
		string? customCorrelationId = null,
		ILogger? logger = null,
		CultureInfo? cultureInfo = null,
		IRequestMetadata? requestMetadata = null,
		CancellationToken? cancellationToken = null,
		bool removePreviousSameMethodFrame = true,
		bool forceNullValues = false);

	IInvocationContext InvocationCreateNewWithResultCallback(
		Func<IInvocationResult, CancellationToken, Task> invocationResultAsyncCallback,
		Action<IInvocationResult> invocationResultSyncCallback);

	IInvocationContext InvocationCreateNewWithUnhandledErrorCode(IErrorCode unhandledErrorCode);

	IInvocationContext InvocationCreateNewWithDefaultClientErrorMessage(string defaultClientErrorMessage);
}
