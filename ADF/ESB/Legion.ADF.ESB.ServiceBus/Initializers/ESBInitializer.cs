using Legion.ADF.ESB.Components;
using Legion.DependencyInjection;
using Legion.Transactions;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.ESB.ServiceBus.Initializers;

public class ESBInitializer : IStartupTask
{
	public static ESBInitializationStatus ConfigsInitializationStatus => _configsInitializationStatus;

	private static readonly object _configInitLock = new();
	private static ESBInitializationStatus _configsInitializationStatus = ESBInitializationStatus.NotStarted;
	public static bool SetInitializedConfig(ESBInitializationStatus newStatus, out ESBInitializationStatus currentStatus)
	{
		var currStatus = _configsInitializationStatus;
		if (newStatus <= currStatus)
		{
			currentStatus = currStatus;
			return false;
		}

		lock (_configInitLock)
		{
			currStatus = _configsInitializationStatus;
			if (newStatus <= currStatus)
			{
				currentStatus = currStatus;
				return false;
			}

			_configsInitializationStatus = newStatus;
			currentStatus = _configsInitializationStatus;
			return true;
		}
	}

	private readonly ILogger<ESBInitializer> _logger;

	public ESBInitializer(ILogger<ESBInitializer> logger)
	{
		Throw.IfArgumentNull(logger);

		_logger = logger;
	}

	public async Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create(nameof(ESBInitializer), targetStoreId: null);

		if (_logger.IsEnabled(LogLevel.Information))
			_logger.LogInformation("Initialize ESB at: {UtcNow}", GlobalContext.Instance.UtcNow);

		var transactionsController = new TransactionsController();

		await using var invocationContext =
			new InvocationContextBuilder(scopeContext)
			.Initialize(serviceProvider, transactionsController, false)
			.Build();

		var componentsUowResult = invocationContext.CreateUnitOfWork<IComponentsUnitOfWork, ConnectionStringProvider>();
		var componentsUoW = componentsUowResult.Data!;

		var dbAdapters = (await componentsUoW.AdapterRepository
			.GetAllAdapters(new Components.Queries.Adapter.GetAllAdaptersQuery(null))
			.ToResultAsync(invocationContext.InvocationCreateNew(), cancellationToken))
			.ToDictionary(x => x.IdAdapter);

		var esbAdapters = ESBModelRegister.GetAllAdapters(serviceProvider);

		var started = SetInitializedConfig(ESBInitializationStatus.Started, out var currentStatus);
		if (!started)
			Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.ESBInitializerException.CannotSetInitStatus(currentStatus.ToString(), ESBInitializationStatus.Started.ToString()), invocationContext.InvocationCreateNew());

		foreach (var esbAdapter in esbAdapters)
		{
			var ic = invocationContext.InvocationCreateNew()
				.InvocationAddContextProperty(nameof(esbAdapter.IdAdapter), esbAdapter.IdAdapter.ToString(), true);

			if (dbAdapters.TryGetValue(esbAdapter.IdAdapter, out var dbAdapter))
			{
				if (!string.Equals(dbAdapter.Class, esbAdapter.Class, StringComparison.InvariantCulture))
				{
					Throw.InvalidOperationException(
						Exceptions.Internal.ErrorCodes.ESBInitializerException.InvalidAdapterClass(esbAdapter.IdAdapter, esbAdapter.Class, dbAdapter.Class),
						ic.InvocationCreateNew());
				}

				var updateResult = dbAdapter.Update(ic.InvocationCreateNew(), esbAdapter);
				Throw.ResultExceptionIfHasError(
					ic.InvocationCreateNew(),
					Exceptions.Internal.ErrorCodes.ESBInitializerException.CannotUpdateAdapter(esbAdapter.IdAdapter),
					updateResult,
					true,
					true,
					_logger);

			}
			else //insert to DB
			{
				var persitResult = esbAdapter.ToPersistentModel(ic.InvocationCreateNew());
				Throw.ResultExceptionIfHasError(
					ic.InvocationCreateNew(),
					Exceptions.Internal.ErrorCodes.ESBInitializerException.CannotInsertAdapter(esbAdapter.IdAdapter),
					persitResult,
					true,
					true,
					_logger);

				dbAdapter = persitResult.Data!;
				componentsUoW.AdapterRepository.Add(ic.InvocationCreateNew(), dbAdapter);
			}
		}

		var finished = SetInitializedConfig(ESBInitializationStatus.Finished, out currentStatus);
		if (!finished)
			Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.ESBInitializerException.CannotSetInitStatus(currentStatus.ToString(), ESBInitializationStatus.Finished.ToString()), invocationContext.InvocationCreateNew());

		var saveResult = await componentsUoW.SaveAsync(invocationContext.InvocationCreateNew());
		saveResult.ThrowIfError(invocationContext, null/*//TODO*/, true);

		await transactionsController.CommitAllAsync(invocationContext.InvocationCreateNew(), true, cancellationToken);

		Console.WriteLine();
	}
}
