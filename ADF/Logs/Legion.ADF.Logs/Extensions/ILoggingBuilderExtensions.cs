using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Logs.Extensions;

public static class ILoggingBuilderExtensions
{
	public static ILoggingBuilder AddProvider<T>(this ILoggingBuilder builder)
		where T : class, ILoggerProvider
	{
		builder.Services.TryAddSingleton<ILoggerProvider, T>();
		return builder;
	}

	public static ILoggingBuilder AddProvider<T>(this ILoggingBuilder builder, Func<IServiceProvider, T> factory)
	where T : class, ILoggerProvider
	{
		builder.Services.AddSingleton<ILoggerProvider, T>(factory);
		return builder;
	}
}
