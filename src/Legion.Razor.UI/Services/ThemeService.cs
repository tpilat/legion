namespace Legion.Razor.UI.Services;

/// <summary>
/// Service for theme registration and management.
/// </summary>
public class ThemeService
{
	/// <summary>
	/// Gets the current theme.
	/// </summary>
	public string Theme { get; private set; }

	/// <summary>
	/// Specify if the theme colors should meet WCAG contrast requirements.
	/// </summary>
	public bool? Wcag { get; private set; }

	/// <summary>
	/// Specify if the theme should be right-to-left.
	/// </summary>
	public bool? RightToLeft { get; private set; }

	/// <summary>
	/// Raised when the theme changes.
	/// </summary>
	public event Action ThemeChanged;

	/// <summary>
	/// Changes the current theme.
	/// </summary>
	public void SetTheme(string theme, bool triggerChange = true)
	{
		SetTheme(new ThemeOptions
		{
			Theme = theme,
			Wcag = Wcag,
			RightToLeft = RightToLeft,
			TriggerChange = triggerChange
		});
	}

	/// <summary>
	/// Changes the current theme with additional options.
	/// </summary>
	public void SetTheme(ThemeOptions options)
	{
		var requiresChange = false;

		if (Theme != options.Theme)
		{
			Theme = options.Theme;
			requiresChange = true;
		}

		if (Wcag != options.Wcag)
		{
			Wcag = options.Wcag;
			requiresChange = true;
		}

		if (RightToLeft != options.RightToLeft)
		{
			RightToLeft = options.RightToLeft;
			requiresChange = true;
		}

		if (requiresChange && options.TriggerChange)
		{
			ThemeChanged?.Invoke();
		}
	}

	/// <summary>
	/// Enables or disables WCAG contrast requirements.
	/// </summary>
	public void SetWcag(bool wcag)
	{
		SetTheme(new ThemeOptions
		{
			Theme = Theme,
			Wcag = wcag,
			RightToLeft = RightToLeft,
			TriggerChange = true
		});
	}

	/// <summary>
	/// Specifies if the theme should be right-to-left.
	/// </summary>
	public void SetRightToLeft(bool rightToLeft)
	{
		SetTheme(new ThemeOptions
		{
			Theme = Theme,
			Wcag = Wcag,
			RightToLeft = rightToLeft,
			TriggerChange = true
		});
	}
}
