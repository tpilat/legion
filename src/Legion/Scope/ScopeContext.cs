using Legion.Exceptions.Internal;
using Legion.Extensions;
using Legion.Identity;
using Legion.Infrastructure;
using Legion.Logging;
using Legion.Serializer;
using Legion.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Legion;

public class ScopeContext : IScopeContext, IApplicationEntryScopeContext
{
	//NECHCEM TO TU DAVAT KVOLI DISPOSE NAD TransactionsControllerom, aby neexistovalo vela objektov, ktore maju referencie na TransactionsController
	//Transactions.ITransactionsController TransactionsController { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private ConcurrentDictionary<string, object?>? _globalItems;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private ConcurrentDictionary<string, object?>? _localInheritableItems;

	public Guid RuntimeUniqueKey { get; private set; }

	public string SourceSystemName { get; private set; }

	public string? BusinessProcess { get; private set; }

	public string? Component { get; private set; }

	public Guid? TenantIdentifier { get; private set; }

	public TraceFrameStack TraceFrameStack { get; private set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public LegionPrincipal? Principal { get; private set; }
	public bool ShouldSerializePrincipal() => false;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public LegionIdentity? User => Principal?.IdentityBase;
	public bool ShouldSerializeUser() => false;

	public Guid? IdUser { get; private set; }

	public string? ExternalCorrelationId { get; private set; }

	public Guid? CorrelationId { get; private set; }

	public Guid? IdApplicationEntry { get; private set; }

	public string? CustomCorrelationId { get; private set; }

	public Guid TraceCorrelationId { get; private set; }

	public Dictionary<string, string?> _contextProperties;
	public IReadOnlyDictionary<string, string?> ContextProperties => _contextProperties;

	public IRequestMetadata? RequestMetadata { get; private set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public CultureInfo? CurrentCulture { get; private set; }
	public bool ShouldSerializeCurrentCulture() => false;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public ILogger? Logger { get; private set; }
	public bool ShouldSerializeLogger() => false;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public CancellationToken? CancellationToken { get; private set; }
	public bool ShouldSerializeCancellationToken() => false;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	internal protected ScopeContext()
	{
		_contextProperties = [];
	}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	protected ScopeContext(string sourceSystemName, TraceFrame currentTraceFrame, IScopeContext? previousScopeContext, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(sourceSystemName ?? previousScopeContext?.SourceSystemName);
		Throw.IfArgumentNullOrWhiteSpace(currentTraceFrame);

		var traceFrameStack = new TraceFrameStack(previousScopeContext?.TraceFrameStack, currentTraceFrame, removePreviousSameMethodFrame);

		Throw.IfNull(traceFrameStack);

		TraceCorrelationId = GlobalContext.Instance.NewGuid();
		SourceSystemName = sourceSystemName ?? previousScopeContext?.SourceSystemName!;
		TraceFrameStack = traceFrameStack;
		RuntimeUniqueKey = EnvironmentInfo.RUNTIME_UNIQUE_KEY;
		_contextProperties = [];

		if (previousScopeContext != null)
		{
			RuntimeUniqueKey = previousScopeContext.RuntimeUniqueKey;
			_contextProperties = previousScopeContext.ContextProperties.ToDictionary(x => x.Key, x => x.Value);

			BusinessProcess = previousScopeContext.BusinessProcess;
			Component = previousScopeContext.Component;
			TenantIdentifier = previousScopeContext.TenantIdentifier;
			IdApplicationEntry = previousScopeContext.IdApplicationEntry;
			IdUser = previousScopeContext.IdUser;
			Principal = previousScopeContext.Principal;
			ExternalCorrelationId = previousScopeContext.ExternalCorrelationId;
			CorrelationId = previousScopeContext.CorrelationId;
			CustomCorrelationId = previousScopeContext.CustomCorrelationId;
			RequestMetadata = previousScopeContext.RequestMetadata;
			CurrentCulture = previousScopeContext.CurrentCulture;
			Logger = previousScopeContext.Logger;
			CancellationToken = previousScopeContext.CancellationToken;

			if (previousScopeContext is ScopeContext scopeContext)
			{
				_globalItems = scopeContext._globalItems;

				if (0 < scopeContext._localInheritableItems?.Count)
				{
					_localInheritableItems = [];
					foreach (var previousInheritableItemKvp in scopeContext._localInheritableItems)
						_localInheritableItems.AddOrUpdate(previousInheritableItemKvp.Key, previousInheritableItemKvp.Value, (k, v) => previousInheritableItemKvp);
				}
			}
		}
	}

	private ScopeContext(IScopeContext previousScopeContext)
	{
		Throw.IfArgumentNull(previousScopeContext);

		var traceFrameStack = new TraceFrameStack(previousScopeContext.TraceFrameStack, previousScopeContext.TraceFrameStack.LastTraceFrame, true);

		TraceCorrelationId = GlobalContext.Instance.NewGuid();
		SourceSystemName = previousScopeContext.SourceSystemName!;
		TraceFrameStack = traceFrameStack;
		RuntimeUniqueKey = EnvironmentInfo.RUNTIME_UNIQUE_KEY;
		_contextProperties = [];

		RuntimeUniqueKey = previousScopeContext.RuntimeUniqueKey;
		_contextProperties = previousScopeContext.ContextProperties.ToDictionary(x => x.Key, x => x.Value);

		BusinessProcess = previousScopeContext.BusinessProcess;
		Component = previousScopeContext.Component;
		TenantIdentifier = previousScopeContext.TenantIdentifier;
		IdApplicationEntry = previousScopeContext.IdApplicationEntry;
		IdUser = previousScopeContext.IdUser;
		Principal = previousScopeContext.Principal;
		ExternalCorrelationId = previousScopeContext.ExternalCorrelationId;
		CorrelationId = previousScopeContext.CorrelationId;
		CustomCorrelationId = previousScopeContext.CustomCorrelationId;
		RequestMetadata = previousScopeContext.RequestMetadata;
		CurrentCulture = previousScopeContext.CurrentCulture;
		Logger = previousScopeContext.Logger;
		CancellationToken = previousScopeContext.CancellationToken;

		if (previousScopeContext is ScopeContext scopeContext)
		{
			_globalItems = scopeContext._globalItems;

			if (0 < scopeContext._localInheritableItems?.Count)
			{
				_localInheritableItems = [];
				foreach (var previousInheritableItemKvp in scopeContext._localInheritableItems)
					_localInheritableItems.AddOrUpdate(previousInheritableItemKvp.Key, previousInheritableItemKvp.Value, (k, v) => previousInheritableItemKvp);
			}
		}
	}

	public bool TryGetGlobalItem(string name, out object? value)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		value = null;
		if (_globalItems == null)
			return false;

		return _globalItems.TryGetValue(name, out value);
	}

	public bool TryGetGlobalItem<T>(string name, out T? value)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		value = default;
		if (_globalItems == null)
			return false;

		var exists = _globalItems.TryGetValue(name, out var val);
		if (exists)
			value = (T?)val;

		return exists;
	}

