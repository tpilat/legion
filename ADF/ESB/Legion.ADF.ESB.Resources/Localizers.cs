using Legion.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;

namespace Legion.ADF.ESB.Resources;

public static class Localizers
{
	private static IServiceProvider _serviceProvider;
	private static readonly ConcurrentDictionary<Type, IStringLocalizer> _stringLocalizers = new();

	internal static Dictionary<Type, Type> ResourceKeyLocalizerTypes { get; set; }

	public static void Configure(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
	}

	public static IStringLocalizer GetStringLocalizer<T>()
		=> GetStringLocalizer(typeof(T));

	public static IStringLocalizer GetStringLocalizer(Type keysType)
		=> _stringLocalizers.GetOrAdd(keysType, t =>
		{
			if (ResourceKeyLocalizerTypes == null)
				throw new InvalidOperationException($"Call {nameof(Extensions.ServiceCollectionExtensions)}.{nameof(Extensions.ServiceCollectionExtensions.AddLegionESBResources)} first.");

			if (_serviceProvider == null)
				throw new InvalidOperationException($"{nameof(Localizers)} was not configured.");

			return ((IResourceLocalizer)_serviceProvider.GetRequiredService(ResourceKeyLocalizerTypes[keysType])).Localizer;
		});
}

