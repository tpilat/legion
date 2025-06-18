using Legion.ADF.Auth.Extensions;
using Legion.ADF.Auth.TestBlazorWebApp.Components;
using Legion.ADF.Auth.TestBlazorWebApp.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.TestBlazorWebApp;
public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents()
			.AddInteractiveWebAssemblyComponents();

		builder.Services.AddCascadingAuthenticationState();
		builder.Services.AddScoped<IdentityUserAccessor>();
		builder.Services.AddScoped<IdentityRedirectManager>();
		builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

		var authenticationBuilder = builder.Services.AddAuthentication(options =>
		{
			options.DefaultScheme = IdentityConstants.ApplicationScheme;
			options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
		})
		.AddFacebook(facebookOptions =>
		{
			facebookOptions.AppId = "2131938053828183";
			facebookOptions.AppSecret = "41d00c8576ccff2d654ebac360234dc0";
			//facebookOptions.CallbackPath = "/signin-facebook";
			facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
		});

		authenticationBuilder.AddApplicationCookie();
		authenticationBuilder.AddExternalCookie();
		authenticationBuilder.AddTwoFactorRememberMeCookie();
		authenticationBuilder.AddTwoFactorUserIdCookie();

		builder.Services.AddADFAuth(true)
			.ConfigurePostgreSQL();

		builder.Services.AddIdentityCore<Legion.ADF.Auth.Model.User>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddSignInManager()
			.AddDefaultTokenProviders();

		builder.Services.AddSingleton<IEmailSender<Legion.ADF.Auth.Model.User>, IdentityNoOpEmailSender>();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseWebAssemblyDebugging();
		}
		else
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseStaticFiles();
		app.UseAntiforgery();

		app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode()
			.AddInteractiveWebAssemblyRenderMode()
			.AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

		// Add additional endpoints required by the Identity /Account Razor components.
		app.MapAdditionalIdentityEndpoints();

		app.Run();
	}
}
