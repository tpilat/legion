namespace Legion.Razor.UI.Services;

/// <summary>
/// Options for changing the theme.
/// </summary>
public class ThemeOptions
{
	/// <summary>
	/// Specifies the theme.
	/// </summary>
	public string Theme { get; set; }
	/// <summary>
	/// Specifies if the theme colors should meet WCAG contrast requirements.
	/// </summary>
	public bool? Wcag { get; set; }
	/// <summary>
	/// Specifies if the theme should be right-to-left.
	/// </summary>
	public bool? RightToLeft { get; set; }
	/// <summary>
	/// Specifies if the theme change should trigger the <see cref="ThemeService.ThemeChanged" /> event.
	/// </summary>
	public bool TriggerChange { get; set; }
}
