using System.Collections;
using System.Reflection;

namespace Legion.Exceptions;

public interface ILegionException
{
	/// <summary>
	/// Gets a string representation of the immediate frames on the call stack.
	/// </summary>
	string? StackTrace { get; }

	/// <summary>
	/// Gets or sets the name of the application or the object that causes the error.
	/// </summary>
	string? Source { get; set; }

	/// <summary>
	/// Gets a message that describes the current exception.
	/// </summary>
	string Message { get; }

	/// <summary>
	/// Gets a message detail that describes the current exception.
	/// </summary>
	string? Detail { get; }

	/// <summary>
	/// Gets the <see cref="System.Exception"/> instance that caused the current exception.
	/// </summary>
	Exception? InnerException { get; }

	/// <summary>
	/// Gets or sets HRESULT, a coded numerical value that is assigned to a specific exception.
	/// </summary>
	int HResult { get; }

	/// <summary>
	/// Gets a collection of key/value pairs that provide additional user-defined information about the exception.
	/// </summary>
	IDictionary Data { get; }

	/// <summary>
	/// Gets the method that throws the current exception.
	/// </summary>
	MethodBase? TargetSite { get; }

	/// <summary>
	/// Gets or sets a link to the help file associated with this exception.
	/// </summary>
	string? HelpLink { get;}

	IErrorCode? ErrorCode { get; }
	
	IScopeContext? ScopeContext { get; }
}
