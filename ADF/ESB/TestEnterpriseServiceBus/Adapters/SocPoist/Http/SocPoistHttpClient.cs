using Legion;
using Legion.NetHttp;
using Legion.Web;
using TestEnterpriseServiceBus.Adapters.SocPoist.Http.Requests;
using TestEnterpriseServiceBus.Exceptions.Internal;

namespace TestEnterpriseServiceBus.Adapters.SocPoist.Http;

public partial class SocPoistHttpClient : HttpApiClient
{
	public async Task<IResult<string>> DownloadSocPoistHtmlAsync(
		IScopeContext scopeContext,
		Guid idEsbAdapter,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		var request = JsonRequestFactory.Create(
			Options,
			Legion.Http.HttpMethod.Get,
			"/nastroje-sluzby/zoznam-dlznikov",
			null,
			idEsbAdapter);

		try
		{
			using var response = await SendAsync(request, scopeContext, null, false, transactionsController: null, cancellationToken);
			var htmlResult = await ToStringResultAsync(scopeContext, request, response, cancellationToken);
			return htmlResult!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<string>()
					.WithError(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<FormFile>> DownloadSocPoistZipAsync(
		IScopeContext scopeContext,
		Guid idEsbAdapter,
		string relativePath,
		Stream responseStream,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		var request = JsonRequestFactory.Create(
			Options,
			Legion.Http.HttpMethod.Post,
			relativePath,
			null,
			idEsbAdapter);

		try
		{
			using var response = await SendAsync(request, scopeContext, null, false, transactionsController: null, cancellationToken);
			var zipResult = await ToStreamResultAsync(scopeContext, responseStream, request, response, cancellationToken);
			return zipResult!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<FormFile>()
					.WithError(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, x => x.ExceptionInfo(ex));
		}
	}
}
