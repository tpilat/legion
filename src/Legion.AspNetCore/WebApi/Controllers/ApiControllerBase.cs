using Legion.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Legion.AspNetCore.WebApi;

public abstract class ApiControllerBase : ControllerBase
{
	protected TService GetRequiredService<TService>()
		where TService : notnull
		=> HttpContext.RequestServices.GetRequiredService<TService>();

	protected TService? GetService<TService>()
		=> HttpContext.RequestServices.GetService<TService>();

	protected IScopeContext GetNewScopeContext(
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> HttpContext.RequestServices
			.GetRequiredApplicationEntryScopeContextClone()
			.CreateNew(removePreviousSameMethodFrame: true, memberName, sourceFilePath, sourceLineNumber);
}
