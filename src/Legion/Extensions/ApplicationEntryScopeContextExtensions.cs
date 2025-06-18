using Legion.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Legion.Extensions;

public static class ApplicationEntryScopeContextExtensions
{
	//public static IApplicationEntryScopeContext? GetApplicationEntryScopeContext(this IServiceProvider serviceProvider)
	//{
	//	Throw.IfArgumentNull(serviceProvider);

	//	var store = serviceProvider.GetRequiredService<ApplicationEntryScopeContextStore>();
	//	if (store == null)
	//		return null;

	//	return store.ApplicationEntryScopeContext;
	//}

	public static IApplicationEntryScopeContext? GetApplicationEntryScopeContextClone(this IServiceProvider serviceProvider)
	{
		Throw.IfArgumentNull(serviceProvider);

		var store = serviceProvider.GetService<ApplicationEntryScopeContextStore>();
		if (store == null)
			return null;

		var clone = store.GetApplicationEntryScopeContextClone();
		return clone;
	}

	public static void AddApplicationEntryScopeContextPrincipal(
		this IServiceProvider serviceProvider,
		LegionPrincipal principal,
		bool force,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		Throw.IfArgumentNull(serviceProvider);

		var store = serviceProvider.GetRequiredService<ApplicationEntryScopeContextStore>();

		store.ApplicationEntryScopeContext
			.AppendTraceFrameWithPrincipal(
				principal,
				force,
				true,
				memberName,
				sourceFilePath,
				sourceLineNumber);
	}

	public static void AddApplicationEntryScopeContextRequestMetadata(
		this IServiceProvider serviceProvider,
		Web.IRequestMetadata requestMetadata,
		bool force,
		string? externalCorrelationId = null,
		Guid? idApplicationEntry = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		Throw.IfArgumentNull(serviceProvider);

		var store = serviceProvider.GetRequiredService<ApplicationEntryScopeContextStore>();
		if (store == null)
			return;

		store.ApplicationEntryScopeContext
			.AppendTraceFrameWithRequestMetadata(
				requestMetadata,
				force,
				true,
				memberName,
				sourceFilePath,
				sourceLineNumber);

		if (externalCorrelationId != null)
			((ScopeContext)store.ApplicationEntryScopeContext)
				.SetExternalCorrelationId(externalCorrelationId, force);

		if (idApplicationEntry.HasValue)
			((ScopeContext)store.ApplicationEntryScopeContext)
				.SetIdApplicationEntry(idApplicationEntry, force);
	}
}
