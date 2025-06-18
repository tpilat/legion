using System.Xml;

namespace Legion.EntityFrameworkCore.Expressions.MemberAccess;

public static class XmlNodeExtensions
{
	/// <exception cref="ArgumentException">
	/// Child element with name specified by <paramref name="childName"/> does not exists.
	/// </exception>
	public static string? ChildElementInnerText(this XmlNode node, string childName)
		=> node[childName]?.InnerText;
}