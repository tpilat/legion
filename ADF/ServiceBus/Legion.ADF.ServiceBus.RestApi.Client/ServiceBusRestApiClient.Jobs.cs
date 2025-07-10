using Legion.ADF.ServiceBus.DTOs.Jobs;
using Legion.ADF.ServiceBus.RestApi.Client.Requests;
using Legion.NetHttp;
using Legion.Queries.Sorting;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>
{
	public async Task<IResult<List<JobSummaryDto>>> GetJobsSummaryAsync(
		IScopeContext scopeContext,
		ISortDescriptorBuilder<JobSummaryDto> sortDescriptor,
		int pageIndex,
		int pageSize,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Jobs.V1.GetJobsSummary);

		var result = new ResultBuilder<List<JobSummaryDto>>();

		if (result.IsNull(scopeContext, sortDescriptor))
			return result.Build();

		if (result.IsLessThanOrEqual(scopeContext, pageIndex, 0))
			return result.Build();

		if (result.IsLessThanOrEqual(scopeContext, pageSize, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Jobs.V1.GetJobsSummary,
			timeoutInSeconds,
			queryString: null,
			new
			{
				SortDescriptor = sortDescriptor.Serialize(),
				PageIndex = pageIndex,
				PageSize = pageSize
			});

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<List<JobSummaryDto>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<List<JobSummaryDto>>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<JobStatisticsDto>>> GetJobsStatisticsAsync(
		IScopeContext scopeContext,
		DateTime from,
		DateTime to,
		JobExecutionTypeEnum jobExecutionType,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Jobs.V1.GetJobsStatistics);

		var result = new ResultBuilder<List<JobStatisticsDto>>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Jobs.V1.GetJobsStatistics,
			timeoutInSeconds,
			queryString: null);

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

	public async Task<IResult<List<JobExecutionDto>>> GetJobExecutionsAsync(
		IScopeContext scopeContext,
		Guid idJob,
		DateTime from,
		DateTime to,
		int pageIndex,
		int pageSize,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Jobs.V1.GetJobExecutions)
			.AddContextProperty(nameof(idJob), idJob.ToString())
			.AddContextProperty(nameof(from), from.ToString())
			.AddContextProperty(nameof(to), to.ToString());

		var result = new ResultBuilder<List<JobExecutionDto>>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Jobs.V1.GetJobExecutions,
			timeoutInSeconds,
			queryString: null,
			new
			{
				IdJob = idJob,
				From = from,
				To = to,
				PageIndex = pageIndex,
				PageSize = pageSize
			});

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

	public async Task<IResult<List<JobLogDto>>> GetJobLogsAsync(
		IScopeContext scopeContext,
		Guid idJob,
		DateTime from,
		DateTime to,
		int pageIndex,
		int pageSize,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Jobs.V1.GetJobLogs)
			.AddContextProperty(nameof(idJob), idJob.ToString())
			.AddContextProperty(nameof(from), from.ToString())
			.AddContextProperty(nameof(to), to.ToString());

		var result = new ResultBuilder<List<JobLogDto>>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Jobs.V1.GetJobLogs,
			timeoutInSeconds,
			queryString: null,
			new
			{
				IdJob = idJob,
				From = from,
				To = to,
				PageIndex = pageIndex,
				PageSize = pageSize
			});

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
}
