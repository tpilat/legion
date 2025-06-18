using Legion;
using Legion.ADF.ESB.Components;
using Legion.ADF.ESB.ServiceBus;
using Legion.DependencyInjection;
using Legion.Extensions;
using Legion.Model.Repositories;
using Legion.NetHttp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using System.Text.RegularExpressions;
using TestEnterpriseServiceBus.Adapters.SocPoist.Http;
using TestEnterpriseServiceBus.Adapters.SocPoist.Messages;

namespace TestEnterpriseServiceBus.Adapters.SocPoist;

public class SocPoistClientAdapter : AdapterBase<SocPoistClientAdapterConfig>, IESBAdapter, IServiceCollectionBuilder,
	IMessageSubscriber<SocPoistRequest, SocPoistResponse>
{
	private readonly SocPoistHttpClient _socPoistHttpClient;
	public static readonly Guid ADAPTER_ID = new("00000001-0000-0000-0000-000000000000");

	public SocPoistClientAdapter(IOptionsSnapshot<SocPoistClientAdapterConfig> options, IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory)
		: base(options)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(httpClientFactory);

		var client = httpClientFactory.CreateClient(nameof(SocPoistHttpClient));
		var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
		_socPoistHttpClient = new SocPoistHttpClient(client, serviceProvider, options.Value.SocPoistHttpClientOptions, loggerFactory.CreateLogger<HttpApiClient>());

		IdAdapter = ADAPTER_ID;
		Code = "SocPoistClient";
		Name = "SocPoistClient";
		Description = "SocPoistClient";
		IdAdapterStatus = Legion.ADF.ESB.Components.Model.AdapterStatus.Offline;
		Properties = SerializeProperties();
		IsInbound = false;
		IsOutbound = true;
	}

	public override SocPoistClientAdapter Default()
		=> throw new Legion.Exceptions.NotImplementedException(); //new(Microsoft.Extensions.Options.Options.Create(new SocPoistClientAdapterConfig()), new SocPoistHttpClient());

	public async Task<IResult<SocPoistResponse>> HandleAsync(
		IInvocationContext invocationContext,
		SocPoistRequest message,
		IUnitOfWorkProvider unitOfWorkProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();
		//.InvocationAddContextProperty(nameof(message.IdMessage), message.IdMessage.ToString());

		var result = new ResultBuilder<SocPoistResponse>();

		var htmlResult = await _socPoistHttpClient.DownloadSocPoistHtmlAsync(invocationContext, IdAdapter, cancellationToken);
		if (result.MergeHasError(htmlResult))
			return result.Build();

		var pageSource = htmlResult.Data!;

		var htmlDocument = new HtmlAgilityPack.HtmlDocument();
		htmlDocument.LoadHtml(pageSource);

		var link = htmlDocument
			.DocumentNode
			.SelectSingleNode("//body")
			.Descendants("a")
			.FirstOrDefault(x => x.Attributes != null
				&& x.Attributes.Any(a =>
					a.Name == "href"
					&& a.Value?.StartsWith(Options.HrefPrefix) == true));

		var relativeUri = link?.Attributes.FirstOrDefault(a => a.Name == "href")?.Value;

		var year = GlobalContext.Instance.Now.Year;

		await using var ms = new MemoryStream();
		var zipResult = await _socPoistHttpClient.DownloadSocPoistZipAsync(
			invocationContext,
			IdAdapter,
			relativeUri!,
			ms,
			cancellationToken: default);

		if (result.MergeHasError(zipResult))
			return result.Build();

		var zipFormFile = zipResult.Data!;

		var zipStream = await zipFormFile.OpenReadStreamAsync();
		var csvResult = CsvStreamFromZipStream(invocationContext, zipStream!);
		if (result.MergeHasError(csvResult))
			return result.Build();

		result.WithData(new SocPoistResponse { CSV = csvResult.Data.csvStream.ToArray(), Year = year, Week = csvResult.Data.week});

		return result.Build();
	}

	private static IResult<(Stream csvStream, int week)> CsvStreamFromZipStream(
		IScopeContext scopeContext,
		Stream zipStream)
	{
		scopeContext = ScopeContext.Create(scopeContext);
		var result = new ResultBuilder<(Stream csv, int week)>();

		if (result.IsArgumentNull(scopeContext, zipStream))
			return result.Build();

		if (zipStream.CanSeek)
			zipStream.Seek(0, SeekOrigin.Begin);

		Stream unzippedEntryStream;

		var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
		foreach (var entry in archive.Entries)
		{
			if (entry.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
			{
				_ = int.TryParse(Regex.Match(entry.Name, @"\d+").Value, out int week);
				unzippedEntryStream = entry.Open();
				return result.WithData(new (unzippedEntryStream, week)).Build();
			}
		}

		return result.WithInvalidOperationException(scopeContext, null, $"{nameof(unzippedEntryStream)} == null");
	}

	public static IServiceCollection ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
	{
		Throw.IfArgumentNull(services);
		Throw.IfArgumentNull(configuration);

		ESBModelRegister.RegisterAdapter<SocPoistClientAdapter>(services, ADAPTER_ID);

		return services;
	}
}
