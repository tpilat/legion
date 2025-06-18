using Legion.Transactions;
using Legion.Web.Logging;

namespace Legion.Http;

public interface IRequestResponseLogger
{
}

public interface IRequestResponseLogger<T> : IRequestResponseLogger
{
	Task<T> LogRequestAsync(
		RequestDto request,
		HttpContentDto requestContent,
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		ITransactionsController? transactionsController,
		string clientName,
		bool logPayload,
		Dictionary<string, object?>? items,
		CancellationToken cancellationToken = default);

	Task LogResponseAsync(
		T requestIdentifier,
		ResponseDto response,
		HttpContentDto responseContent,
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		ITransactionsController? transactionsController,
		string clientName,
		bool logPayload,
		Dictionary<string, object?>? items,
		CancellationToken cancellationToken = default);
}
