using System.Runtime.CompilerServices;

namespace Legion.Model.Messaging;

public interface IDomainEventStore : IDisposable, IAsyncDisposable
{
	bool AddDomainEvent(
		IScopeContext scopeContext,
		IDomainEvent domainEvent,
		string? publisher,
		string? publisherId,
		string? propertiesJson = null);

	bool AddDomainEvents(
		IScopeContext scopeContext,
		IEnumerable<IDomainEvent> domainEvents,
		string? publisher,
		string? publisherId);

	int Save(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);
}
