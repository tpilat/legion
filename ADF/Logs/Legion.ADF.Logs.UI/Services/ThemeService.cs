namespace Legion.ADF.Logs.UI.Services;

public class ThemeService
{
	public string CurrentTheme { get; set; } = "light";

	public void ToggleTheme()
	{
		CurrentTheme = CurrentTheme == "light" ? "dark" : "light";
	}
}

