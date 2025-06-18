using Legion.Identity;
using Legion.Logging;
using Legion.Web;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Legion;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IScopeContext
{
	//NECHCEM TO TU DAVAT KVOLI DISPOSE NAD TransactionsControllerom, aby neexistovalo vela objektov, ktore maju referencie na TransactionsController
	//Transactions.ITransactionsController TransactionsController { get; }

	Guid RuntimeUniqueKey { get; }

	string SourceSystemName { get; }

	string? BusinessProcess { get; }

	string? Component { get; }

	Guid? TenantIdentifier { get; }

	TraceFrameStack TraceFrameStack { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	LegionPrincipal? Principal { get; }
	bool ShouldSerializePrincipal();

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	LegionIdentity? User { get; }
	bool ShouldSerializeUser();

	Guid? IdUser { get; }

	/// <summary>
	/// Usualy HttpContext.TraceIdentifier, which is marked as external because can be changed
	/// by RequestCorrelationMiddleware based on Request header (DefaultHeader = "X-Correlation-ID") from client - it may be not unique
	/// or can be changed by another middleware / filter
	/// </summary>
	string? ExternalCorrelationId { get; }

	/// <summary>
	/// Usualy HttpContext.Item[X-Correlation-ID] set by RequestCorrelationMiddleware
	/// It is unique identifier for current request
	/// </summary>
	Guid? CorrelationId { get; }

	Guid? IdApplicationEntry { get; }

	string? CustomCorrelationId { get; }

	Guid TraceCorrelationId { get; }

	IReadOnlyDictionary<string, string?> ContextProperties { get; }

	IRequestMetadata? RequestMetadata { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	CultureInfo? CurrentCulture { get; }
	bool ShouldSerializeCurrentCulture();

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	ILogger? Logger { get; }
	bool ShouldSerializeLogger();

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	CancellationToken? CancellationToken { get; }
	bool ShouldSerializeCancellationToken();


	bool TryGetGlobalItem(string name, out object? value);

	bool TryGetGlobalItem<T>(string name, out T? value);

	object? GetOrAddGlobalItem(string name, object? data);

	void AddOrUpdateGlobalItem(string name, object? data);

	bool TryGetLocalInheritableItem(string name, out object? value);

	bool TryGetLocalInheritableItem<T>(string name, out T? value);

	object? GetOrAddLocalInheritableItem(string name, object? data);

	void AddOrUpdateLocalInheritableItem(string name, object? data);

	string ContextPropertiesToJson();

	string ToStringTrace();

	IScopeContext AddContextProperty(
		string key,
		string? value,
		bool force = false,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase);

	IScopeContext RemoveContextProperty(
		string key,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase);

	IScopeContext AppendTraceFrame(
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrame(TraceFrame traceFrame, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithPrincipal(
		LegionPrincipal? principal,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithPrincipal(TraceFrame traceFrame, LegionPrincipal? principal, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithIduser(
		Guid? idUser,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithIduser(TraceFrame traceFrame, Guid? idUser, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithRequestMetadata(
		IRequestMetadata? requestMetadata,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithRequestMetadata(TraceFrame traceFrame, IRequestMetadata? requestMetadata, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithCultureInfo(
		CultureInfo? cultureInfo,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithCultureInfo(TraceFrame traceFrame, CultureInfo? cultureInfo, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithBusinessProcess(
		string businessProcess,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithBusinessProcess(TraceFrame traceFrame, string businessProcess, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithComponent(
		string component,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithComponent(TraceFrame traceFrame, string component, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithTenantIdentifier(
		Guid? tenantIdentifier,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithTenantIdentifier(TraceFrame traceFrame, Guid? tenantIdentifier, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithIdApplicationEntry(
		Guid? idApplicationEntry,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithIdApplicationEntry(TraceFrame traceFrame, Guid? idApplicationEntry, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithCustomCorrelationId(
		string? customCorrelationId,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithCustomCorrelationId(TraceFrame traceFrame, string? customCorrelationId, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithLogger(
		ILogger? logger,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithLogger(TraceFrame traceFrame, ILogger? logger, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrameWithCancellationToken(
		CancellationToken? cancellationToken,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext AppendTraceFrameWithCancellationToken(TraceFrame traceFrame, CancellationToken? cancellationToken, bool force, bool removePreviousSameMethodFrame = true);

	IScopeContext AppendTraceFrame(
		LegionPrincipal? principal,
		Guid? idUser,
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

	IScopeContext AppendTraceFrame(
		TraceFrame traceFrame,
		LegionPrincipal? principal,
		Guid? idUser,
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
	[Obsolete("Use AppendTraceFrameWithPrincipal instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext SetPrincipal(LegionPrincipal? principal, bool force);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithIduser instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext SetIduser(Guid? idUser, bool force);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithRequestMetadata instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext SetRequestMetadata(IRequestMetadata? requestMetadata, bool force);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithCultureInfo instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext SetCultureInfo(CultureInfo? cultureInfo, bool force);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithBusinessProcess instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext SetBusinessProcess(string businessProcess, bool force);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithComponent instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext SetComponent(string component, bool force);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithTenantIdentifier instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext SetTenantIdentifier(Guid? tenantIdentifier, bool force);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithCustomCorrelationId instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext SetCustomCorrelationId(string? customCorrelationId, bool force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithLogger instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext SetLogger(ILogger? logger, bool force);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithCancellationToken instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext SetCancellationToken(CancellationToken? cancellationToken, bool force);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrame instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext Set(
		LegionPrincipal? principal,
		Guid? idUser,
		string? businessProcess,
		string? component,
		Guid? tenantIdentifier,
		string? customCorrelationId,
		ILogger? logger,
		CultureInfo? cultureInfo,
		IRequestMetadata? requestMetadata,
		CancellationToken? cancellationToken,
		bool force);



	IScopeContext CreateNew(
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNew(TraceFrame traceFrame, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithPrincipal(
		LegionPrincipal? principal,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithPrincipal(TraceFrame traceFrame, LegionPrincipal? principal, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithIduser(
		Guid? idUser,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithIduser(TraceFrame traceFrame, Guid? idUser, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithRequestMetadata(
		IRequestMetadata? requestMetadata,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithRequestMetadata(TraceFrame traceFrame, IRequestMetadata? requestMetadata, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithCultureInfo(
		CultureInfo? cultureInfo,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithCultureInfo(TraceFrame traceFrame, CultureInfo? cultureInfo, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithBusinessProcess(
		string businessProcess,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithBusinessProcess(TraceFrame traceFrame, string businessProcess, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithComponent(
		string component,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithComponent(TraceFrame traceFrame, string component, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithTenantIdentifier(
		Guid? tenantIdentifier,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithTenantIdentifier(TraceFrame traceFrame, Guid? tenantIdentifier, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithCustomCorrelationId(
		string? customCorrelationId,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithCustomCorrelationId(TraceFrame traceFrame, string? customCorrelationId, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithLogger(
		ILogger? logger,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithLogger(TraceFrame traceFrame, ILogger? logger, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithCancellationToken(
		CancellationToken? cancellationToken,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithCancellationToken(TraceFrame traceFrame, CancellationToken? cancellationToken, bool removePreviousSameMethodFrame = true);

	IScopeContext CreateNewWithContextProperty(
		string contextPropertyKey,
		string? contextPropertyValue,
		bool removePreviousSameMethodFrame = true,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IScopeContext CreateNewWithContextProperty(
		TraceFrame traceFrame,
		string contextPropertyKey,
		string? contextPropertyValue,
		bool removePreviousSameMethodFrame = true,
		bool force = false,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase);

	IScopeContext CreateNewWith(
		Guid? correlationId = null,
		LegionPrincipal? principal = null,
		Guid? idUser = null,
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

	IScopeContext CreateNewWith(
		TraceFrame traceFrame,
		Guid? correlationId = null,
		LegionPrincipal? principal = null,
		Guid? idUser = null,
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

	IDisposable? CreateLoggerScope();

	IResult<bool> LogMessage(
		ILogMessage logMessage,
		bool skipIfAlreadyLogged = true);

	IResult<bool> LogTraceMessage(
		ILogMessage traceMessage,
		bool skipIfAlreadyLogged = true);

	IResult<bool> LogDebugMessage(
		ILogMessage debugMessage,
		bool skipIfAlreadyLogged = true);

	IResult<bool> LogInformationMessage(
		ILogMessage infoMessage,
		bool skipIfAlreadyLogged = true);

	IResult<bool> LogWarningMessage(
		ILogMessage warningMessage,
		bool skipIfAlreadyLogged = true);

	IResult<bool> LogErrorMessage(
		IErrorMessage errorMessage,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true);

	IResult<bool> LogCriticalMessage(
		IErrorMessage criticalMessage,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true);

	IResult<bool> LogResultAllMessages(
		IResult result,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true);

	IResult<bool> LogResultErrorMessages(
		IResult result,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true);

	string? GetLastTraceFrame();
}
