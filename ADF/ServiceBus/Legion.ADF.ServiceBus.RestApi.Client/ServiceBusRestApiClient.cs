using Legion.ADF.ServiceBus.RestApi.Client.Requests;
using Legion.NetHttp;
using Legion.Queries.Sorting;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>
{
	public async Task<IResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobSummaryDto>>> GetJobsSummaryAsync(
		IScopeContext scopeContext,
		ISortDescriptorBuilder<Legion.ADF.ServiceBus.DTOs.Jobs.JobSummaryDto> sortDescriptor,
		int pageIndex,
		int pageSize,
		int? timeoutInSeconds,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.GetJobsSummary);

		var result = new ResultBuilder<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobSummaryDto>>();

		if (result.IsNull(scopeContext, sortDescriptor))
			return result.Build();

		if (result.IsLessThanOrEqual(scopeContext, pageIndex, 0))
			return result.Build();

		if (result.IsLessThanOrEqual(scopeContext, pageSize, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.GetJobsSummary,
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
			var jsonResponse = await ToJsonResultAsync<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobSummaryDto>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobSummaryDto>>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobStatisticsDto>>> GetJobsStatisticsAsync(
		IScopeContext scopeContext,
		int? timeoutInSeconds,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.GetJobsStatistics);

		var result = new ResultBuilder<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobStatisticsDto>>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.GetJobsStatistics,
			timeoutInSeconds,
			queryString: null);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobStatisticsDto>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobStatisticsDto>>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobExecutionDto>>> GetJobExecutionsAsync(
		IScopeContext scopeContext,
		Guid idJob,
		DateTime from,
		DateTime to,
		int pageIndex,
		int pageSize,
		int? timeoutInSeconds,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.GetJobExecutions)
			.AddContextProperty(nameof(idJob), idJob.ToString())
			.AddContextProperty(nameof(from), from.ToString())
			.AddContextProperty(nameof(to), to.ToString());

		var result = new ResultBuilder<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobExecutionDto>>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.GetJobExecutions,
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
			var jsonResponse = await ToJsonResultAsync<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobExecutionDto>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobExecutionDto>>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobLogDto>>> GetJobLogsAsync(
		IScopeContext scopeContext,
		Guid idJob,
		DateTime from,
		DateTime to,
		int pageIndex,
		int pageSize,
		int? timeoutInSeconds,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.GetJobLogs)
			.AddContextProperty(nameof(idJob), idJob.ToString())
			.AddContextProperty(nameof(from), from.ToString())
			.AddContextProperty(nameof(to), to.ToString());

		var result = new ResultBuilder<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobLogDto>>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.GetJobLogs,
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
			var jsonResponse = await ToJsonResultAsync<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobLogDto>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobLogDto>>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}
}
