using Legion.Http;
using Legion.Transactions;
using Legion.Web.Logging;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.ESB.Components;

public class AdapterRequestResponseLogger : IRequestResponseLogger<Guid?>
{
	public const string PARAM_idAdapter = nameof(Components.Model.AdapterRequest.IdAdapter);

	public async Task<Guid?> LogRequestAsync(
		RequestDto request,
		HttpContentDto requestContent,
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		ITransactionsController? transactionsController,
		string clientName,
		bool logPayload,
		Dictionary<string, object?>? items,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(clientName), clientName)
			.AddContextProperty(nameof(request.Path), request?.Path);

		Throw.IfArgumentNull(items);

		bool isLocalTransaction = false;
		if (transactionsController == null)
		{
			transactionsController = new TransactionsController();
			isLocalTransaction = true;
		}

		await using var invocationContext =
			new InvocationContextBuilder(scopeContext)
			.Initialize(serviceProvider, transactionsController, false)
			.Build();

		try
		{
			var componentsUowResult = invocationContext.CreateUnitOfWork<IComponentsUnitOfWork, ConnectionStringProvider>();

			Throw.ResultExceptionIfHasError(
				invocationContext,
				Legion.ADF.ESB.Exceptions.Internal.ErrorCodes.HttpClientRequestResponseLoggerException.InvalidUnitOfWork(nameof(IComponentsUnitOfWork)),
				componentsUowResult,
				true,
				true);

			var componentsUoW = componentsUowResult.Data!;

			if (!items.TryGetValue(PARAM_idAdapter, out var param_idAdapter))
				Throw.OutOfRangeException(items, $"{nameof(param_idAdapter)} == null", scopeContext);

			if (param_idAdapter is not Guid idAdapter)
			{
				Throw.OutOfRangeException(items, $"{nameof(param_idAdapter)} is not Guid", scopeContext);
				throw null;
			}

			var adapterRequestResult = await Model.AdapterRequest.CreateAdapterRequestAsync(
				invocationContext,
				idAdapter,
				request!,
				requestContent,
				clientName,
				logPayload);

			Throw.ResultExceptionIfHasError(
				invocationContext,
				Legion.ADF.ESB.Exceptions.Internal.ErrorCodes.HttpClientRequestResponseLoggerException.Default(nameof(IComponentsUnitOfWork)),
				adapterRequestResult,
				true,
				true);

			var adapterRequest = adapterRequestResult.Data!;
			componentsUoW.AdapterRequestRepository.Add(invocationContext, adapterRequest);

			if (logPayload && 0 < adapterRequest.AdapterRequestPayloads?.Count)
			{
				foreach (var adapterRequestPayload in adapterRequest.AdapterRequestPayloads)
					componentsUoW.AdapterRequestPayloadRepository.Add(invocationContext, adapterRequestPayload);
			}

			if (isLocalTransaction)
			{
				var saveResult = await componentsUoW.SaveAsync(invocationContext, cancellationToken: default);
				saveResult.ThrowIfError(invocationContext, null/*//TODO*/, true);

				var commitResult = await transactionsController.CommitAllAsync(invocationContext, false, cancellationToken);
				commitResult.ThrowIfError(invocationContext, null/*//TODO*/, true);
			}

			return adapterRequest.IdAdapterRequest;
		}
		catch (Exception ex)
		{
			if (isLocalTransaction)
			{
				try
				{
					var rollbackResult = await transactionsController.RollbackAllAsync(invocationContext, ex, false, cancellationToken: default);
					rollbackResult.ThrowIfError(invocationContext, null/*//TODO*/, true);
				}
				catch { }
			}

			throw;
		}
		finally
		{
			if (isLocalTransaction)
			{
				try
				{
					await transactionsController.DisposeAsync();
				}
				catch { }
			}
		}
	}

	public async Task LogResponseAsync(
		Guid? requestIdentifier,
		ResponseDto response,
		HttpContentDto responseContent,
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		ITransactionsController? transactionsController,
		string clientName,
		bool logPayload,
		Dictionary<string, object?>? items,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(clientName), clientName)
			.AddContextProperty(nameof(requestIdentifier), requestIdentifier?.ToString());

		Throw.IfArgumentNull(items);

		if (!requestIdentifier.HasValue)
			return;

		bool isLocalTransaction = false;
		if (transactionsController == null)
		{
			transactionsController = new TransactionsController();
			isLocalTransaction = true;
		}

		await using var invocationContext =
			new InvocationContextBuilder(scopeContext)
			.Initialize(serviceProvider, transactionsController, false)
			.Build();

		try
		{
			var componentsUowResult = invocationContext.CreateUnitOfWork<IComponentsUnitOfWork, ConnectionStringProvider>();

			Throw.ResultExceptionIfHasError(
				invocationContext,
				Legion.ADF.ESB.Exceptions.Internal.ErrorCodes.HttpClientRequestResponseLoggerException.InvalidUnitOfWork(nameof(IComponentsUnitOfWork)),
				componentsUowResult,
				true,
				true);

			var componentsUoW = componentsUowResult.Data!;

			if (!items.TryGetValue(PARAM_idAdapter, out var param_idAdapter))
				Throw.OutOfRangeException(items, $"{nameof(param_idAdapter)} == null", scopeContext);

			if (param_idAdapter is not Guid idAdapter)
			{
				Throw.OutOfRangeException(items, $"{nameof(param_idAdapter)} is not Guid", scopeContext);
				throw null;
			}

			var adapterResponseResult = await Model.AdapterResponse.CreateAdapterResponseAsync(
				invocationContext,
				requestIdentifier.Value,
				idAdapter,
				response!,
				responseContent,
				clientName,
				logPayload);

			Throw.ResultExceptionIfHasError(
				invocationContext,
				Legion.ADF.ESB.Exceptions.Internal.ErrorCodes.HttpClientRequestResponseLoggerException.Default(nameof(IComponentsUnitOfWork)),
				adapterResponseResult,
				true,
				true);

			var adapterResponse = adapterResponseResult.Data!;
			componentsUoW.AdapterResponseRepository.Add(invocationContext, adapterResponse);

			if (logPayload && 0 < adapterResponse.AdapterResponsePayloads?.Count)
			{
				foreach (var adapterResponsePayload in adapterResponse.AdapterResponsePayloads)
					componentsUoW.AdapterResponsePayloadRepository.Add(invocationContext, adapterResponsePayload);
			}

			if (isLocalTransaction)
			{
				var saveResult = await componentsUoW.SaveAsync(invocationContext, cancellationToken: default);
				saveResult.ThrowIfError(invocationContext, null/*//TODO*/, true);

				var commitResult = await transactionsController.CommitAllAsync(invocationContext, false, cancellationToken);
				commitResult.ThrowIfError(invocationContext, null/*//TODO*/, true);
			}
		}
		catch (Exception ex)
		{
			if (isLocalTransaction)
			{
				try
				{
					var rollbackResult = await transactionsController.RollbackAllAsync(invocationContext, ex, false, cancellationToken: default);
					rollbackResult.ThrowIfError(invocationContext, null/*//TODO*/, true);
				}
				catch { }
			}

			throw;
		}
		finally
		{
			if (isLocalTransaction)
			{
				try
				{
					await transactionsController.DisposeAsync();
				}
				catch { }
			}
		}
	}
}
