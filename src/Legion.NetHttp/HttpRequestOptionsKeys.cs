using Legion.Transactions;

namespace Legion.NetHttp;

internal class HttpRequestOptionsKeys
{
	public const string ServiceProviderHttpRequestOptionsKeyName = nameof(IServiceProvider);
	public const string ScopeContextHttpRequestOptionsKeyName = nameof(IScopeContext);
	public const string TransactionsControllerHttpRequestOptionsKeyName = nameof(ITransactionsController);
	public const string DictionaryItemsHttpRequestOptionsKeyName = $"ITEMS_{nameof(Dictionary<string, object?>)}";

#if NET6_0_OR_GREATER
	internal static readonly Lazy<HttpRequestOptionsKey<IServiceProvider>> ServiceProviderHttpRequestOptionsKey
		= new(() => new(ServiceProviderHttpRequestOptionsKeyName));

	internal static readonly Lazy<HttpRequestOptionsKey<IScopeContext>> ScopeContextHttpRequestOptionsKey
		= new(() => new(ScopeContextHttpRequestOptionsKeyName));

	internal static readonly Lazy<HttpRequestOptionsKey<ITransactionsController>> TransactionsControllerHttpRequestOptionsKey
		= new(() => new(TransactionsControllerHttpRequestOptionsKeyName));

	internal static readonly Lazy<HttpRequestOptionsKey<Dictionary<string, object?>>> DictionaryItemsHttpRequestOptionsKey
		= new(() => new(DictionaryItemsHttpRequestOptionsKeyName));
#endif
}
