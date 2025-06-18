using Legion.Model;
using Legion.Model.Messaging;

namespace Legion.ADF.Messaging.DomainEvents.Services;

public partial class DomainEventStore : IDomainEventStore, IDisposable, IAsyncDisposable
{
	public async Task<IResult<Guid?>> SaveDomainEventAsync(
		IScopeContext scopeContext,
		IDomainEvent domainEventContent,
		string? propertiesJson,
		string? publisher,
		string? publisherId,
		bool checkMessageExists,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(domainEventContent.Id), domainEventContent?.Id.ToString());

		var result = new ResultBuilder<Guid?>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, domainEventContent))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(MessagingPermissions.DomainEvent.SaveDomainEventAsync);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, (Model.DomainEvent?)null) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		if (checkMessageExists)
		{
			var existingIdDomainEvent = await UoW.DomainEventRepository
				.ExistsDomainEventByIdDomainEvent(new Queries.DomainEvent.ExistsDomainEventByIdDomainEventQuery(
					domainEventContent.Id,
					checkPermissions,
					AsNoTracking: true))
				.GetIdDomainEventAsync(scopeContext, cancellationToken);

			if (existingIdDomainEvent.HasValue && existingIdDomainEvent != Guid.Empty)
			{
				var warningMessage = $"{nameof(Model.DomainEvent)} with {nameof(domainEventContent.Id)} = {domainEventContent.Id} already exists.";
				scopeContext.Logger?.LogWarningMessage(scopeContext, null, x => x.InternalMessage(warningMessage));
				result.WithWarning(scopeContext, null, warningMessage);
				return result.WithData(existingIdDomainEvent.Value).Build();
			}
		}

		if (IsBlocked(scopeContext, domainEventContent))
			return result.WithData(null).Build();

		var createResult = Model.DomainEvent.Create(
			scopeContext,
			domainEventContent,
			propertiesJson,
			publisher,
			publisherId);

		if (result.MergeHasError(createResult))
			return result.Build();

		var dbDomainEvent = createResult.Data!;

		UoW.DomainEventRepository.Add(scopeContext, dbDomainEvent);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbDomainEvent.IdDomainEvent).Build();
	}
}
