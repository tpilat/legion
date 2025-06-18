using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.Extensions;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationEntryScopeContext(this IServiceCollection services, Func<IServiceProvider, IScopeContext> factory)
	{
		Throw.IfArgumentNull(services);

		services.TryAddScoped(sp => new ApplicationEntryScopeContextStore((ScopeContext)factory(sp)));

		return services;
	}

	//public static IServiceCollection AddApplicationEntryScopeContext(
	//	this IServiceCollection services,
	//	string sourceSystemName,
	//	string? targetStoreId = null,
	//	string? businessProcess = null,
	//	string? component = null,
	//	Guid? tenantIdentifier = null,
	//	CultureInfo? cultureInfo = null,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//{
	//	Throw.IfArgumentNull(services);

	//	var scopeContext = ScopeContext.Create(
	//		sourceSystemName,
	//		removePreviousSameMethodFrame: true,
	//		previousScopeContext: null,
	//		correlationId: null,
	//		principal: null,
	//		idUser: null,
	//		targetStoreId,
	//		businessProcess,
	//		component,
	//		tenantIdentifier,
	//		externalCorrelationId: null,
	//		customCorrelationId: null,
	//		logger: null,
	//		cultureInfo,
	//		requestMetadata: null,
	//		cancellationToken: null,
	//		memberName,
	//		sourceFilePath,
	//		sourceLineNumber);

	//	services.TryAddScoped(sp => new ApplicationEntryScopeContextStore((ScopeContext)scopeContext));

	//	return services;
	//}
}
