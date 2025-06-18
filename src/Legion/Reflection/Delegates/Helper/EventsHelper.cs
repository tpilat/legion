using System.Reflection;

namespace Legion.Reflection.Delegates.Helper;

internal static partial class EventsHelper
{
	public static readonly MethodInfo EventHandlerFactoryMethodInfo =
		typeof(EventsHelper).GetMethod("EventHandlerFactory")!;
}
