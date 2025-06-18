using Legion.Http;
using Legion.Policy;
using Legion.Reflection.ObjectPaths;
using Legion.Security.Cryptography;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Legion.NetHttp;

public abstract class HttpApiClientOptions
{
	private Type? _defaultRequestResponseLoggerType;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public string ClientName { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public string SourceSystemName { get; set; } = nameof(HttpApiClient);
	public string? BaseAddress { get; set; }
	public string? UserAgent { get; set; } = nameof(HttpApiClient);
	public Version? Version { get; set; }
	public int? DefaultTimeoutInSeconds { get; set; }
	public bool LogRequest { get; set; }
	public bool LogRequestPayload { get; set; }
	public bool LogResponse { get; set; }
	public bool LogResponsePayload { get; set; }
	public List<string>? LogDisabledUris { get; set; }
	public Dictionary<string, string>? StaticQueryStrings { get; set; }
	public bool ForceStaticQueryStrings { get; set; }
	public List<ForceableKeyValuePair>? StaticHeaders { get; set; }
	public List<ForceableKeyValuePairList>? StaticHeaderCollections { get; set; }
	public List<ForceableKeyValuePair>? StaticCookies { get; set; }
	public List<ForceableKeyValuePair>? StaticFormData { get; set; }

	#region HttpClientHandler

	public DecompressionMethods? AutomaticDecompression { get; set; } = DecompressionMethods.GZip;
	public WebProxySettings? WebProxySettings { get; set; }
	public ICredentials? DefaultProxyCredentials { get; set; }
	public bool TrustToAllServerCertificates { get; set; }
	public bool? CheckCertificateRevocationList { get; set; }
	public bool UsesCookieContainerToStoreServerCookies { get; set; }
	public bool? UseDefaultCredentials { get; set; }
	public CredentialCache? CredentialCache { get; set; }
	public ICredentials? Credentials { get; set; }
	public List<X509Certificate>? ClientCertificates { get; set; }
	public bool SendAuthorizationHeaderInRequest { get; set; }
	public SslProtocols? SslProtocols { get; set; }
	public int? MaxResponseHeadersLength { get; set; }
	public long? MaxRequestContentBufferSize { get; set; }
	public int? MaxConnectionsPerServer { get; set; }
	public int? MaxAutomaticRedirections { get; set; }
	public bool? AllowAutoRedirect { get; set; }

	#endregion HttpClientHandler

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public Dictionary<string, IAsyncPolicy<HttpResponseMessage>>? UriPolicies { get; set; } //Dictionary<Uri, IAsyncPolicy<HttpResponseMessage>> OR ----- Wildcard ----- Dictionary<*, IAsyncPolicy<HttpResponseMessage>>

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public Dictionary<string, IRequestResponseLogger>? UriLoggers { get; set; } //Dictionary<Uri, IRequestResponseLogger> OR ----- Wildcard ----- Dictionary<*, IRequestResponseLogger>

	//Func<object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors, bool)>
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool>? RemoteCertificateValidationCallback { get; set; }
		= DefaultServerCertificateValidation.ServerCertificateValidation;

	public void SetDefaultRequestResponseLogger<T, TCorrelation>()
		where T : IRequestResponseLogger<TCorrelation>
	{
		_defaultRequestResponseLoggerType = typeof(T);
	}

	public bool ApplyToHttpClientHandler => 
		AutomaticDecompression.HasValue
		|| WebProxySettings != null
		|| DefaultProxyCredentials != null
		|| TrustToAllServerCertificates
		|| CheckCertificateRevocationList.HasValue
		|| !UsesCookieContainerToStoreServerCookies
		|| UseDefaultCredentials.HasValue
		|| CredentialCache != null
		|| Credentials != null
		|| 0 < ClientCertificates?.Count
		|| SendAuthorizationHeaderInRequest
		|| SslProtocols.HasValue
		|| MaxResponseHeadersLength.HasValue
		|| MaxRequestContentBufferSize.HasValue
		|| MaxConnectionsPerServer.HasValue
		|| MaxAutomaticRedirections.HasValue
		|| AllowAutoRedirect.HasValue
		;

	public void ConfigureHttpClientHandler(HttpClientHandler handler)
	{
		Throw.IfArgumentNull(handler);

		var isBrowser = Legion.Infrastructure.OSPlatformHelper.IsBrowser();

		if (!isBrowser)
		{
			if (AutomaticDecompression.HasValue)
				handler.AutomaticDecompression = AutomaticDecompression.Value;

			if (WebProxySettings != null)
			{
				if (WebProxySettings != null)
				{
					NetworkCredential? networkCredential = null;
					if (!string.IsNullOrWhiteSpace(WebProxySettings.UserName))
					{
						if (string.IsNullOrWhiteSpace(WebProxySettings.DomainName))
						{
							networkCredential = new NetworkCredential(
								userName: WebProxySettings.UserName,
								password: WebProxySettings.Password);
						}
						else
						{
							networkCredential = new NetworkCredential(
								userName: WebProxySettings.UserName,
								password: WebProxySettings.Password,
								domain: WebProxySettings.DomainName);
						}
					}

					handler.Proxy =
						new WebProxy
						{
							Address = new Uri(WebProxySettings.Address),
							BypassProxyOnLocal = WebProxySettings.BypassOnLocal,
							UseDefaultCredentials = WebProxySettings.UseDefaultCredentials,
							Credentials = networkCredential
						};

					handler.UseProxy = WebProxySettings.UseProxy;
				}

			}

			if (DefaultProxyCredentials != null)
				handler.DefaultProxyCredentials = DefaultProxyCredentials;

			if (TrustToAllServerCertificates && RemoteCertificateValidationCallback != null)
				handler.ServerCertificateCustomValidationCallback = RemoteCertificateValidationCallback!;

			if (CheckCertificateRevocationList.HasValue)
				handler.CheckCertificateRevocationList = CheckCertificateRevocationList.Value;

			if (!UsesCookieContainerToStoreServerCookies)
				handler.UseCookies = false;

			if (UseDefaultCredentials.HasValue)
				handler.UseDefaultCredentials = UseDefaultCredentials.Value;

			if (CredentialCache != null)
				handler.Credentials = CredentialCache;
			else if (Credentials != null)
				handler.Credentials = Credentials;

			if (0 < ClientCertificates?.Count)
				handler.ClientCertificates.AddRange(ClientCertificates.ToArray());

			if (SendAuthorizationHeaderInRequest)
				handler.PreAuthenticate = true;

			if (SslProtocols.HasValue)
				handler.SslProtocols = SslProtocols.Value;

			if (MaxResponseHeadersLength.HasValue)
				handler.MaxResponseHeadersLength = MaxResponseHeadersLength.Value;

			if (MaxConnectionsPerServer.HasValue)
				handler.MaxConnectionsPerServer = MaxConnectionsPerServer.Value;

			if (MaxAutomaticRedirections.HasValue)
				handler.MaxAutomaticRedirections = MaxAutomaticRedirections.Value;
		}

		if (MaxRequestContentBufferSize.HasValue)
			handler.MaxRequestContentBufferSize = MaxRequestContentBufferSize.Value;

		if (AllowAutoRedirect.HasValue)
			handler.AllowAutoRedirect = AllowAutoRedirect.Value;
	}

	public void ConfigureStaticRequestParams(IHttpClientRequest request)
	{
		Throw.IfArgumentNull(request);

		var builder = new RequestBuilder(request);

		if (StaticQueryStrings != null)
		{
			var dict = new Dictionary<string, string>();
			foreach (var staticQueryString in StaticQueryStrings)
				if (!string.IsNullOrWhiteSpace(staticQueryString.Key))
					dict[staticQueryString.Key] = staticQueryString.Value;

			if (0 < dict.Count)
				builder.QueryString(dict, ForceStaticQueryStrings);
		}

		if (StaticHeaders != null)
		{
			foreach (var staticHeader in StaticHeaders)
			{
				if (string.IsNullOrWhiteSpace(staticHeader.Key))
					continue;

				request.Headers.AddHeader(staticHeader.Key, staticHeader.Value, staticHeader.Force);
			}
		}

		if (StaticHeaderCollections != null)
		{
			foreach (var staticHeaderCollection in StaticHeaderCollections)
			{
				if (string.IsNullOrWhiteSpace(staticHeaderCollection.Key))
					continue;

				request.Headers.AddHeader(staticHeaderCollection.Key, staticHeaderCollection.Values, staticHeaderCollection.Force);
			}
		}

		if (StaticCookies != null)
		{
			foreach (var staticCookie in StaticCookies)
			{
				if (string.IsNullOrWhiteSpace(staticCookie.Key))
					continue;

				request.Headers.AddCookie(staticCookie.Key, staticCookie.Value, staticCookie.Force);
			}
		}

		if (StaticFormData != null)
		{
			foreach (var staticFormData in StaticFormData)
			{
				if (string.IsNullOrWhiteSpace(staticFormData.Key))
					continue;

				builder.AddFormData(new KeyValuePair<string, string>(staticFormData.Key, staticFormData.Value), staticFormData.Force);
			}
		}
	}

	public void AddCredentialCache(string host, int port, AuthenticationType authenticationType, NetworkCredential credential)
	{
		Throw.IfArgumentNullOrWhiteSpace(host);
		Throw.IfArgumentNull(credential);

		CredentialCache ??= new();
		CredentialCache.Add(host, port, authenticationType.ToString(), credential);
	}

	public void AddCredentialCache(string uriPrefix, AuthenticationType authenticationType, NetworkCredential credential)
	{
		Throw.IfArgumentNullOrWhiteSpace(uriPrefix);
		Throw.IfArgumentNull(credential);

		CredentialCache ??= new CredentialCache();
		CredentialCache.Add(new Uri(uriPrefix), authenticationType.ToString(), credential);
	}

	public virtual IRequestResponseLogger<TCorrelation>? GetLogger<TCorrelation>(string? uri, IServiceProvider? serviceProvider = null)
	{
		if (serviceProvider != null)
		{
			if (_defaultRequestResponseLoggerType != null)
			{
				var requestResponseLogger = serviceProvider.GetService(_defaultRequestResponseLoggerType);
				if (requestResponseLogger != null)
					return (IRequestResponseLogger<TCorrelation>)requestResponseLogger;
			}
			else
			{
				var requestResponseLogger = serviceProvider.GetService<IRequestResponseLogger<TCorrelation>>();
				if (requestResponseLogger != null)
					return requestResponseLogger;
			}
		}

		if (string.IsNullOrWhiteSpace(uri))
			return null;

		if (LogDisabledUris != null && LogDisabledUris.Any(x => uri!.StartsWith(x)))
			return null;

		if (UriLoggers == null || UriLoggers.Count == 0)
			return null;

		var key = UriLoggers.Keys.FirstOrDefault(x => uri!.StartsWith(x));
		if (!string.IsNullOrWhiteSpace(key) && UriLoggers.TryGetValue(key, out var logger))
			return logger as IRequestResponseLogger<TCorrelation>;

		if (UriLoggers.TryGetValue("*", out var defaultLogger))
			return defaultLogger as IRequestResponseLogger<TCorrelation>;

		return null;
	}

	public class Validator<T> : ValidatorBase<T>
		where T : HttpApiClientOptions
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<T> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<T> builder)
		{
			builder?
				.ForProperty(x => x.ClientName, v => v.NotDefaultOrWhiteSpace())
				.ForEach(
					x => x.StaticHeaders,
					ForceableKeyValuePair.Validator.RulesBuilder)
				.ForEach(
					x => x.StaticHeaderCollections,
					ForceableKeyValuePairList.Validator.RulesBuilder)
				.ForEach(
					x => x.StaticCookies,
					ForceableKeyValuePair.Validator.RulesBuilder)
				.ForEach(
					x => x.StaticFormData,
					ForceableKeyValuePair.Validator.RulesBuilder)
				.ForNavigation(
						x => x.WebProxySettings,
						WebProxySettings.Validator.RulesBuilder)
			;
		}
	}
}
