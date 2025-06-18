using Legion.Extensions;
using Legion.MessageBus.MessageResolvers;
using System.Reflection;

namespace Legion.MessageBus.MessageResolvers.Internal;

public class FullNameTypeResolver : IMessageTypeResolver
{
	public string ToName(Type type)
		=> type?.FullName ?? type?.ToFriendlyFullName() ?? throw new ArgumentNullException(nameof(type));

	public Type ToType(string fullName)
	{
		if (string.IsNullOrWhiteSpace(fullName))
			throw new ArgumentNullException(nameof(fullName));

		var referencedAssemblies = Assembly.GetEntryAssembly()?
			.GetReferencedAssemblies()
			.Select(a => a.FullName);

		return referencedAssemblies == null
			? throw new InvalidOperationException("No EntryAssembly.")
			: AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => referencedAssemblies.Contains(a.FullName))
				.SelectMany(a => a.GetTypes().Where(x => x.FullName == fullName || x.ToFriendlyFullName() == fullName))
				.FirstOrDefault()
					?? throw new InvalidOperationException($"{nameof(Type)}.{nameof(Type.FullName)} {fullName} cannot be resolved to any type.");
	}
}
