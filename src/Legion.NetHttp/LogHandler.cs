using Legion.Extensions;
using Legion.Http;
using Legion.Transactions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Legion.NetHttp;

/// <inheritdoc />
internal class LogHandler<TOptions, TCorrelation> : DelegatingHandler
	where TOptions : HttpApiClientOptions
{
	private readonly TOptions _options;
	private readonly ILogger _errorLogger;

	public LogHandler(IOptions<TOptions> options, IServiceProvider serviceProvider, ILogger<LogHandler<TOptions, TCorrelation>> errorLogger)
	{
		_options = options?.Value ?? throw new ArgumentNullException(nameof(options));
		_errorLogger = errorLogger ?? throw new ArgumentNullException(nameof(errorLogger));
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var uri = request.RequestUri?.ToString();
		Stopwatch? sw = null;

#if NET6_0_OR_GREATER
		if (!request.Options.TryGetValue(HttpRequestOptionsKeys.ServiceProviderHttpRequestOptionsKey.Value, out IServiceProvider? serviceProvider))
		{
			var exception = $"{nameof(serviceProvider)} == null";
			_errorLogger.LogErrorMessage(
				_options.SourceSystemName,
				Legion.NetHttp.Exceptions.Internal.ErrorCodes.HttpApiClientException.MissingServiceProvider,
				x => x.InternalMessage(exception).Detail($"{nameof(LogHandler<TOptions, TCorrelation>)}.{nameof(SendAsync)}"),
				true);

			throw new InvalidOperationException(exception);
		}

		if (!request.Options.TryGetValue(HttpRequestOptionsKeys.ScopeContextHttpRequestOptionsKey.Value, out IScopeContext? scopeContext))
		{
			var exception = $"{nameof(scopeContext)} == null";
			_errorLogger.LogErrorMessage(
				_options.SourceSystemName,
				Legion.NetHttp.Exceptions.Internal.ErrorCodes.HttpApiClientException.MissingScopeContext,
				x => x.InternalMessage(exception).Detail($"{nameof(LogHandler<TOptions, TCorrelation>)}.{nameof(SendAsync)}"),
				true);

			throw new InvalidOperationException(exception);
		}

		request.Options.TryGetValue(HttpRequestOptionsKeys.TransactionsControllerHttpRequestOptionsKey.Value, out ITransactionsController? transactionsController);
		request.Options.TryGetValue(HttpRequestOptionsKeys.DictionaryItemsHttpRequestOptionsKey.Value, out Dictionary<string, object?>? items);

#elif NETSTANDARD2_0 || NETSTANDARD2_1
		if (!request.Properties.TryGetValue(HttpRequestOptionsKeys.ServiceProviderHttpRequestOptionsKeyName, out IServiceProvider? serviceProvider))
		{
			var exception = $"{nameof(serviceProvider)} == null";
			_errorLogger.LogErrorMessage(
				_options.SourceSystemName,
				Legion.NetHttp.Exceptions.Internal.ErrorCodes.HttpApiClientException.MissingServiceProvider,
				x => x.InternalMessage(exception).Detail($"{nameof(LogHandler<TOptions, TCorrelation>)}.{nameof(SendAsync)}"),
				true);

			throw new InvalidOperationException(exception);
		}
		
		if (!request.Properties.TryGetValue(HttpRequestOptionsKeys.ScopeContextHttpRequestOptionsKeyName, out IScopeContext? scopeContext))
		{
			var exception = $"{nameof(scopeContext)} == null";
			_errorLogger.LogErrorMessage(
				_options.SourceSystemName,
				Legion.NetHttp.Exceptions.Internal.ErrorCodes.HttpApiClientException.MissingScopeContext,
				x => x.InternalMessage(exception).Detail($"{nameof(LogHandler<TOptions, TCorrelation>)}.{nameof(SendAsync)}"),
				true);

			throw new InvalidOperationException(exception);
		}

		request.Properties.TryGetValue(HttpRequestOptionsKeys.TransactionsControllerHttpRequestOptionsKeyName, out ITransactionsController? transactionsController);
		request.Properties.TryGetValue(HttpRequestOptionsKeys.DictionaryItemsHttpRequestOptionsKeyName, out Dictionary<string, object?>? items);
#endif

		scopeContext = ScopeContext.Create(scopeContext!);

		TCorrelation? requestLogIdentifier = default;
		var logger = _options.GetLogger<TCorrelation>(uri, serviceProvider);
		if (logger != null && _options.LogRequest)
		{
			var requestDto = await RequestDtoMapper.MapAsync(request, null, scopeContext, true, false, false, cancellationToken).ConfigureAwait(false);
			var httpContentDto = HttpContentHelper.ParseHttpContent(request.Content);

			try
			{
				requestLogIdentifier = await logger.LogRequestAsync(
					requestDto,
					httpContentDto,
					scopeContext,
					serviceProvider!,
					transactionsController,
					_options.ClientName,
					_options.LogRequestPayload,
					items,
					cancellationToken).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				_errorLogger.LogErrorMessage(
					_options.SourceSystemName,
					Legion.NetHttp.Exceptions.Internal.ErrorCodes.HttpApiClientException.SendError,
					x => x.ExceptionInfo(ex).Detail($"{nameof(LogHandler<TOptions, TCorrelation>)}.{nameof(SendAsync)} - {nameof(logger.LogRequestAsync)}"),
					true);
			}

			sw = Stopwatch.StartNew();
		}

		HttpResponseMessage? response = null;
		string? error = null;
		try
		{
			response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

			if (logger != null && requestLogIdentifier != null && _options.LogResponse)
			{
				sw?.Stop();

				var responseDto = await ResponseDtoMapper.MapAsync(response, scopeContext, error, sw?.ElapsedMilliseconds, true, false, false, cancellationToken).ConfigureAwait(false);
				var httpContentDto = HttpContentHelper.ParseHttpContent(response?.Content);

				try
				{
					await logger.LogResponseAsync(
						requestLogIdentifier,
						responseDto,
						httpContentDto,
						scopeContext,
						serviceProvider!,
						transactionsController,
						_options.ClientName,
						_options.LogResponsePayload,
						items,
						cancellationToken).ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					_errorLogger.LogErrorMessage(
						_options.SourceSystemName,
						Legion.NetHttp.Exceptions.Internal.ErrorCodes.HttpApiClientException.SendError,
						x => x.ExceptionInfo(ex).Detail($"{nameof(LogHandler<TOptions, TCorrelation>)}.{nameof(SendAsync)} - {nameof(logger.LogResponseAsync)}"),
						true);
				}
			}

			return response!;
		}
		catch (Exception ex)
		{
			error = ex.ToStringTrace();

			if (logger != null && requestLogIdentifier != null && _options.LogResponse)
			{
				sw?.Stop();

				var responseDto = await ResponseDtoMapper.MapAsync(response, scopeContext, error, sw?.ElapsedMilliseconds, true, false, false, cancellationToken).ConfigureAwait(false);
				var httpContentDto = HttpContentHelper.ParseHttpContent(response?.Content);

				try
				{
					await logger.LogResponseAsync(
						requestLogIdentifier,
						responseDto,
						httpContentDto,
						scopeContext,
						serviceProvider!,
						transactionsController,
						_options.ClientName,
						_options.LogResponsePayload,
						items,
						cancellationToken).ConfigureAwait(false);
				}
				catch (Exception exLog)
				{
					_errorLogger.LogErrorMessage(
						_options.SourceSystemName,
						Legion.NetHttp.Exceptions.Internal.ErrorCodes.HttpApiClientException.SendError,
						x => x.ExceptionInfo(exLog).Detail($"{nameof(LogHandler<TOptions, TCorrelation>)}.{nameof(SendAsync)} - {nameof(logger.LogResponseAsync)}"),
						true);
				}
			}

			throw;
		}
	}
}
