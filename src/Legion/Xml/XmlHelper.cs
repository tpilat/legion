using System.Xml;
using System.Xml.Schema;

namespace Legion.Xml;

public static class XmlHelper
{
	public static string? GetRootXmlns(Stream xmlStream)
	{
		Throw.IfArgumentNull(xmlStream);

		try
		{
			if (xmlStream.CanSeek == true)
				xmlStream.Seek(0, SeekOrigin.Begin);

			XmlDocument doc = new();
			doc.Load(xmlStream);
			var xmlRootNamespace = doc.DocumentElement?.NamespaceURI;
			return xmlRootNamespace;
		}
		finally
		{
			if (xmlStream.CanSeek == true)
			{
				try
				{
					xmlStream.Seek(0, SeekOrigin.Begin);
				}
				catch { }
			}
		}
	}

	public static string? GetRootElementName(Stream xmlStream)
	{
		Throw.IfArgumentNull(xmlStream);

		try
		{
			if (xmlStream.CanSeek == true)
				xmlStream.Seek(0, SeekOrigin.Begin);

			XmlDocument doc = new();
			doc.Load(xmlStream);
			var rootElementName = doc.DocumentElement?.LocalName;
			return rootElementName;
		}
		finally
		{
			if (xmlStream.CanSeek == true)
			{
				try
				{
					xmlStream.Seek(0, SeekOrigin.Begin);
				}
				catch { }
			}
		}
	}

	public static XmlValidationMessage[]? ValidateXml(Stream xmlStream, Stream[] xsdStreams, bool checkXmlNamespace = true)
	{
		Throw.IfArgumentNull(xmlStream);

		Throw.IfArgumentNullOrEmpty(xsdStreams);

		var result = new List<XmlValidationMessage>();

		try
		{
			if (xmlStream.CanSeek == true)
				xmlStream.Seek(0, SeekOrigin.Begin);

			var settings = new XmlReaderSettings
			{
				ValidationType = ValidationType.Schema
			};
			settings.ValidationEventHandler += (sender, validationEventArgs) => result.Add(new XmlValidationMessage(validationEventArgs));

			string? schemaTargetNamespace = null;

			var i = 0;
			foreach (var xsdStream in xsdStreams)
			{
				if (xsdStream.CanSeek == true)
					xsdStream.Seek(0, SeekOrigin.Begin);

				var schema = XmlSchema.Read(xsdStream, (sender, validationEventArgs) => result.Add(new XmlValidationMessage(validationEventArgs)));

				Throw.IfNull(schema);

				var isValid = result.Count == 0;

				if (isValid)
				{
					settings.Schemas.Add(schema);
					schemaTargetNamespace = schema.TargetNamespace;
				}
				else
					throw new XmlSchemaReadException(result.Select(xvm => xvm.EventArgs).ToList()!, $"{nameof(xsdStreams)}[{i}]");

				i++;
			}

			if (checkXmlNamespace)
			{
				XmlDocument doc = new();
				doc.Load(xmlStream);
				var xmlRootNamespace = doc.DocumentElement?.NamespaceURI;

				if (!string.Equals(schemaTargetNamespace, xmlRootNamespace, StringComparison.InvariantCultureIgnoreCase))
					result.Add(new XmlValidationMessage { Message = $"Invalid XML root namespace = {xmlRootNamespace} | XSD target namespace is = {schemaTargetNamespace}" });

				xmlStream.Seek(0, SeekOrigin.Begin);
			}

			using var xmlFile = XmlReader.Create(xmlStream, settings);

			while (xmlFile.Read()) ;

			return 0 < result.Count
				? [.. result]
				: null; //no error, no warning
		}
		catch (Exception ex)
		{
			result.Add(new XmlValidationMessage { Message = ex.ToString() });
			return result.ToArray();
		}
		finally
		{
			if (xmlStream.CanSeek == true)
			{
				try
				{
					xmlStream.Seek(0, SeekOrigin.Begin);
				}
				catch { }
			}

			if (0 < xsdStreams?.Length)
			{
				foreach (var xsdStream in xsdStreams)
				{
					if (xsdStream.CanSeek == true)
					{
						try
						{
							xsdStream.Seek(0, SeekOrigin.Begin);
						}
						catch { }
					}
				}
			}
		}
	}
}
