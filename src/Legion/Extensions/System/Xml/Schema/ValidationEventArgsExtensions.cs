using Legion.Logging;
using System.Xml.Schema;

namespace Legion.Extensions;

public static  class ValidationEventArgsExtensions
{
	public static IErrorMessage ToErrorMessage(this ValidationEventArgs arg, IScopeContext scopeContext, IErrorCode errorCode)
		=> LogMessage.CreateErrorMessage(scopeContext, errorCode, x => x.ExceptionInfo(arg.Exception).Detail(arg.Message));

	public static ILogMessage ToLogMessage(this ValidationEventArgs arg, IScopeContext scopeContext, IErrorCode errorCode)
		=> arg.Severity == XmlSeverityType.Error
		? LogMessage.CreateErrorMessage(scopeContext, errorCode, x => x.ExceptionInfo(arg.Exception).Detail(arg.Message))
		: LogMessage.CreateWarningMessage(scopeContext, errorCode, x => x.ExceptionInfo(arg.Exception).Detail(arg.Message));
}