	public object? GetOrAddGlobalItem(string name, object? data)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		if (_globalItems == null)
			Interlocked.CompareExchange(ref _globalItems, [], null);

		return _globalItems.GetOrAdd(name, data);
	}

	public void AddOrUpdateGlobalItem(string name, object? data)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		if (_globalItems == null)
			Interlocked.CompareExchange(ref _globalItems, [], null);

		_globalItems.AddOrUpdate(name, data, (k, v) => data);
	}

	public bool TryGetLocalInheritableItem(string name, out object? value)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		value = null;
		if (_localInheritableItems == null)
			return false;

		return _localInheritableItems.TryGetValue(name, out value);
	}

	public bool TryGetLocalInheritableItem<T>(string name, out T? value)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		value = default;
		if (_localInheritableItems == null)
			return false;

		var exists = _localInheritableItems.TryGetValue(name, out var val);
		if (exists)
			value = (T?)val;

		return exists;
	}

	public object? GetOrAddLocalInheritableItem(string name, object? data)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		if (_localInheritableItems == null)
			Interlocked.CompareExchange(ref _localInheritableItems, [], null);

		return _localInheritableItems.GetOrAdd(name, data);
	}

	public void AddOrUpdateLocalInheritableItem(string name, object? data)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		if (_localInheritableItems == null)
			Interlocked.CompareExchange(ref _localInheritableItems, [], null);

		_localInheritableItems.AddOrUpdate(name, data, (k, v) => data);
	}

	public IScopeContext AddContextProperty(
		string key,
		string? value,
		bool force = false,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);

		if (force || !ContextProperties.ContainsKey(key, comparison))
			_contextProperties[key] = value;

		return this;
	}

	public IScopeContext RemoveContextProperty(
		string key,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);

		_contextProperties.Remove(key, comparison);
		return this;
	}

	public string ContextPropertiesToJson()
		=> JsonSerializerHelper.Serialize(ContextProperties);

	public override string? ToString()
		=> $"{SourceSystemName} {TraceFrameStack}{Environment.NewLine} {nameof(RuntimeUniqueKey)} = {RuntimeUniqueKey} | {nameof(CorrelationId)} = {CorrelationId} | {nameof(IdUser)} = {IdUser}";

	public string ToStringTrace()
	{
		var sb = new StringBuilder();
		sb.Append(nameof(RuntimeUniqueKey)).Append(": ").Append(RuntimeUniqueKey).AppendLine();
		sb.Append(nameof(SourceSystemName)).Append(": ").Append(SourceSystemName).AppendLine();
		sb.Append(nameof(BusinessProcess)).Append(": ").Append(BusinessProcess).AppendLine();
		sb.Append(nameof(Component)).Append(": ").Append(Component).AppendLine();
		sb.Append(nameof(TenantIdentifier)).Append(": ").Append(TenantIdentifier).AppendLine();

		if (TraceFrameStack != null)
		{
			sb.Append(nameof(TraceFrameStack)).Append(": ");
			sb.Append(TraceFrameStack.ToStringTrace($"{Environment.NewLine}\t"))
				.AppendLine();
		}

		sb.Append(nameof(IdUser)).Append(": ").Append(IdUser).AppendLine();
		sb.Append(nameof(CorrelationId)).Append(": ").Append(CorrelationId).AppendLine();
		sb.Append(nameof(ExternalCorrelationId)).Append(": ").Append(ExternalCorrelationId);
		sb.Append(nameof(CustomCorrelationId)).Append(": ").Append(CustomCorrelationId);
		sb.Append(nameof(TraceCorrelationId)).Append(": ").Append(TraceCorrelationId).AppendLine();

		if (0 < _contextProperties?.Count)
		{
			sb.AppendLine();
			sb.Append(nameof(_contextProperties)).Append(": ");
			foreach (var kvp in _contextProperties)
				sb.AppendLine().Append('\t').Append(kvp.Key).Append(": ").Append(kvp.Value);
		}

		if (RequestMetadata?.Uri != null)
		{
			sb.AppendLine();
			sb.Append(nameof(RequestMetadata)).Append(": ").Append(RequestMetadata.Uri);
		}

		if (CurrentCulture != null)
		{
			sb.AppendLine();
			sb.Append(nameof(CurrentCulture)).Append(": ").Append(CurrentCulture.TwoLetterISOLanguageName);
		}

		return sb.ToString();
	}

	public IScopeContext AppendTraceFrame(
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrame(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrame(TraceFrame traceFrame, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithPrincipal(
		LegionPrincipal? principal,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithPrincipal(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), principal, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithPrincipal(TraceFrame traceFrame, LegionPrincipal? principal, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (Principal == null || force)
			{
				Principal = principal;

				if (Principal?.IdentityBase?.IdUser != null)
					IdUser = Principal.IdentityBase.IdUser;
			}
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithIduser(
		Guid? idUser,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithIduser(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), idUser, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithIduser(TraceFrame traceFrame, Guid? idUser, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (!IdUser.HasValue || force)
				IdUser = idUser;
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithRequestMetadata(
		IRequestMetadata? requestMetadata,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithRequestMetadata(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), requestMetadata, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithRequestMetadata(TraceFrame traceFrame, IRequestMetadata? requestMetadata, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (RequestMetadata == null || force)
				RequestMetadata = requestMetadata;
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithCultureInfo(
		CultureInfo? cultureInfo,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithCultureInfo(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), cultureInfo, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithCultureInfo(TraceFrame traceFrame, CultureInfo? cultureInfo, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (CurrentCulture == null || force)
				CurrentCulture = cultureInfo;
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithBusinessProcess(
		string businessProcess,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithBusinessProcess(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), businessProcess, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithBusinessProcess(TraceFrame traceFrame, string businessProcess, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (string.IsNullOrWhiteSpace(BusinessProcess) || force)
				BusinessProcess = businessProcess;
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithComponent(
		string component,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithComponent(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), component, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithComponent(TraceFrame traceFrame, string component, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (string.IsNullOrWhiteSpace(Component) || force)
				Component = component;
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithTenantIdentifier(
		Guid? tenantIdentifier,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithTenantIdentifier(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), tenantIdentifier, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithTenantIdentifier(TraceFrame traceFrame, Guid? tenantIdentifier, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (!TenantIdentifier.HasValue || force)
				TenantIdentifier = tenantIdentifier;
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithIdApplicationEntry(
		Guid? idApplicationEntry,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithIdApplicationEntry(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), idApplicationEntry, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithIdApplicationEntry(TraceFrame traceFrame, Guid? idApplicationEntry, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (!IdApplicationEntry.HasValue || force)
				IdApplicationEntry = idApplicationEntry;
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithCustomCorrelationId(
		string? customCorrelationId,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithCustomCorrelationId(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), customCorrelationId, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithCustomCorrelationId(TraceFrame traceFrame, string? customCorrelationId, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (string.IsNullOrWhiteSpace(CustomCorrelationId) || force)
				CustomCorrelationId = customCorrelationId;
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithLogger(
		ILogger? logger,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithLogger(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), logger, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithLogger(TraceFrame traceFrame, ILogger? logger, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (Logger == null || force)
				Logger = logger;
		}

		return this;
	}

	public IScopeContext AppendTraceFrameWithCancellationToken(
		CancellationToken? cancellationToken,
		bool force,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> AppendTraceFrameWithCancellationToken(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), cancellationToken, force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrameWithCancellationToken(TraceFrame traceFrame, CancellationToken? cancellationToken, bool force, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (CancellationToken == null || force)
				CancellationToken = cancellationToken;
		}

		return this;
	}

	public IScopeContext AppendTraceFrame(
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
		=> AppendTraceFrame(
			new TraceFrame(memberName, sourceFilePath, sourceLineNumber),
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
			force, removePreviousSameMethodFrame);

	public IScopeContext AppendTraceFrame(
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
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		lock (TraceFrameStack)
		{
			TraceFrameStack = TraceFrameStack.CreateNext(traceFrame, removePreviousSameMethodFrame);

			if (Principal == null || force)
				Principal = principal;

			if (!IdUser.HasValue || force)
				IdUser = idUser;

			if (string.IsNullOrWhiteSpace(BusinessProcess) || force)
				BusinessProcess = businessProcess;

			if (string.IsNullOrWhiteSpace(Component) || force)
				Component = component;

			if (!TenantIdentifier.HasValue || force)
				TenantIdentifier = tenantIdentifier;

			if (string.IsNullOrWhiteSpace(CustomCorrelationId) || force)
				CustomCorrelationId = customCorrelationId;

			if (Logger == null || force)
				Logger = logger;

			if (CurrentCulture == null || force)
				CurrentCulture = cultureInfo;

			if (RequestMetadata == null || force)
				RequestMetadata = requestMetadata;

			if (CancellationToken == null || force)
				CancellationToken = cancellationToken;
		}

		return this;
	}







	[System.Diagnostics.StackTraceHidden]
	internal IScopeContext SetExternalCorrelationId(string? externalCorrelationId, bool force)
	{
		if (ExternalCorrelationId == null || force)
			ExternalCorrelationId = externalCorrelationId;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	internal IScopeContext SetIdApplicationEntry(Guid? idApplicationEntry, bool force)
	{
		if (!IdApplicationEntry.HasValue || force)
			IdApplicationEntry = idApplicationEntry;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithPrincipal instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext SetPrincipal(LegionPrincipal? principal, bool force)
	{
		if (Principal == null || force)
			Principal = principal;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithIduser instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext SetIduser(Guid? idUser, bool force)
	{
		if (!IdUser.HasValue || force)
			IdUser = idUser;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithRequestMetadata instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext SetRequestMetadata(IRequestMetadata? requestMetadata, bool force)
	{
		if (RequestMetadata == null || force)
			RequestMetadata = requestMetadata;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithCultureInfo instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext SetCultureInfo(CultureInfo? cultureInfo, bool force)
	{
		if (CurrentCulture == null || force)
			CurrentCulture = cultureInfo;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithBusinessProcess instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext SetBusinessProcess(string businessProcess, bool force)
	{
		if (string.IsNullOrWhiteSpace(BusinessProcess) || force)
			BusinessProcess = businessProcess;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithComponent instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext SetComponent(string component, bool force)
	{
		if (string.IsNullOrWhiteSpace(Component) || force)
			Component = component;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithTenantIdentifier instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext SetTenantIdentifier(Guid? tenantIdentifier, bool force)
	{
		if (!TenantIdentifier.HasValue || force)
			TenantIdentifier = tenantIdentifier;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithCustomCorrelationId instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext SetCustomCorrelationId(string? customCorrelationId, bool force)
	{
		if (string.IsNullOrWhiteSpace(CustomCorrelationId) || force)
			CustomCorrelationId = customCorrelationId;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithLogger instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext SetLogger(ILogger? logger, bool force)
	{
		if (Logger == null || force)
			Logger = logger;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrameWithCancellationToken instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext SetCancellationToken(CancellationToken? cancellationToken, bool force)
	{
		if (CancellationToken == null || force)
			CancellationToken = cancellationToken;

		return this;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use AppendTraceFrame instead."
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_SCOPE_SET")]
#else
	)]
#endif
	public IScopeContext Set(
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
	{
		if (Principal == null || force)
			Principal = principal;

		if (!IdUser.HasValue || force)
			IdUser = idUser;

		if (string.IsNullOrWhiteSpace(BusinessProcess) || force)
			BusinessProcess = businessProcess;

		if (string.IsNullOrWhiteSpace(Component) || force)
			Component = component;

		if (!TenantIdentifier.HasValue || force)
			TenantIdentifier = tenantIdentifier;

		if (string.IsNullOrWhiteSpace(CustomCorrelationId) || force)
			CustomCorrelationId = customCorrelationId;

		if (Logger == null || force)
			Logger = logger;

		if (CurrentCulture == null || force)
			CurrentCulture = cultureInfo;

		if (RequestMetadata == null || force)
			RequestMetadata = requestMetadata;

		if (CancellationToken == null || force)
			CancellationToken = cancellationToken;

		return this;
	}








	public IScopeContext CreateNew(
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNew(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), removePreviousSameMethodFrame);

	public IScopeContext CreateNew(TraceFrame traceFrame, bool removePreviousSameMethodFrame = true)
		=> new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithPrincipal(
		LegionPrincipal? principal,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithPrincipal(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), principal, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithPrincipal(TraceFrame traceFrame, LegionPrincipal? principal, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame)
		{
			Principal = principal
		};

		return scopeContext;
	}

	public IScopeContext CreateNewWithIduser(
		Guid? idUser,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithIduser(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), idUser, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithIduser(TraceFrame traceFrame, Guid? idUser, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame)
		{
			IdUser = idUser
		};

		return scopeContext;
	}

	public IScopeContext CreateNewWithRequestMetadata(
		IRequestMetadata? requestMetadata,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithRequestMetadata(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), requestMetadata, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithRequestMetadata(TraceFrame traceFrame, IRequestMetadata? requestMetadata, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame)
		{
			RequestMetadata = requestMetadata
		};

		return scopeContext;
	}

	public IScopeContext CreateNewWithCultureInfo(
		CultureInfo? cultureInfo,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithCultureInfo(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), cultureInfo, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithCultureInfo(TraceFrame traceFrame, CultureInfo? cultureInfo, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame)
		{
			CurrentCulture = cultureInfo
		};

		return scopeContext;
	}

	public IScopeContext CreateNewWithBusinessProcess(
		string businessProcess,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithBusinessProcess(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), businessProcess, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithBusinessProcess(TraceFrame traceFrame, string businessProcess, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame)
		{
			BusinessProcess = businessProcess
		};

		return scopeContext;
	}

	public IScopeContext CreateNewWithComponent(
		string component,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithComponent(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), component, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithComponent(TraceFrame traceFrame, string component, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame)
		{
			Component = component
		};

		return scopeContext;
	}

	public IScopeContext CreateNewWithTenantIdentifier(
		Guid? tenantIdentifier,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithTenantIdentifier(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), tenantIdentifier, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithTenantIdentifier(TraceFrame traceFrame, Guid? tenantIdentifier, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame)
		{
			TenantIdentifier = tenantIdentifier
		};

		return scopeContext;
	}

	public IScopeContext CreateNewWithCustomCorrelationId(
		string? customCorrelationId,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithCustomCorrelationId(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), customCorrelationId, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithCustomCorrelationId(TraceFrame traceFrame, string? customCorrelationId, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame)
		{
			CustomCorrelationId = customCorrelationId
		};

		return scopeContext;
	}

	public IScopeContext CreateNewWithLogger(
		ILogger? logger,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithLogger(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), logger, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithLogger(TraceFrame traceFrame, ILogger? logger, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame)
		{
			Logger = logger
		};

		return scopeContext;
	}

	public IScopeContext CreateNewWithCancellationToken(
		CancellationToken? cancellationToken,
		bool removePreviousSameMethodFrame = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithCancellationToken(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), cancellationToken, removePreviousSameMethodFrame);

	public IScopeContext CreateNewWithCancellationToken(TraceFrame traceFrame, CancellationToken? cancellationToken, bool removePreviousSameMethodFrame = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame)
		{
			CancellationToken = cancellationToken
		};

		return scopeContext;
	}

	public IScopeContext CreateNewWithContextProperty(
		string contextPropertyKey,
		string? contextPropertyValue,
		bool removePreviousSameMethodFrame = true,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWithContextProperty(new TraceFrame(memberName, sourceFilePath, sourceLineNumber), contextPropertyKey, contextPropertyValue, removePreviousSameMethodFrame, false, comparison);

	public IScopeContext CreateNewWithContextProperty(
		TraceFrame traceFrame,
		string contextPropertyKey,
		string? contextPropertyValue,
		bool removePreviousSameMethodFrame = true,
		bool force = false,
		StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);
		Throw.IfArgumentNullOrWhiteSpace(contextPropertyKey);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame);
		scopeContext.AddContextProperty(contextPropertyKey, contextPropertyValue, force, comparison);

		return scopeContext;
	}

	public IScopeContext CreateNewWith(
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
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateNewWith(
			new TraceFrame(memberName, sourceFilePath, sourceLineNumber),
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

	public IScopeContext CreateNewWith(
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
		bool forceNullValues = false)
	{
		Throw.IfArgumentNullOrWhiteSpace(traceFrame);

		var scopeContext = new ScopeContext(SourceSystemName, traceFrame, this, removePreviousSameMethodFrame);

		if (forceNullValues)
		{
			scopeContext.CorrelationId = correlationId;
			scopeContext.ExternalCorrelationId = externalCorrelationId;
			scopeContext.Principal = principal;
			scopeContext.IdUser = idUser;
			scopeContext.BusinessProcess = businessProcess;
			scopeContext.Component = component;
			scopeContext.TenantIdentifier = tenantIdentifier;
			scopeContext.CustomCorrelationId = customCorrelationId;
			scopeContext.Logger = logger;
			scopeContext.CancellationToken = cancellationToken;
			scopeContext.CurrentCulture = cultureInfo;
			scopeContext.RequestMetadata = requestMetadata;
		}
		else
		{
			if (correlationId.HasValue)
				scopeContext.CorrelationId = correlationId;

			if (externalCorrelationId != null)
				scopeContext.ExternalCorrelationId = externalCorrelationId;

			if (principal != null)
				scopeContext.Principal = principal;

			if (idUser.HasValue)
				scopeContext.IdUser = idUser;

			if (businessProcess != null)
				scopeContext.BusinessProcess = businessProcess;

			if (component != null)
				scopeContext.Component = component;

			if (tenantIdentifier.HasValue)
				scopeContext.TenantIdentifier = tenantIdentifier;

			if (customCorrelationId != null)
				scopeContext.CustomCorrelationId = customCorrelationId;

			if (logger != null)
				scopeContext.Logger = logger;

			if (cancellationToken.HasValue)
				scopeContext.CancellationToken = cancellationToken;

			if (cultureInfo != null)
				scopeContext.CurrentCulture = cultureInfo;

			if (requestMetadata != null)
				scopeContext.RequestMetadata = requestMetadata;
		}

		return scopeContext;
	}

	public static IScopeContext Create(
		string? sourceSystemName,
		bool removePreviousSameMethodFrame = true,
		IScopeContext? previousScopeContext = null,
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
		bool forceNullValues = false,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var scopeContext = new ScopeContext(
				sourceSystemName ?? previousScopeContext?.SourceSystemName!,
				new TraceFrame(memberName, sourceFilePath, sourceLineNumber),
				previousScopeContext,
				removePreviousSameMethodFrame);

		if (previousScopeContext == null || forceNullValues)
		{
			scopeContext.CorrelationId = correlationId;
			scopeContext.ExternalCorrelationId = externalCorrelationId;
			scopeContext.Principal = principal;
			scopeContext.IdUser = idUser;
			scopeContext.BusinessProcess = businessProcess;
			scopeContext.Component = component;
			scopeContext.TenantIdentifier = tenantIdentifier;
			scopeContext.CustomCorrelationId = customCorrelationId;
			scopeContext.Logger = logger;
			scopeContext.CancellationToken = cancellationToken;
			scopeContext.CurrentCulture = cultureInfo;
			scopeContext.RequestMetadata = requestMetadata;
		}
		else
		{
			if (correlationId.HasValue)
				scopeContext.CorrelationId = correlationId;

			if (externalCorrelationId != null)
				scopeContext.ExternalCorrelationId = externalCorrelationId;

			if (principal != null)
				scopeContext.Principal = principal;

			if (idUser.HasValue)
				scopeContext.IdUser = idUser;

			if (businessProcess != null)
				scopeContext.BusinessProcess = businessProcess;

			if (component != null)
				scopeContext.Component = component;

			if (tenantIdentifier.HasValue)
				scopeContext.TenantIdentifier = tenantIdentifier;

			if (customCorrelationId != null)
				scopeContext.CustomCorrelationId = customCorrelationId;

			if (logger != null)
				scopeContext.Logger = logger;

			if (cancellationToken.HasValue)
				scopeContext.CancellationToken = cancellationToken;

			if (cultureInfo != null)
				scopeContext.CurrentCulture = cultureInfo;

			if (requestMetadata != null)
				scopeContext.RequestMetadata = requestMetadata;
		}

		return scopeContext;
	}

	public static IScopeContext Create(
		IServiceProvider serviceProvider,
		bool removePreviousSameMethodFrame = true,
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
		bool forceNullValues = false,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		Throw.IfArgumentNull(serviceProvider);
		var scopedScopeContext = serviceProvider.GetRequiredService<IScopeContext>();

		return Create(
			null,
			removePreviousSameMethodFrame,
			scopedScopeContext,
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
			forceNullValues,
			memberName,
			sourceFilePath,
			sourceLineNumber);
	}

	public static IScopeContext Create(
		IScopeContext previousScopeContext,
		bool removePreviousSameMethodFrame = true,
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
		bool forceNullValues = false,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		Throw.IfArgumentNull(previousScopeContext);

		var scopeContext = new ScopeContext(
			previousScopeContext.SourceSystemName,
			new TraceFrame(memberName, sourceFilePath, sourceLineNumber),
			previousScopeContext,
			removePreviousSameMethodFrame);

		if (forceNullValues)
		{
			scopeContext.CorrelationId = correlationId;
			scopeContext.ExternalCorrelationId = externalCorrelationId;
			scopeContext.Principal = principal;
			scopeContext.IdUser = idUser;
			scopeContext.BusinessProcess = businessProcess;
			scopeContext.Component = component;
			scopeContext.TenantIdentifier = tenantIdentifier;
			scopeContext.CustomCorrelationId = customCorrelationId;
			scopeContext.Logger = logger;
			scopeContext.CancellationToken = cancellationToken;
			scopeContext.CurrentCulture = cultureInfo;
			scopeContext.RequestMetadata = requestMetadata;
		}
		else
		{
			if (correlationId.HasValue)
				scopeContext.CorrelationId = correlationId;

			if (externalCorrelationId != null)
				scopeContext.ExternalCorrelationId = externalCorrelationId;

			if (principal != null)
				scopeContext.Principal = principal;

			if (idUser.HasValue)
				scopeContext.IdUser = idUser;

			if (businessProcess != null)
				scopeContext.BusinessProcess = businessProcess;

			if (component != null)
				scopeContext.Component = component;

			if (tenantIdentifier.HasValue)
				scopeContext.TenantIdentifier = tenantIdentifier;

			if (customCorrelationId != null)
				scopeContext.CustomCorrelationId = customCorrelationId;

			if (logger != null)
				scopeContext.Logger = logger;

			if (cancellationToken.HasValue)
				scopeContext.CancellationToken = cancellationToken;

			if (cultureInfo != null)
				scopeContext.CurrentCulture = cultureInfo;

			if (requestMetadata != null)
				scopeContext.RequestMetadata = requestMetadata;
		}

		return scopeContext;
	}

	public IDisposable? CreateLoggerScope()
	{
		if (Logger == null)
			return null;

		return LoggerExtensions.BeginScope(Logger, this);
	}

	public IResult<bool> LogMessage(
		ILogMessage logMessage,
		bool skipIfAlreadyLogged = true)
	{
		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(this, logMessage))
			return result.WithData(false).Build();

		if (Logger == null)
			return result.WithData(false).Build();

		try
		{
			Logger.LogMessage(logMessage, skipIfAlreadyLogged);
			return result.WithData(true).Build();
		}
		catch (Exception ex)
		{
			return
				result
					.WithData(false)
					.WithError(
						this,
						ErrorCodes.Logger.LoggerException(logMessage.IdLogLevel),
						x => x.ExceptionInfo(ex));
		}
	}

	public IResult<bool> LogTraceMessage(
		ILogMessage traceMessage,
		bool skipIfAlreadyLogged = true)
	{
		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(this, traceMessage))
			return result.WithData(false).Build();

		if (Logger == null)
			return result.WithData(false).Build();

		try
		{
			Logger.LogTraceMessage(traceMessage, skipIfAlreadyLogged);
			return result.WithData(true).Build();
		}
		catch (Exception ex)
		{
			return
				result
					.WithData(false)
					.WithError(
						this,
						ErrorCodes.Logger.LoggerException((int)LogLevel.Trace),
						x => x.ExceptionInfo(ex));
		}
	}

	public IResult<bool> LogDebugMessage(
		ILogMessage debugMessage,
		bool skipIfAlreadyLogged = true)
	{
		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(this, debugMessage))
			return result.WithData(false).Build();

		if (Logger == null)
			return result.WithData(false).Build();

		try
		{
			Logger.LogDebugMessage(debugMessage, skipIfAlreadyLogged);
			return result.WithData(true).Build();
		}
		catch (Exception ex)
		{
			return
				result
					.WithData(false)
					.WithError(
						this,
						ErrorCodes.Logger.LoggerException((int)LogLevel.Debug),
						x => x.ExceptionInfo(ex));
		}
	}

	public IResult<bool> LogInformationMessage(
		ILogMessage infoMessage,
		bool skipIfAlreadyLogged = true)
	{
		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(this, infoMessage))
			return result.WithData(false).Build();

		if (Logger == null)
			return result.WithData(false).Build();

		try
		{
			Logger.LogInformationMessage(infoMessage, skipIfAlreadyLogged);
			return result.WithData(true).Build();
		}
		catch (Exception ex)
		{
			return
				result
					.WithData(false)
					.WithError(
						this,
						ErrorCodes.Logger.LoggerException((int)LogLevel.Information),
						x => x.ExceptionInfo(ex));
		}
	}

	public IResult<bool> LogWarningMessage(
		ILogMessage warningMessage,
		bool skipIfAlreadyLogged = true)
	{
		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(this, warningMessage))
			return result.WithData(false).Build();

		if (Logger == null)
			return result.WithData(false).Build();

		try
		{
			Logger.LogWarningMessage(warningMessage, skipIfAlreadyLogged);
			return result.WithData(true).Build();
		}
		catch (Exception ex)
		{
			return
				result
					.WithData(false)
					.WithError(
						this,
						ErrorCodes.Logger.LoggerException((int)LogLevel.Warning),
						x => x.ExceptionInfo(ex));
		}
	}

	public IResult<bool> LogErrorMessage(
		IErrorMessage errorMessage,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true)
	{
		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(this, errorMessage))
			return result.WithData(false).Build();

		if (Logger == null)
			return result.WithData(false).Build();

		try
		{
			if (!string.IsNullOrWhiteSpace(defaultClientErrorMessage) && string.IsNullOrWhiteSpace(errorMessage.ClientMessage))
				errorMessage.ClientMessage = defaultClientErrorMessage;

			Logger.LogErrorMessage(errorMessage, skipIfAlreadyLogged);
			return result.WithData(true).Build();
		}
		catch (Exception ex)
		{
			return
				result
					.WithData(false)
					.WithError(
						this,
						ErrorCodes.Logger.LoggerException((int)LogLevel.Error),
						x => x.ExceptionInfo(ex));
		}
	}

	public IResult<bool> LogCriticalMessage(
		IErrorMessage criticalMessage,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true)
	{
		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(this, criticalMessage))
			return result.WithData(false).Build();

		if (Logger == null)
			return result.WithData(false).Build();

		try
		{
			if (!string.IsNullOrWhiteSpace(defaultClientErrorMessage) && string.IsNullOrWhiteSpace(criticalMessage.ClientMessage))
				criticalMessage.ClientMessage = defaultClientErrorMessage;

			Logger.LogCriticalMessage(criticalMessage, skipIfAlreadyLogged);
			return result.WithData(true).Build();
		}
		catch (Exception ex)
		{
			return
				result
					.WithData(false)
					.WithError(
						this,
						ErrorCodes.Logger.LoggerException((int)LogLevel.Critical),
						x => x.ExceptionInfo(ex));
		}
	}

	public IResult<bool> LogResultAllMessages(
		IResult result,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true)
	{
		var internalResult = new ResultBuilder<bool>();

		if (internalResult.IsArgumentNull(this, result))
			return internalResult.WithData(false).Build();

		if (Logger == null)
			return internalResult.WithData(false).Build();

		var messages = new List<ILogMessage>(result.ErrorMessages);
		messages.AddRange(result.WarningMessages);
		messages.AddRange(result.SuccessMessages);

		messages = messages.OrderBy(x => x.CreatedUtc).ToList();
		foreach (var message in messages)
		{
			if (!string.IsNullOrWhiteSpace(defaultClientErrorMessage)
				&& message is ErrorMessage errorMessage
				&& string.IsNullOrWhiteSpace(errorMessage.ClientMessage))
				errorMessage.ClientMessage = defaultClientErrorMessage;

			try
			{
				switch (message.LogLevel)
				{
					case LogLevel.Trace:
						Logger.LogTraceMessage(message, skipIfAlreadyLogged);
						break;
					case LogLevel.Debug:
						Logger.LogDebugMessage(message, skipIfAlreadyLogged);
						break;
					case LogLevel.Information:
						Logger.LogInformationMessage(message, skipIfAlreadyLogged);
						break;
					case LogLevel.Warning:
						Logger.LogWarningMessage(message, skipIfAlreadyLogged);
						break;
					case LogLevel.Error:
						Logger.LogErrorMessage((message as IErrorMessage)!, skipIfAlreadyLogged);
						break;
					case LogLevel.Critical:
						Logger.LogCriticalMessage((message as IErrorMessage)!, skipIfAlreadyLogged);
						break;
				}
			}
			catch (Exception ex)
			{
				return
					internalResult
						.WithData(false)
						.WithError(
							this,
							ErrorCodes.Logger.LoggerException((int)message.LogLevel),
							x => x.ExceptionInfo(ex));
			}
		}

		return
			internalResult
				.WithData(!internalResult.HasError())
				.Build();
	}

	public IResult<bool> LogResultErrorMessages(
		IResult result,
		string? defaultClientErrorMessage = null,
		bool skipIfAlreadyLogged = true)
	{
		var internalResult = new ResultBuilder<bool>();

		if (internalResult.IsArgumentNull(this, result))
			return internalResult.WithData(false).Build();

		if (Logger == null)
			return internalResult.WithData(false).Build();

		foreach (var message in result.ErrorMessages.OrderBy(x => x.CreatedUtc))
		{
			if (!string.IsNullOrWhiteSpace(defaultClientErrorMessage)
				&& message is ErrorMessage errorMessage
				&& string.IsNullOrWhiteSpace(errorMessage.ClientMessage))
				errorMessage.ClientMessage = defaultClientErrorMessage;

			try
			{
				switch (message.LogLevel)
				{
					case LogLevel.Error:
						Logger.LogErrorMessage(message, skipIfAlreadyLogged);
						break;
					case LogLevel.Critical:
						Logger.LogCriticalMessage(message, skipIfAlreadyLogged);
						break;
				}
			}
			catch (Exception ex)
			{
				return
					internalResult
						.WithData(false)
						.WithError(
							this,
							ErrorCodes.Logger.LoggerException((int)message.LogLevel),
							x => x.ExceptionInfo(ex));
			}
		}

		return
			internalResult
				.WithData(!internalResult.HasError())
				.Build();
	}

	public string? GetLastTraceFrame()
		=> TraceFrameStack.LastFrame;

	public IApplicationEntryScopeContext Clone()
	{
		var clone = new ScopeContext(this);
		return clone;
	}
}
