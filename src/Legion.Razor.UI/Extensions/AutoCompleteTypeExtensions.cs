using System.Text.RegularExpressions;

namespace Legion.Razor.UI;

public static class AutoCompleteTypeExtensions
{
	/// <summary>
	/// Converts the autocomplete type enum value to the expected
	/// autocomplete attribute value.
	/// </summary>
	/// <returns>The autocomplete attribute string value.</returns>
	public static string GetAutoCompleteValue(this AutoCompleteType typeValue)
	{
		//// Handle synonyms.
		//switch (typeValue)
		//{
		//	case AutoCompleteType.FirstName:
		//		return "given-name";
		//	case AutoCompleteType.LastName:
		//		return "family-name";
		//	case AutoCompleteType.MiddleName:
		//		return "additional-name";
		//	case AutoCompleteType.ZipCode:
		//		return "postal-code";
		//	case AutoCompleteType.Province:
		//		return "address-level1";
		//	case AutoCompleteType.State:
		//		return "address-level1";
		//}

		// Handle standard values.
		var value = typeValue.ToString();
		value = Regex.Replace(value, "([^A-Z])([A-Z])", "$1-$2");
		return Regex.Replace(value, "([A-Z]+)([A-Z][^A-Z$])", "$1-$2")
			.Trim().ToLower();
	}
}
