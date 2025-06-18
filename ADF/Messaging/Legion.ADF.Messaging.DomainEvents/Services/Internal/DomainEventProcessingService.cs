using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Logging;
using Legion.MessageBus;
using Legion.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.DomainEvents.Services.Internal;

public class DomainEventProcessingService : BackgroundService
{
	private readonly DomainEventProcessingServiceOptions _options;
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<DomainEventProcessingService> _logger;

	public DomainEventProcessingService(
		IOptions<DomainEventProcessingServiceOptions> options,
		IServiceProvider serviceProvider,
		ILogger<DomainEventProcessingService> logger)
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(logger);

		_options = options.Value;
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	private IDomainEventsUnitOfWork CreateUnitOfWork(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		string messagingDomainEventsStoreId)
	{
		var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			serviceProvider!,
			messagingDomainEventsStoreId,
			transactionIsolationLevel: null,
			false,
			createAuditEntryStore: false);

		var domainEventsUowResult = connectionProvider.UnitOfWorkProvider.Create<IDomainEventsUnitOfWork>(scopeContext);

		if (domainEventsUowResult.HasError)
			domainEventsUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.DomainEventsUnitOfWorkException.InvalidUoW, true);

		var uow = domainEventsUowResult.Data!;
		return uow;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var scopeContextGlobal = ScopeContext.Create(nameof(DomainEventProcessingService));

		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = ScopeContext.Create(scopeContextGlobal, correlationId: Guid.NewGuid());

			_logger.LogTraceMessage(scopeContext, x => x.InternalMessage($"{nameof(DomainEventProcessingService)}.{nameof(ExecuteAsync)}: START"));

			List<Guid> domainEventIds = [];

			try
			{
				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;

				var connectionProviderFactory = scopedServiceProvider.GetRequiredService<IConnectionProviderFactory>();
				var options = scopedServiceProvider.GetRequiredService<IOptions<MessagingDomainEventsStoreOptions>>().Value;

				var domainEventsUoW = CreateUnitOfWork(
					scopeContext,
					scopedServiceProvider,
					connectionProviderFactory,
					options.MessagingDomainEventsStoreId);

				IReadOnlyList<string> blockedDomainEventNamespaces;

				await using (var connectionProvider = domainEventsUoW.ConnectionProvider)
				{
					var nowUtc = GlobalContext.Instance.UtcNow;
					domainEventIds = await domainEventsUoW.DomainEventRepository.GetNextDomainEvents(
						new Queries.DomainEvent.GetNextDomainEventsQuery(
							BatchCount: _options.MessagesBatchCount,
							NowUtc: nowUtc,
							CheckReadPermissions: false,
							AsNoTracking: true,
							QueryableBuilder: null))
						.ToDomainEventIds(scopeContext, cancellationToken);

					blockedDomainEventNamespaces = domainEventsUoW.BlockedDomainEventTypeRepository
						.GetAllBlockedDomainEventTypes(new Queries.BlockedDomainEventType.GetAllBlockedDomainEventTypesQuery(false, true, null))
						.ToNamespaces(scopeContext);
				}

				if (_options.MaxDegreeOfParallelism.HasValue)
				{
					var parallelOptions = new ParallelOptions
					{
						MaxDegreeOfParallelism = _options.MaxDegreeOfParallelism.Value,
						CancellationToken = cancellationToken
					};

					await Parallel.ForEachAsync(
						domainEventIds,
						parallelOptions,
						async (idDomainEvent, cancelToken) =>
						{
							await ProcessDomainEvent(scopeContext, idDomainEvent, blockedDomainEventNamespaces, scopedServiceProvider, connectionProviderFactory, options, cancelToken);
						});
				}
				else
				{
					foreach (var idDomainEvent in domainEventIds)
					{
						await ProcessDomainEvent(scopeContext, idDomainEvent, blockedDomainEventNamespaces, scopedServiceProvider, connectionProviderFactory, options, cancellationToken);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogErrorMessage(scopeContext.CreateNew(), Exceptions.Internal.ErrorCodes.DomainEventProcessingService.Default, x => x.ExceptionInfo(ex));
			}
			finally
			{
				//only if no domain event was processed
				if (domainEventIds.Count == 0 && !cancellationToken.IsCancellationRequested)
				{
					try
					{
						await Task.Delay(_options.IdleTimeout, cancellationToken);
					}
					catch (Exception ex)
					{
						_logger.LogErrorMessage(scopeContext.CreateNew(), Exceptions.Internal.ErrorCodes.DomainEventProcessingService.Default, x => x.ExceptionInfo(ex));
					}
				}
			}
		}
	}

	private async Task ProcessDomainEvent(
		IScopeContext scopeContext,
		Guid idDomainEvent,
		IReadOnlyList<string> blockedDomainEventNamespaces,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingDomainEventsStoreOptions options,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext, correlationId: Guid.NewGuid())
			.AddContextProperty(nameof(idDomainEvent), idDomainEvent.ToString());

		try
		{
			Model.DomainEvent? domainEvent;

			var domainEventsUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				options.MessagingDomainEventsStoreId);

			await using (var connectionProvider = domainEventsUoW.ConnectionProvider)
			{
				domainEvent = await domainEventsUoW.DomainEventRepository.GetDomainEventById(
					new Queries.DomainEvent.GetDomainEventByIdQuery(
						idDomainEvent,
						IncludeContent: true,
						CheckReadPermissions: false,
						AsNoTracking: false,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				if (domainEvent == null)
					Throw.IfNull(domainEvent, scopeContext);

				var setResult = domainEvent.SetProcessing(scopeContext, _options.NextProcessingTimeout, _options.DisableProcessingLog, _options.DisableMultiProcessingLog);
				setResult.ThrowIfError(scopeContext, null, true);

				if (setResult.Data != null)
					domainEventsUoW.DomainEventProcessingLogRepository.Add(scopeContext, setResult.Data);

				var saveResult = await domainEventsUoW.SaveAsync(scopeContext);
				saveResult.ThrowIfError(scopeContext, null, true);

				var commitResult = connectionProvider.CommitAll(scopeContext);
				commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				if (commitResult.Data != true)
					Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
			}

			if (blockedDomainEventNamespaces.Contains(domainEvent.Namespace))
			{
				var warningMessage = new LogMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.DomainEventProcessingService.BlockedDomainEvent(domainEvent.Namespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				_logger.LogWarningMessage(warningMessage);

				await SetBlocked(
					scopeContext,
					idDomainEvent,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					options,
					cancellationToken);

				return;
			}

			var domainEventType = Type.GetType(domainEvent.Namespace);
			var deserializedDomainEvent = domainEventType != null
				? JsonSerializerHelper.Deserialize(
					domainEvent.Content.Content,
					domainEventType!,
					new Newtonsoft.Json.JsonSerializerSettings
					{
						Formatting = Newtonsoft.Json.Formatting.None,
						ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
						PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects, //PreserveReferencesHandling.All,
						TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
						MaxDepth = 255,
						ContractResolver = new Legion.Serializer.JsonConverters.PrivateSetterContractResolver()
					})
				: null;

			if (deserializedDomainEvent is Legion.Model.IDomainEvent idomainEvent)
			{
				var messageBus = serviceProvider.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
				var publisResult = await messageBus.PublishAsync(scopeContext, idomainEvent, connectionProvider: null, cancellationToken);
				if (publisResult.HasError)
				{
					_logger.LogResultErrorMessages(publisResult, true, true);
					var errorMessage = publisResult.ErrorMessages[0];

					await SetFailedDomainEvent(
						scopeContext,
						idDomainEvent,
						_options.MaxRetryCount,
						_options.NextProcessingTimeout,
						errorMessage,
						serviceProvider,
						connectionProviderFactory,
						options,
						cancellationToken);

					return;
				}
				else if (!publisResult.DataWasSet || publisResult.Data != true)
				{
					var warningMessage = new LogMessageBuilder(
						scopeContext,
						Exceptions.Internal.ErrorCodes.DomainEventProcessingService.DomainEventHasNoHandler(deserializedDomainEvent?.GetType()))
						.LogLevel(LogLevel.Warning)
						.Build();

					_logger.LogWarningMessage(warningMessage);

					await SetNoHandler(
						scopeContext,
						idDomainEvent,
						warningMessage,
						serviceProvider,
						connectionProviderFactory,
						options,
						cancellationToken);

					return;
				}
				else
				{
					await SetProcessedDomainEvent(
						scopeContext,
						idDomainEvent,
						serviceProvider,
						connectionProviderFactory,
						options,
						cancellationToken);

					return;
				}
			}
			else
			{
				var errorMessage = new ErrorMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.DomainEventProcessingService.InvalidDomainEventType(deserializedDomainEvent?.GetType()))
					.Build();

				_logger.LogErrorMessage(errorMessage);

				await SetSuspendedDomainEvent(
					scopeContext,
					idDomainEvent,
					_options.MaxRetryCount,
					_options.NextProcessingTimeout,
					errorMessage,
					serviceProvider,
					connectionProviderFactory,
					options,
					cancellationToken);

				return;
			}
		}
		catch (Exception ex)
		{
			_logger.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.DomainEventProcessingService.CanNotSetState(nameof(Model.DomainEventProcessingStatus.Processing)), x => x.ExceptionInfo(ex));
		}
	}

	private async Task SetProcessedDomainEvent(
		IScopeContext scopeContext,
		Guid idDomainEvent,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingDomainEventsStoreOptions options,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var domainEventsUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				options.MessagingDomainEventsStoreId);

			await using var connectionProvider = domainEventsUoW.ConnectionProvider;

			var domainEvent = await domainEventsUoW.DomainEventRepository.GetDomainEventById(
				new Queries.DomainEvent.GetDomainEventByIdQuery(
					idDomainEvent,
					IncludeContent: true,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			if (domainEvent == null)
				Throw.IfNull(domainEvent, scopeContext);

			var setResult = domainEvent.SetProcessed(scopeContext, _options.DisableProcessingLog);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				domainEventsUoW.DomainEventProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await domainEventsUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfError(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			_logger.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.DomainEventProcessingService.CanNotSetState(nameof(Model.DomainEventProcessingStatus.Processed)), x => x.ExceptionInfo(ex));
		}
	}

	private async Task SetNoHandler(
		IScopeContext scopeContext,
		Guid idDomainEvent,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingDomainEventsStoreOptions options,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var domainEventsUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				options.MessagingDomainEventsStoreId);

			await using var connectionProvider = domainEventsUoW.ConnectionProvider;

			var domainEvent = await domainEventsUoW.DomainEventRepository.GetDomainEventById(
				new Queries.DomainEvent.GetDomainEventByIdQuery(
					idDomainEvent,
					IncludeContent: true,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			if (domainEvent == null)
				Throw.IfNull(domainEvent, scopeContext);

			var setResult = domainEvent.SetNoHandler(scopeContext, _options.DisableProcessingLog, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				domainEventsUoW.DomainEventProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await domainEventsUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfError(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			_logger.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.DomainEventProcessingService.CanNotSetState(nameof(Model.DomainEventProcessingStatus.Processed)), x => x.ExceptionInfo(ex));
		}
	}

	private async Task SetBlocked(
		IScopeContext scopeContext,
		Guid idDomainEvent,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingDomainEventsStoreOptions options,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var domainEventsUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				options.MessagingDomainEventsStoreId);

			await using var connectionProvider = domainEventsUoW.ConnectionProvider;

			var domainEvent = await domainEventsUoW.DomainEventRepository.GetDomainEventById(
				new Queries.DomainEvent.GetDomainEventByIdQuery(
					idDomainEvent,
					IncludeContent: true,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			if (domainEvent == null)
				Throw.IfNull(domainEvent, scopeContext);

			var setResult = domainEvent.SetBlocked(scopeContext, _options.DisableProcessingLog, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				domainEventsUoW.DomainEventProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await domainEventsUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfError(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			_logger.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.DomainEventProcessingService.CanNotSetState(nameof(Model.DomainEventProcessingStatus.Processed)), x => x.ExceptionInfo(ex));
		}
	}

	private async Task SetFailedDomainEvent(
		IScopeContext scopeContext,
		Guid idDomainEvent,
		int maxRetryCount,
		TimeSpan nextProcessingDelay,
		IErrorMessage errorMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingDomainEventsStoreOptions options,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var domainEventsUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				options.MessagingDomainEventsStoreId);

			await using var connectionProvider = domainEventsUoW.ConnectionProvider;

			var domainEvent = await domainEventsUoW.DomainEventRepository.GetDomainEventById(
				new Queries.DomainEvent.GetDomainEventByIdQuery(
					idDomainEvent,
					IncludeContent: true,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			if (domainEvent == null)
				Throw.IfNull(domainEvent, scopeContext);

			var setResult = domainEvent.SetFailed(
				scopeContext,
				maxRetryCount,
				nextProcessingDelay,
				_options.DisableProcessingLog,
				errorMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				domainEventsUoW.DomainEventProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await domainEventsUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfError(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			_logger.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.DomainEventProcessingService.CanNotSetState(nameof(Model.DomainEventProcessingStatus.Failed)), x => x.ExceptionInfo(ex));
		}
	}

	private async Task SetSuspendedDomainEvent(
		IScopeContext scopeContext,
		Guid idDomainEvent,
		int maxRetryCount,
		TimeSpan nextProcessingDelay,
		IErrorMessage errorMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingDomainEventsStoreOptions options,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var domainEventsUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				options.MessagingDomainEventsStoreId);

			await using var connectionProvider = domainEventsUoW.ConnectionProvider;

			var domainEvent = await domainEventsUoW.DomainEventRepository.GetDomainEventById(
				new Queries.DomainEvent.GetDomainEventByIdQuery(
					idDomainEvent,
					IncludeContent: true,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			if (domainEvent == null)
				Throw.IfNull(domainEvent, scopeContext);

			var setResult = domainEvent.SetSuspended(
				scopeContext,
				_options.DisableProcessingLog,
				errorMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				domainEventsUoW.DomainEventProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await domainEventsUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfError(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			_logger.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.DomainEventProcessingService.CanNotSetState(nameof(Model.DomainEventProcessingStatus.Suspended)), x => x.ExceptionInfo(ex));
		}
	}
}
