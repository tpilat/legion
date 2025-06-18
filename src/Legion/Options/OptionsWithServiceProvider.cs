using Microsoft.Extensions.DependencyInjection;

namespace Legion.Options;

public class OptionsWithServiceProvider<TOptions> : Microsoft.Extensions.Options.IConfigureOptions<TOptions>
	where TOptions : class
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly Action<IServiceProvider, TOptions> _configure;

	public OptionsWithServiceProvider(
		IServiceScopeFactory serviceScopeFactory,
		Action<IServiceProvider, TOptions> configure)
	{
		Throw.IfArgumentNull(serviceScopeFactory);
		Throw.IfArgumentNull(configure);

		_serviceScopeFactory = serviceScopeFactory;
		_configure = configure;
	}

	public void Configure(TOptions options)
	{
		using var scope = _serviceScopeFactory.CreateScope();
		var scopedServiceProvider = scope.ServiceProvider;

		_configure.Invoke(scopedServiceProvider, options);
	}
}
