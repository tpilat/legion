using Legion.Model;
using Legion.Model.Messaging;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Messaging.DomainEvents.Services;

public partial class DomainEventStore : IDomainEventStore, IDisposable, IAsyncDisposable
{
	private static readonly object _blockedDomainEventTypesLock = new();
	private static IReadOnlyList<string>? _blockedDomainEventNamespaces;
	private IReadOnlyList<string> GetBlockedDomainEventTypes(IScopeContext scopeContext)
	{
		if (_blockedDomainEventNamespaces != null)
			return _blockedDomainEventNamespaces;

		lock (_blockedDomainEventTypesLock)
		{
			if (_blockedDomainEventNamespaces != null)
				return _blockedDomainEventNamespaces;

			_blockedDomainEventNamespaces = UoW.BlockedDomainEventTypeRepository
				.GetAllBlockedDomainEventTypes(new Queries.BlockedDomainEventType.GetAllBlockedDomainEventTypesQuery(false, true, null))
				.ToNamespaces(scopeContext);

			return _blockedDomainEventNamespaces;
		}
	}

	private bool IsBlocked(IScopeContext scopeContext, IDomainEvent domainEvent)
	{
		var blockedDomainEventTypes = GetBlockedDomainEventTypes(scopeContext);
		return blockedDomainEventTypes.Contains(domainEvent.Namespace);
	}

	public bool AddDomainEvent(
		IScopeContext scopeContext,
		IDomainEvent domainEvent,
		string? publisher,
		string? publisherId,
		string? propertiesJson = null)
	{
		Throw.IfArgumentNull(domainEvent);

		if (IsBlocked(scopeContext, domainEvent))
			return false;

		var createResult = Model.DomainEvent.Create(
			scopeContext,
			domainEvent,
			propertiesJson,
			publisher,
			publisherId);

		createResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		UoW.DomainEventRepository
			.Add(scopeContext, createResult.Data!);

		return true;
	}

	public bool AddDomainEvents(
		IScopeContext scopeContext,
		IEnumerable<IDomainEvent> domainEvents,
		string? publisher,
		string? publisherId)
	{
		Throw.IfArgumentNullOrEmpty(domainEvents);

		domainEvents = domainEvents
			.Where(x => !IsBlocked(scopeContext, x));

		if (!domainEvents.Any())
			return false;

		var createResult = Model.DomainEvent.CreateRange(
			scopeContext,
			domainEvents,
			publisher,
			publisherId);

		createResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		UoW.DomainEventRepository
			.AddRange(scopeContext, createResult.Data!);

		return true;
	}

	public int Save(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var saveResult = SaveInternal(scopeContext, force: false);
		saveResult.ThrowIfError(scopeContext, null, true);
		return saveResult.Data;
	}

	public async Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		saveResult.ThrowIfError(scopeContext, null, true);
		return saveResult.Data;
	}
}
