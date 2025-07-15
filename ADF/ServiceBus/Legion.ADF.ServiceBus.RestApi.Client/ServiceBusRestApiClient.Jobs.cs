using Legion.ADF.ServiceBus.DTOs.Jobs;
using Legion.ADF.ServiceBus.RestApi.Client.Requests;
using Legion.NetHttp;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>, IServiceBusMonitor
{
	public async Task<IResult<JobDetailDto>> GetJobDetailV1Async(
		IScopeContext scopeContext,
		DTOs.Jobs.GetJobDetailRequest req,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Job.V1.GetDetail);

		var result = new ResultBuilder<JobDetailDto>();

		if (result.IsArgumentNull(scopeContext, req))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Job.V1.GetDetail,
			timeoutInSeconds,
			queryString: null,
			req);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<JobDetailDto>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<JobDetailDto>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<JobStatisticsDto>>> GetJobStatisticsV1Async(
		IScopeContext scopeContext,
		DTOs.Jobs.GetJobStatisticsRequest req,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Job.V1.GetStatistics);

		var result = new ResultBuilder<List<JobStatisticsDto>>();

		if (result.IsArgumentNull(scopeContext, req))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Job.V1.GetStatistics,
			timeoutInSeconds,
			queryString: null,
			req);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<List<JobStatisticsDto>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<List<JobStatisticsDto>>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<JobExecutionDto>>> GetJobExecutionsV1Async(
		IScopeContext scopeContext,
		DTOs.Jobs.GetJobExecutionsRequest req,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Job.V1.GetExecutions);

		var result = new ResultBuilder<List<JobExecutionDto>>();

		if (result.IsArgumentNull(scopeContext, req))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Job.V1.GetExecutions,
			timeoutInSeconds,
			queryString: null,
			req);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<List<JobExecutionDto>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<List<JobExecutionDto>>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<JobLogDto>>> GetJobLogsV1Async(
		IScopeContext scopeContext,
		DTOs.Jobs.GetJobLogsRequest req,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Job.V1.GetLogs);

		var result = new ResultBuilder<List<JobLogDto>>();

		if (result.IsArgumentNull(scopeContext, req))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Job.V1.GetLogs,
			timeoutInSeconds,
			queryString: null,
			req);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<List<JobLogDto>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<List<JobLogDto>>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	async Task<IResult<JobDetailDto>> IServiceBusMonitor.GetJobDetailAsync(
		IScopeContext scopeContext,
		Guid idJob,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<JobDetailDto>();

		var res = await GetJobDetailV1Async(
			scopeContext,
			new GetJobDetailRequest { IdJob = idJob },
			timeoutInSeconds: null,
			cancellationToken).ConfigureAwait(false);

		result.MergeAllWithDataHasError(res);
		return result.Build();
	}

	async Task<IResult<List<JobStatisticsDto>>> IServiceBusMonitor.GetJobStatisticsAsync(
		IScopeContext scopeContext,
		GetJobStatisticsRequest request,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<JobStatisticsDto>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build();

		var res = await GetJobStatisticsV1Async(
			scopeContext,
			request,
			timeoutInSeconds: null,
			cancellationToken).ConfigureAwait(false);

		result.MergeAllWithDataHasError(res);
		return result.Build();
	}

	async Task<IResult<List<JobExecutionDto>>> IServiceBusMonitor.GetJobExecutionsAsync(
		IScopeContext scopeContext,
		GetJobExecutionsRequest request,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<JobExecutionDto>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build();

		var res = await GetJobExecutionsV1Async(
			scopeContext,
			request,
			timeoutInSeconds: null,
			cancellationToken).ConfigureAwait(false);

		result.MergeAllWithDataHasError(res);
		return result.Build();
	}

	async Task<IResult<List<JobLogDto>>> IServiceBusMonitor.GetJobLogsAsync(
		IScopeContext scopeContext,
		GetJobLogsRequest request,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<JobLogDto>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build();

		var res = await GetJobLogsV1Async(
			scopeContext,
			request,
			timeoutInSeconds: null,
			cancellationToken).ConfigureAwait(false);

		result.MergeAllWithDataHasError(res);
		return result.Build();
	}
}
