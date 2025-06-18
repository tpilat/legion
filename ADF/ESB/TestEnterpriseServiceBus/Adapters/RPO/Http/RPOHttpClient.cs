using Legion;
using Legion.NetHttp;
using Legion.Results;
using TestEnterpriseServiceBus.Adapters.RPO.Http.Requests;
using TestEnterpriseServiceBus.Adapters.RPO.Http.Responses;
using TestEnterpriseServiceBus.Exceptions.Internal;

namespace TestEnterpriseServiceBus.Adapters.RPO.Http;

public partial class RPOHttpClient : HttpApiClient
{
	public async Task<IResult<ResultDto<Subject>>> SearchByBusinessIdAsync(
		IScopeContext scopeContext,
		Guid idEsbAdapter,
		string businessId,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		var request = JsonRequestFactory.Create(
			Options,
			Legion.Http.HttpMethod.Get,
			"/search",
			new Dictionary<string, string>
			{
				{ "identifier", businessId },
				{ "onlyActive", "true"}
			},
			idEsbAdapter);

		try
		{
			using var response = await SendAsync(request, scopeContext, null, false, transactionsController: null, cancellationToken);
			var str = await response.ReadContentAsStringAsync(cancellationToken);
			var jsonResponse = await ToJsonResultAsync<ResultDto<Subject>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<ResultDto<Subject>>()
					.WithError(scopeContext, ErrorCodes.RPOHttpClientException.Default, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<ResultDto<bool>>> SearchByBusinessNameAsync(
		IScopeContext scopeContext,
		Guid idEsbAdapter,
		string businessName,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext)
			.AddContextProperty(nameof(businessName), businessName);

		var request = JsonRequestFactory.Create(
			Options,
			Legion.Http.HttpMethod.Get,
			"/search",
			new Dictionary<string, string>
			{
				{ "fullName", businessName },
				{ "onlyActive", "true"}
			},
			idEsbAdapter);

		try
		{
			using var response = await SendAsync(request, scopeContext, null, false, transactionsController: null, cancellationToken);
			var jsonResponse = await ToJsonResultAsync<ResultDto<bool>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<ResultDto<bool>>()
					.WithError(scopeContext, ErrorCodes.RPOHttpClientException.Default, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<ResultDto<Subject>>> GetCompanyDetailAsync(
		IScopeContext scopeContext,
		Guid idEsbAdapter,
		string rpoInternalId,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		var request = JsonRequestFactory.Create(
			Options,
			Legion.Http.HttpMethod.Get,
			$"/entity/{rpoInternalId}",
			new Dictionary<string, string>
			{
				{ "showHistoricalData", "false" },
				{ "showOrganizationUnits", "false"}
			},
			idEsbAdapter);

		try
		{
			using var response = await SendAsync(request, scopeContext, null, false, transactionsController: null, cancellationToken);
			var jsonResponse = await ToJsonResultAsync<ResultDto<Subject>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<ResultDto<Subject>>()
					.WithError(scopeContext, ErrorCodes.RPOHttpClientException.Default, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<ResultDto<Subject>>> GetCompanyDetailByBusinessIdAsync(
		IScopeContext scopeContext,
		Guid idEsbAdapter,
		string rpoInternalId,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext)
			.AddContextProperty(nameof(rpoInternalId), rpoInternalId);

		var request = JsonRequestFactory.Create(
			Options,
			Legion.Http.HttpMethod.Get,
			$"/entity/{rpoInternalId}",
			new Dictionary<string, string>
			{
				{ "showHistoricalData", "false" },
				{ "showOrganizationUnits", "false"}
			},
			idEsbAdapter);

		try
		{
			using var response = await SendAsync(request, scopeContext, null, false, transactionsController: null, cancellationToken);
			var jsonResponse = await ToJsonResultAsync<ResultDto<Subject>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<ResultDto<Subject>>()
					.WithError(scopeContext, ErrorCodes.RPOHttpClientException.Default, x => x.ExceptionInfo(ex));
		}
	}
}
