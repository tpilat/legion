namespace Legion.Razor.UI.Services;

/// <summary>
/// Options for the <see cref="QueryStringThemeService" />.
/// </summary>
public class QueryStringThemeServiceOptions
{
	/// <summary>
	/// Gets or sets the query string parameter for the theme.
	/// </summary>
	public string ThemeParameter { get; set; } = "theme";
	/// <summary>
	/// Gets or sets the query string parameter for the wcag compatible color theme.
	/// </summary>
	public string WcagParameter { get; set; } = "wcag";

	/// <summary>
	/// Gets or sets the query string parameter for the right to left theme.
	/// </summary>
	public string RightToLeftParameter { get; set; } = "rtl";
}
