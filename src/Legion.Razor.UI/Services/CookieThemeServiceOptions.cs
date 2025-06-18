namespace Legion.Razor.UI.Services;

/// <summary>
/// Options for the <see cref="CookieThemeService" />.
/// </summary>
public class CookieThemeServiceOptions
{
	/// <summary>
	/// Gets or sets the cookie name.
	/// </summary>
	public string Name { get; set; } = "Theme";

	/// <summary>
	/// Gets or sets the cookie duration.
	/// </summary>
	public TimeSpan Duration { get; set; } = TimeSpan.FromDays(365);
}
