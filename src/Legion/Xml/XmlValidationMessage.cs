using System.Xml.Schema;

namespace Legion.Xml;

public class XmlValidationMessage
{
	public string Message { get; set; }

	public XmlSeverityType Severity { get; set; }

	public XmlSchemaException Exception { get; set; }

	public ValidationEventArgs? EventArgs { get; set; }

	public XmlValidationMessage()
	{
	}

	public XmlValidationMessage(ValidationEventArgs eventArgs)
	{
		Throw.IfArgumentNull(eventArgs);

		EventArgs = eventArgs;
		Message = eventArgs.Message;
		Severity = eventArgs.Severity;
		Exception = eventArgs.Exception;
	}
}
