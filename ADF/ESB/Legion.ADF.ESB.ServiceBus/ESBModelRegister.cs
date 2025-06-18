using Legion.ADF.ESB.Components;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Concurrent;

namespace Legion.ADF.ESB.ServiceBus;

public static class ESBModelRegister
{
	private static readonly Type IESBAdapterType = typeof(IESBAdapter);
	private static readonly ConcurrentDictionary<Guid, Type> _esbAdapters = new();

	public static void RegisterAdapter<TAdapter>(IServiceCollection services, Guid idAdapter)
		where TAdapter : class, IESBAdapter
	{
		Throw.IfArgumentNull(services);

		RegisterAdapter(idAdapter, typeof(TAdapter));

		services.TryAddTransient<TAdapter>();
	}

	private static void RegisterAdapter(Guid idAdapter, Type adapterType)
	{
		Throw.IfArgumentNull(adapterType);

		//if (adapterType.GetInterfaces()?.Contains(IESBAdapterType) != true)
		//	Throw.InvalidOperationException((IErrorCode)null);

		var added = _esbAdapters.TryAdd(idAdapter, adapterType);

		if (!added)
			Throw.InvalidOperationException(
				Legion.ADF.ESB.Exceptions.Internal.ErrorCodes.ESBRegistrationException.MultipleAdapterRegistration(idAdapter, _esbAdapters[idAdapter].ToFriendlyFullName(), adapterType.ToFriendlyFullName()));
	}

	public static List<IESBAdapter> GetAllAdapters(IServiceProvider serviceProvider)
	{
		Throw.IfArgumentNull(serviceProvider);

		var result = _esbAdapters.Values.Select(x => (IESBAdapter)serviceProvider.GetRequiredService(x)).ToList();
		return result;
	}
}
