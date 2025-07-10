using Legion.AspNetCore.WebApi.Configurations;
using Legion.AspNetCore.WebApi.Conversions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Legion.AspNetCore.Configurations;

public class WebApiMvcOptions : IConfigureOptions<MvcOptions>
{
	private readonly WebApiOptions _webApiOptions;

	public WebApiMvcOptions(IOptions<WebApiOptions> webApiOptions)
	{
		Throw.IfArgumentNull(webApiOptions);
		_webApiOptions = webApiOptions.Value;
	}

	public void Configure(MvcOptions options)
	{
		if (!string.IsNullOrWhiteSpace(_webApiOptions.ApiPrefix))
			options.Conventions.Add(new MultiVersionRoutePrefixConvention(_webApiOptions.ApiPrefix));
	}
}

