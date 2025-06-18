using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Legion.Razor.UI.Services;

/// <summary>
/// Persist the current theme in a cookie. Requires <see cref="ThemeService" /> to be registered in the DI container.
/// </summary>
public class CookieThemeService
{
	private readonly CookieThemeServiceOptions options;
	private readonly IJSRuntime jsRuntime;
	private readonly ThemeService themeService;

	/// <summary>
	/// Initializes a new instance of the <see cref="CookieThemeService" /> class.
	/// </summary>
	public CookieThemeService(IJSRuntime jsRuntime, ThemeService themeService, IOptions<CookieThemeServiceOptions> options)
	{
		this.jsRuntime = jsRuntime;
		this.themeService = themeService;
		this.options = options.Value;

		themeService.ThemeChanged += OnThemeChanged;

		_ = InitializeAsync();
	}

	private async Task InitializeAsync()
	{
		try
		{
			var cookies = await jsRuntime.InvokeAsync<string>("eval", "document.cookie");

			var themeCookie = cookies?.Split("; ").Select(x =>
			{
				var parts = x.Split("=");

				return (Key: parts[0], Value: parts[1]);
			})
			.FirstOrDefault(x => x.Key == options.Name);

			var theme = themeCookie?.Value;

			if (!string.IsNullOrEmpty(theme) && themeService.Theme != theme)
			{
				themeService.SetTheme(theme);
			}
		}
		catch (InvalidOperationException)
		{
		}
	}

	private void OnThemeChanged()
	{
		var expiration = DateTime.Now.Add(options.Duration);

		_ = jsRuntime.InvokeVoidAsync("eval", $"document.cookie = \"{options.Name}={themeService.Theme}; expires={expiration:R}; path=/\"");
	}
}

