using Legion.Identity;
using Legion.Logging;
using Legion.Web;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Legion;

public class InvocationContext : IInvocationContext, IScopeContext
{
	private IScopeContext _scopeContext;

	public IServiceProvider? ServiceProvider { get; protected internal set; }
	public Func<IInvocationResult, CancellationToken, Task>? InvocationResultAsyncCallback { get; protected internal set; }
	public Action<IInvocationResult>? InvocationResultSyncCallback { get; protected internal set; }
	public IErrorCode? UnhandledErrorCode { get; protected internal set; }
	public string? DefaultClientErrorMessage { get; protected internal set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public string? TargetStoreId { get; private set; }
	public bool ShouldSerializeTargetStoreId() => false;

	public Guid RuntimeUniqueKey => _scopeContext.RuntimeUniqueKey;

	public string SourceSystemName => _scopeContext.SourceSystemName;

	public string? BusinessProcess => _scopeContext.BusinessProcess;

	public string? Component => _scopeContext.Component;

	public Guid? TenantIdentifier => _scopeContext.TenantIdentifier;

	public TraceFrameStack TraceFrameStack => _scopeContext.TraceFrameStack;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public LegionPrincipal? Principal => _scopeContext.Principal;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public LegionIdentity? User => _scopeContext.User;

	public Guid? IdUser => _scopeContext.IdUser;

	public string? ExternalCorrelationId => _scopeContext.ExternalCorrelationId;

	public Guid? CorrelationId => _scopeContext.CorrelationId;

	public Guid? IdApplicationEntry => _scopeContext.IdApplicationEntry;

	public string? CustomCorrelationId => _scopeContext.CustomCorrelationId;

	public Guid TraceCorrelationId => _scopeContext.TraceCorrelationId;

	public IReadOnlyDictionary<string, string?> ContextProperties => _scopeContext.ContextProperties;

	public IRequestMetadata? RequestMetadata => _scopeContext.RequestMetadata;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public CultureInfo? CurrentCulture => _scopeContext.CurrentCulture;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public ILogger? Logger => _scopeContext.Logger;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public CancellationToken? CancellationToken => _scopeContext.CancellationToken;

	public InvocationContext(IScopeContext scopeContext, IErrorCode? unhandledErrorCode = null)
	{
		Throw.IfArgumentNull(scopeContext);

		_scopeContext = scopeContext;
		UnhandledErrorCode = unhandledErrorCode;
	}

	public bool ShouldSerializePrincipal()
		=> _scopeContext.ShouldSerializePrincipal();

	public bool ShouldSerializeUser()
		=> _scopeContext.ShouldSerializeUser();

	public bool ShouldSerializeCurrentCulture()
		=> _scopeContext.ShouldSerializeCurrentCulture();

	public bool ShouldSerializeLogger()
		=> _scopeContext.ShouldSerializeLogger();

	public bool ShouldSerializeCancellationToken()
		=> _scopeContext.ShouldSerializeCancellationToken();

	public bool TryGetGlobalItem(string name, out object? value)
		=> _scopeContext.TryGetGlobalItem(name, out value);

	public bool TryGetGlobalItem<T>(string name, out T? value)
		=> _scopeContext.TryGetGlobalItem(name, out value);

	public object? GetOrAddGlobalItem(string name, object? data)
		=> _scopeContext.GetOrAddGlobalItem(name, data);

	public void AddOrUpdateGlobalItem(string name, object? data)
		=> _scopeContext.AddOrUpdateGlobalItem(name, data);

	public bool TryGetLocalInheritableItem(string name, out object? value)
		=> _scopeContext.TryGetLocalInheritableItem(name, out value);

	public bool TryGetLocalInheritableItem<T>(string name, out T? value)
		=> _scopeContext.TryGetLocalInheritableItem(name, out value);

	public object? GetOrAddLocalInheritableItem(string name, object? data)
		=> _scopeContext.GetOrAddLocalInheritableItem(name, data);

	public void AddOrUpdateLocalInheritableItem(string name, object? data)
		=> _scopeContext.AddOrUpdateLocalInheritableItem(name, data);

	public string ContextPropertiesToJson()
		=> _scopeContext.ContextPropertiesToJson();

	public string ToStringTrace()
		=> _scopeContext.ToStringTrace();

	IScopeContext IScopeContext.AddContextProperty(
		string key,
		string? value,
		bool force,
		StringComparison comparison)
		=> _scopeContext.AddContextProperty(key, value, force, comparison);

	IScopeContext IScopeContext.RemoveContextProperty(
		string key,
		StringComparison comparison)
		=> _scopeContext.RemoveContextProperty(key, comparison);

	IScopeContext IScopeContext.AppendTraceFrame(
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrame(
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrame(TraceFrame traceFrame, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrame(traceFrame, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithPrincipal(
		LegionPrincipal? principal,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithPrincipal(
			principal,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithPrincipal(TraceFrame traceFrame, LegionPrincipal? principal, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithPrincipal(traceFrame, principal, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithIduser(
		Guid? idUser,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithIduser(
			idUser,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithIduser(TraceFrame traceFrame, Guid? idUser, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithIduser(traceFrame, idUser, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithRequestMetadata(
		IRequestMetadata? requestMetadata,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithRequestMetadata(
			requestMetadata,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithRequestMetadata(TraceFrame traceFrame, IRequestMetadata? requestMetadata, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithRequestMetadata(traceFrame, requestMetadata, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithCultureInfo(
		CultureInfo? cultureInfo,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithCultureInfo(
			cultureInfo,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithCultureInfo(TraceFrame traceFrame, CultureInfo? cultureInfo, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithCultureInfo(traceFrame, cultureInfo, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithBusinessProcess(
		string businessProcess,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithBusinessProcess(
			businessProcess,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithBusinessProcess(TraceFrame traceFrame, string businessProcess, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithBusinessProcess(traceFrame, businessProcess, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithComponent(
		string component,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithComponent(
			component,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithComponent(TraceFrame traceFrame, string component, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithComponent(traceFrame, component, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithTenantIdentifier(
		Guid? tenantIdentifier,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithTenantIdentifier(
			tenantIdentifier,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithTenantIdentifier(TraceFrame traceFrame, Guid? tenantIdentifier, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithTenantIdentifier(traceFrame, tenantIdentifier, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithIdApplicationEntry(
		Guid? idApplicationEntry,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithIdApplicationEntry(
			idApplicationEntry,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithIdApplicationEntry(TraceFrame traceFrame, Guid? idApplicationEntry, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithIdApplicationEntry(traceFrame, idApplicationEntry, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithCustomCorrelationId(
		string? customCorrelationId,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithCustomCorrelationId(
			customCorrelationId,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithCustomCorrelationId(TraceFrame traceFrame, string? customCorrelationId, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithCustomCorrelationId(traceFrame, customCorrelationId, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithLogger(
		ILogger? logger,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithLogger(
			logger,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithLogger(TraceFrame traceFrame, ILogger? logger, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithLogger(traceFrame, logger, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrameWithCancellationToken(
		CancellationToken? cancellationToken,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrameWithCancellationToken(
			cancellationToken,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrameWithCancellationToken(TraceFrame traceFrame, CancellationToken? cancellationToken, bool force, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrameWithCancellationToken(traceFrame, cancellationToken, force, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.AppendTraceFrame(
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
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.AppendTraceFrame(
			principal,
			idUser,
			businessProcess,
			component,
			tenantIdentifier,
			customCorrelationId,
			logger,
			cultureInfo,
			requestMetadata,
			cancellationToken,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.AppendTraceFrame(
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
		bool removePreviousSameMethodFrame = true)
		=> _scopeContext.AppendTraceFrame(
			traceFrame,
			principal,
			idUser,
			businessProcess,
			component,
			tenantIdentifier,
			customCorrelationId,
			logger,
			cultureInfo,
			requestMetadata,
			cancellationToken,
			force,
			removePreviousSameMethodFrame);


	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithPrincipal instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.SetPrincipal(LegionPrincipal? principal, bool force)
		=> _scopeContext.SetPrincipal(
			principal,
			force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithIduser instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.SetIduser(Guid? idUser, bool force)
		=> _scopeContext.SetIduser(
			idUser,
			force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithRequestMetadata instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.SetRequestMetadata(IRequestMetadata? requestMetadata, bool force)
		=> _scopeContext.SetRequestMetadata(
			requestMetadata,
			force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithCultureInfo instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.SetCultureInfo(CultureInfo? cultureInfo, bool force)
		=> _scopeContext.SetCultureInfo(
			cultureInfo,
			force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithBusinessProcess instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.SetBusinessProcess(string businessProcess, bool force)
		=> _scopeContext.SetBusinessProcess(
			businessProcess,
			force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithComponent instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.SetComponent(string component, bool force)
		=> _scopeContext.SetComponent(
			component,
			force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithTenantIdentifier instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.SetTenantIdentifier(Guid? tenantIdentifier, bool force)
		=> _scopeContext.SetTenantIdentifier(
			tenantIdentifier,
			force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithCustomCorrelationId instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.SetCustomCorrelationId(string? customCorrelationId, bool force)
		=> _scopeContext.SetCustomCorrelationId(
			customCorrelationId,
			force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithLogger instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.SetLogger(ILogger? logger, bool force)
		=> _scopeContext.SetLogger(
			logger,
			force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithCancellationToken instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.SetCancellationToken(CancellationToken? cancellationToken, bool force)
		=> _scopeContext.SetCancellationToken(
			cancellationToken,
			force);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrame instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	IScopeContext IScopeContext.Set(
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
		bool force)
		=> _scopeContext.Set(
			principal,
			idUser,
			businessProcess,
			component,
			tenantIdentifier,
			customCorrelationId,
			logger,
			cultureInfo,
			requestMetadata,
			cancellationToken,
			force);



	IScopeContext IScopeContext.CreateNew(
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNew(
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNew(TraceFrame traceFrame, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNew(traceFrame, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithPrincipal(
		LegionPrincipal? principal,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithPrincipal(
			principal,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithPrincipal(TraceFrame traceFrame, LegionPrincipal? principal, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNewWithPrincipal(traceFrame, principal, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithIduser(
		Guid? idUser,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithIduser(
			idUser,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithIduser(TraceFrame traceFrame, Guid? idUser, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNewWithIduser(traceFrame, idUser, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithRequestMetadata(
		IRequestMetadata? requestMetadata,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithRequestMetadata(
			requestMetadata,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithRequestMetadata(TraceFrame traceFrame, IRequestMetadata? requestMetadata, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNewWithRequestMetadata(traceFrame, requestMetadata, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithCultureInfo(
		CultureInfo? cultureInfo,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithCultureInfo(
			cultureInfo,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithCultureInfo(TraceFrame traceFrame, CultureInfo? cultureInfo, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNewWithCultureInfo(traceFrame, cultureInfo, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithBusinessProcess(
		string businessProcess,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithBusinessProcess(
			businessProcess,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithBusinessProcess(TraceFrame traceFrame, string businessProcess, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNewWithBusinessProcess(traceFrame, businessProcess, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithComponent(
		string component,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithComponent(
			component,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithComponent(TraceFrame traceFrame, string component, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNewWithComponent(traceFrame, component, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithTenantIdentifier(
		Guid? tenantIdentifier,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithTenantIdentifier(
			tenantIdentifier,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithTenantIdentifier(TraceFrame traceFrame, Guid? tenantIdentifier, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNewWithTenantIdentifier(traceFrame, tenantIdentifier, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithCustomCorrelationId(
		string? customCorrelationId,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithCustomCorrelationId(
			customCorrelationId,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithCustomCorrelationId(TraceFrame traceFrame, string? customCorrelationId, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNewWithCustomCorrelationId(traceFrame, customCorrelationId, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithLogger(
		ILogger? logger,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithLogger(
			logger,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithLogger(TraceFrame traceFrame, ILogger? logger, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNewWithLogger(traceFrame, logger, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithCancellationToken(
		CancellationToken? cancellationToken,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithCancellationToken(
			cancellationToken,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithCancellationToken(TraceFrame traceFrame, CancellationToken? cancellationToken, bool removePreviousSameMethodFrame = true)
		=> _scopeContext.CreateNewWithCancellationToken(traceFrame, cancellationToken, removePreviousSameMethodFrame);

	IScopeContext IScopeContext.CreateNewWithContextProperty(
		string contextPropertyKey,
		string? contextPropertyValue,
		bool removePreviousSameMethodFrame = true,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> _scopeContext.CreateNewWithContextProperty(
			contextPropertyKey,
			contextPropertyValue,
			removePreviousSameMethodFrame,
			comparison,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWithContextProperty(
		TraceFrame traceFrame,
		string contextPropertyKey,
		string? contextPropertyValue,
		bool removePreviousSameMethodFrame = true,
		bool force = false,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
		=> _scopeContext.CreateNewWithContextProperty(traceFrame, contextPropertyKey, contextPropertyValue, removePreviousSameMethodFrame, force, comparison);

	IScopeContext IScopeContext.CreateNewWith(
		Guid? correlationId,
		LegionPrincipal? principal,
		Guid? idUser,
		string? businessProcess,
		string? component,
		Guid? tenantIdentifier,
		string? externalCorrelationId,
		string? customCorrelationId,
		ILogger? logger,
		CultureInfo? cultureInfo,
		IRequestMetadata? requestMetadata,
		CancellationToken? cancellationToken,
		bool removePreviousSameMethodFrame,
		bool forceNullValues,
		string memberName,
		string sourceFilePath,
		int sourceLineNumber)
		=> _scopeContext.CreateNewWith(
			correlationId,
			principal,
			idUser,
			businessProcess,
			component,
			tenantIdentifier,
			externalCorrelationId,
			customCorrelationId,
			logger,
			cultureInfo,
			requestMetadata,
			cancellationToken,
			removePreviousSameMethodFrame,
			forceNullValues,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	IScopeContext IScopeContext.CreateNewWith(
		TraceFrame traceFrame,
		Guid? correlationId,
		LegionPrincipal? principal,
		Guid? idUser,
		string? businessProcess,
		string? component,
		Guid? tenantIdentifier,
		string? externalCorrelationId,
		string? customCorrelationId,
		ILogger? logger,
		CultureInfo? cultureInfo,
		IRequestMetadata? requestMetadata,
		CancellationToken? cancellationToken,
		bool removePreviousSameMethodFrame,
		bool forceNullValues)
		=> _scopeContext.CreateNewWith(
			traceFrame,
			correlationId,
			principal,
			idUser,
			businessProcess,
			component,
			tenantIdentifier,
			externalCorrelationId,
			customCorrelationId,
			logger,
			cultureInfo,
			requestMetadata,
			cancellationToken,
			removePreviousSameMethodFrame,
			forceNullValues);


	private InvocationContext SetScopeContext(IScopeContext scopeContext, bool clone)
	{
		if (clone)
		{
			return new InvocationContext(scopeContext, UnhandledErrorCode)
			{
				ServiceProvider = ServiceProvider,
				InvocationResultAsyncCallback = InvocationResultAsyncCallback,
				InvocationResultSyncCallback = InvocationResultSyncCallback,
				DefaultClientErrorMessage = DefaultClientErrorMessage,
				TargetStoreId = TargetStoreId
			};
		}
		else
		{
			_scopeContext = scopeContext;
			return this;
		}
	}

	public IInvocationContext InvocationAddContextProperty(
		string key,
		string? value,
		bool force,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
		=> SetScopeContext(_scopeContext.AddContextProperty(key, value, force, comparison), false);

	public IInvocationContext InvocationRemoveContextProperty(
		string key,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
		=> SetScopeContext(_scopeContext.RemoveContextProperty(key, comparison), false);

	public IInvocationContext InvocationAppendTraceFrame(
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrame(
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrame(TraceFrame traceFrame, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrame(traceFrame, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithPrincipal(
		LegionPrincipal? principal,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithPrincipal(
			principal,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithPrincipal(TraceFrame traceFrame, LegionPrincipal? principal, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithPrincipal(traceFrame, principal, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithIduser(
		Guid? idUser,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithIduser(
			idUser,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithIduser(TraceFrame traceFrame, Guid? idUser, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithIduser(traceFrame, idUser, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithRequestMetadata(
		IRequestMetadata? requestMetadata,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithRequestMetadata(
			requestMetadata,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithRequestMetadata(TraceFrame traceFrame, IRequestMetadata? requestMetadata, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithRequestMetadata(traceFrame, requestMetadata, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithCultureInfo(
		CultureInfo? cultureInfo,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithCultureInfo(
			cultureInfo,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithCultureInfo(TraceFrame traceFrame, CultureInfo? cultureInfo, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithCultureInfo(traceFrame, cultureInfo, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithTargetStoreId(
		string? targetStoreId,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		SetScopeContext(_scopeContext.AppendTraceFrame(
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

		if (string.IsNullOrWhiteSpace(TargetStoreId) || force)
			TargetStoreId = targetStoreId;

		return this;
	}

	public IInvocationContext InvocationAppendTraceFrameWithTargetStoreId(TraceFrame traceFrame, string? targetStoreId, bool force, bool removePreviousSameMethodFrame = true)
	{
		SetScopeContext(_scopeContext.AppendTraceFrame(
			traceFrame,
			removePreviousSameMethodFrame),
			false);

		if (string.IsNullOrWhiteSpace(TargetStoreId) || force)
			TargetStoreId = targetStoreId;

		return this;
	}

	public IInvocationContext InvocationAppendTraceFrameWithBusinessProcess(
		string businessProcess,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithBusinessProcess(
			businessProcess,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithBusinessProcess(TraceFrame traceFrame, string businessProcess, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithBusinessProcess(traceFrame, businessProcess, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithComponent(
		string component,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithComponent(
			component,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithComponent(TraceFrame traceFrame, string component, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithComponent(traceFrame, component, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithTenantIdentifier(
		Guid? tenantIdentifier,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithTenantIdentifier(
			tenantIdentifier,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithTenantIdentifier(TraceFrame traceFrame, Guid? tenantIdentifier, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithTenantIdentifier(traceFrame, tenantIdentifier, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithIdApplicationEntry(
		Guid? idApplicationEntry,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithIdApplicationEntry(
			idApplicationEntry,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithIdApplicationEntry(TraceFrame traceFrame, Guid? idApplicationEntry, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithIdApplicationEntry(traceFrame, idApplicationEntry, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithCustomCorrelationId(
		string? customCorrelationId,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithCustomCorrelationId(
			customCorrelationId,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithCustomCorrelationId(TraceFrame traceFrame, string? customCorrelationId, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithCustomCorrelationId(traceFrame, customCorrelationId, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithLogger(
		ILogger? logger,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithLogger(
			logger,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithLogger(TraceFrame traceFrame, ILogger? logger, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithLogger(traceFrame, logger, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrameWithCancellationToken(
		CancellationToken? cancellationToken,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithCancellationToken(
			cancellationToken,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

	public IInvocationContext InvocationAppendTraceFrameWithCancellationToken(TraceFrame traceFrame, CancellationToken? cancellationToken, bool force, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.AppendTraceFrameWithCancellationToken(traceFrame, cancellationToken, force, removePreviousSameMethodFrame), false);

	public IInvocationContext InvocationAppendTraceFrame(
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
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		SetScopeContext(_scopeContext.AppendTraceFrame(
			principal,
			idUser,
			businessProcess,
			component,
			tenantIdentifier,
			customCorrelationId,
			logger,
			cultureInfo,
			requestMetadata,
			cancellationToken,
			force,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			false);

		if (string.IsNullOrWhiteSpace(TargetStoreId) || force)
			TargetStoreId = targetStoreId;

		return this;
	}

	public IInvocationContext InvocationAppendTraceFrame(
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
		bool removePreviousSameMethodFrame = true)
	{
		SetScopeContext(_scopeContext.AppendTraceFrame(
			traceFrame,
			principal,
			idUser,
			businessProcess,
			component,
			tenantIdentifier,
			customCorrelationId,
			logger,
			cultureInfo,
			requestMetadata,
			cancellationToken,
			force,
			removePreviousSameMethodFrame),
			false);

		if (string.IsNullOrWhiteSpace(TargetStoreId) || force)
			TargetStoreId = targetStoreId;

		return this;
	}




	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithPrincipal instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetPrincipal(LegionPrincipal? principal, bool force)
		=> SetScopeContext(_scopeContext.SetPrincipal(
			principal,
			force),
			false);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithIduser instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetIduser(Guid? idUser, bool force)
		=> SetScopeContext(_scopeContext.SetIduser(
			idUser,
			force),
			false);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithRequestMetadata instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetRequestMetadata(IRequestMetadata? requestMetadata, bool force)
		=> SetScopeContext(_scopeContext.SetRequestMetadata(
			requestMetadata,
			force),
			false);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithCultureInfo instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetCultureInfo(CultureInfo? cultureInfo, bool force)
		=> SetScopeContext(_scopeContext.SetCultureInfo(
			cultureInfo,
			force),
			false);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithBusinessProcess instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetBusinessProcess(string businessProcess, bool force)
		=> SetScopeContext(_scopeContext.SetBusinessProcess(
			businessProcess,
			force),
			false);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithComponent instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetComponent(string component, bool force)
		=> SetScopeContext(_scopeContext.SetComponent(
			component,
			force),
			false);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithTenantIdentifier instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetTenantIdentifier(Guid? tenantIdentifier, bool force)
		=> SetScopeContext(_scopeContext.SetTenantIdentifier(
			tenantIdentifier,
			force),
			false);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithCustomCorrelationId instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetCustomCorrelationId(string? customCorrelationId, bool force)
		=> SetScopeContext(_scopeContext.SetCustomCorrelationId(
			customCorrelationId,
			force),
			false);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithLogger instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetLogger(ILogger? logger, bool force)
		=> SetScopeContext(_scopeContext.SetLogger(
			logger,
			force),
			false);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithCancellationToken instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetCancellationToken(CancellationToken? cancellationToken, bool force)
		=> SetScopeContext(_scopeContext.SetCancellationToken(
			cancellationToken,
			force),
			false);

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrameWithTargetStoreId instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSetTargetStoreId(string? targetStoreId, bool force)
	{
		if (string.IsNullOrWhiteSpace(TargetStoreId) || force)
			TargetStoreId = targetStoreId;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use InvocationAppendTraceFrame instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IInvocationContext InvocationSet(
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
		bool force)
	{
		SetScopeContext(_scopeContext.Set(
			principal,
			idUser,
			businessProcess,
			component,
			tenantIdentifier,
			customCorrelationId,
			logger,
			cultureInfo,
			requestMetadata,
			cancellationToken,
			force),
			false);

		if (string.IsNullOrWhiteSpace(TargetStoreId) || force)
			TargetStoreId = targetStoreId;

		return this;
	}


	public IInvocationContext InvocationCreateNew(
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNew(
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNew(TraceFrame traceFrame, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNew(traceFrame, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithPrincipal(
		LegionPrincipal? principal,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithPrincipal(
			principal,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithPrincipal(TraceFrame traceFrame, LegionPrincipal? principal, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNewWithPrincipal(traceFrame, principal, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithIduser(
		Guid? idUser,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithIduser(
			idUser,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithIduser(TraceFrame traceFrame, Guid? idUser, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNewWithIduser(traceFrame, idUser, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithRequestMetadata(
		IRequestMetadata? requestMetadata,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithRequestMetadata(
			requestMetadata,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithRequestMetadata(TraceFrame traceFrame, IRequestMetadata? requestMetadata, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNewWithRequestMetadata(traceFrame, requestMetadata, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithCultureInfo(
		CultureInfo? cultureInfo,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithCultureInfo(
			cultureInfo,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithCultureInfo(TraceFrame traceFrame, CultureInfo? cultureInfo, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNewWithCultureInfo(traceFrame, cultureInfo, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithTargetStoreId(
		string? targetStoreId,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var ic = SetScopeContext(_scopeContext.CreateNew(
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

		ic.TargetStoreId = targetStoreId;

		return ic;

	}

	public IInvocationContext InvocationCreateNewWithTargetStoreId(TraceFrame traceFrame, string? targetStoreId, bool removePreviousSameMethodFrame = true)
	{
		var ic = SetScopeContext(_scopeContext.CreateNew(traceFrame, removePreviousSameMethodFrame), true);

		ic.TargetStoreId = targetStoreId;

		return ic;

	}

	public IInvocationContext InvocationCreateNewWithBusinessProcess(
		string businessProcess,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithBusinessProcess(
			businessProcess,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithBusinessProcess(TraceFrame traceFrame, string businessProcess, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNewWithBusinessProcess(traceFrame, businessProcess, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithComponent(
		string component,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithComponent(
			component,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithComponent(TraceFrame traceFrame, string component, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNewWithComponent(traceFrame, component, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithTenantIdentifier(
		Guid? tenantIdentifier,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithTenantIdentifier(
			tenantIdentifier,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithTenantIdentifier(TraceFrame traceFrame, Guid? tenantIdentifier, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNewWithTenantIdentifier(traceFrame, tenantIdentifier, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithCustomCorrelationId(
		string? customCorrelationId,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithCustomCorrelationId(
			customCorrelationId,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithCustomCorrelationId(TraceFrame traceFrame, string? customCorrelationId, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNewWithCustomCorrelationId(traceFrame, customCorrelationId, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithLogger(
		ILogger? logger,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithLogger(
			logger,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithLogger(TraceFrame traceFrame, ILogger? logger, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNewWithLogger(traceFrame, logger, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithCancellationToken(
		CancellationToken? cancellationToken,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithCancellationToken(
			cancellationToken,
			removePreviousSameMethodFrame,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithCancellationToken(TraceFrame traceFrame, CancellationToken? cancellationToken, bool removePreviousSameMethodFrame = true)
		=> SetScopeContext(_scopeContext.CreateNewWithCancellationToken(traceFrame, cancellationToken, removePreviousSameMethodFrame), true);

	public IInvocationContext InvocationCreateNewWithContextProperty(
		string contextPropertyKey,
		string? contextPropertyValue,
		bool removePreviousSameMethodFrame = true,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SetScopeContext(_scopeContext.CreateNewWithContextProperty(
			contextPropertyKey,
			contextPropertyValue,
			removePreviousSameMethodFrame,
			comparison,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

	public IInvocationContext InvocationCreateNewWithContextProperty(
		TraceFrame traceFrame,
		string contextPropertyKey,
		string? contextPropertyValue,
		bool removePreviousSameMethodFrame = true,
		bool force = false,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
		=> SetScopeContext(_scopeContext.CreateNewWithContextProperty(traceFrame, contextPropertyKey, contextPropertyValue, removePreviousSameMethodFrame, force, comparison), true);

	public IInvocationContext InvocationCreateNewWith(
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
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var ic = SetScopeContext(_scopeContext.CreateNewWith(
			correlationId,
			principal,
			idUser,
			businessProcess,
			component,
			tenantIdentifier,
			externalCorrelationId,
			customCorrelationId,
			logger,
			cultureInfo,
			requestMetadata,
			cancellationToken,
			removePreviousSameMethodFrame,
			forceNullValues,
			memberName,
			sourceFilePath,
			sourceLineNumber),
			true);

		if (forceNullValues)
		{
			ic.TargetStoreId = targetStoreId;
		}
		else if (targetStoreId != null)
		{
			ic.TargetStoreId = targetStoreId;
		}

		return ic;
	}

	public IInvocationContext InvocationCreateNewWith(
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
		bool forceNullValues = false)
	{
		var ic = SetScopeContext(_scopeContext.CreateNewWith(
			traceFrame,
			correlationId,
			principal,
			idUser,
			businessProcess,
			component,
			tenantIdentifier,
			externalCorrelationId,
			customCorrelationId,
			logger,
			cultureInfo,
			requestMetadata,
			cancellationToken,
			removePreviousSameMethodFrame,
			forceNullValues),
			true);

		if (forceNullValues)
		{
			ic.TargetStoreId = targetStoreId;
		}
		else if (targetStoreId != null)
		{
			ic.TargetStoreId = targetStoreId;
		}

		return ic;
	}

	public IInvocationContext InvocationCreateNewWithResultCallback(
		Func<IInvocationResult, CancellationToken, Task> invocationResultAsyncCallback,
		Action<IInvocationResult> invocationResultSyncCallback)
	{
		return new InvocationContext(_scopeContext, UnhandledErrorCode)
		{
			ServiceProvider = ServiceProvider,
			InvocationResultAsyncCallback = invocationResultAsyncCallback,
			InvocationResultSyncCallback = invocationResultSyncCallback,
			DefaultClientErrorMessage = DefaultClientErrorMessage,
			TargetStoreId = TargetStoreId
		};
	}

	public IInvocationContext InvocationCreateNewWithUnhandledErrorCode(IErrorCode unhandledErrorCode)
	{
		return new InvocationContext(_scopeContext, unhandledErrorCode)
		{
			ServiceProvider = ServiceProvider,
			InvocationResultAsyncCallback = InvocationResultAsyncCallback,
			InvocationResultSyncCallback = InvocationResultSyncCallback,
			DefaultClientErrorMessage = DefaultClientErrorMessage,
			TargetStoreId = TargetStoreId
		};
	}

	public IInvocationContext InvocationCreateNewWithDefaultClientErrorMessage(string defaultClientErrorMessage)
	{
		return new InvocationContext(_scopeContext, UnhandledErrorCode)
		{
			ServiceProvider = ServiceProvider,
			InvocationResultAsyncCallback = InvocationResultAsyncCallback,
			InvocationResultSyncCallback = InvocationResultSyncCallback,
			DefaultClientErrorMessage = defaultClientErrorMessage,
			TargetStoreId = TargetStoreId
		};
	}

	public IDisposable? CreateLoggerScope()
		=> _scopeContext.CreateLoggerScope();


	public IResult<bool> LogMessage(
		ILogMessage logMessage,
		bool skipIfAlreadyLogged = true)
		=> _scopeContext.LogMessage(logMessage, skipIfAlreadyLogged);

	public IResult<bool> LogTraceMessage(
		ILogMessage traceMessage,
		bool skipIfAlreadyLogged = true)
		=> _scopeContext.LogTraceMessage(traceMessage, skipIfAlreadyLogged);

	public IResult<bool> LogDebugMessage(
		ILogMessage debugMessage,
		bool skipIfAlreadyLogged = true)
		=> _scopeContext.LogDebugMessage(debugMessage, skipIfAlreadyLogged);

	public IResult<bool> LogInformationMessage(
		ILogMessage infoMessage,
		bool skipIfAlreadyLogged = true)
		=> _scopeContext.LogInformationMessage(infoMessage, skipIfAlreadyLogged);

	public IResult<bool> LogWarningMessage(
		ILogMessage warningMessage,
		bool skipIfAlreadyLogged = true)
		=> _scopeContext.LogWarningMessage(warningMessage, skipIfAlreadyLogged);

	public IResult<bool> LogErrorMessage(
		IErrorMessage errorMessage,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true)
		=> _scopeContext.LogErrorMessage(errorMessage, string.IsNullOrWhiteSpace(defaultClientErrorMessage) ? DefaultClientErrorMessage : defaultClientErrorMessage, skipIfAlreadyLogged);

	public IResult<bool> LogCriticalMessage(
		IErrorMessage criticalMessage,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true)
		=> _scopeContext.LogCriticalMessage(criticalMessage, string.IsNullOrWhiteSpace(defaultClientErrorMessage) ? DefaultClientErrorMessage : defaultClientErrorMessage, skipIfAlreadyLogged);

	public IResult<bool> LogResultAllMessages(
		IResult result,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true)
		=> _scopeContext.LogResultAllMessages(result, string.IsNullOrWhiteSpace(defaultClientErrorMessage) ? DefaultClientErrorMessage : defaultClientErrorMessage, skipIfAlreadyLogged);

	public IResult<bool> LogResultErrorMessages(
		IResult result,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true)
		=> _scopeContext.LogResultErrorMessages(result, string.IsNullOrWhiteSpace(defaultClientErrorMessage) ? DefaultClientErrorMessage : defaultClientErrorMessage, skipIfAlreadyLogged);

	public string? GetLastTraceFrame()
		=> _scopeContext.GetLastTraceFrame();
}
