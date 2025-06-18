namespace Legion.ADF.Logs.Web2;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services
			.AddRazorPages(options =>
			{
				options.Conventions.Add(new CustomPageRouteConvention());
			});

		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents()
			.AddCircuitOptions(opt =>
			{
				opt.DetailedErrors = true;
			});

		//builder.Services.AddServerSideBlazor(); // Adds Blazor Server support

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthorization();

		app.MapRazorPages();
		//app.MapRazorComponents<App>.AddInteractiveServerRenderMode();

		app.MapBlazorHub(); // Enables Blazor routing and components
		
		app.MapFallbackToPage("/_Host"); // Fallback for Blazor routes

		app.Run();
	}
}


public class CustomPageRouteConvention : Microsoft.AspNetCore.Mvc.ApplicationModels.IPageRouteModelConvention
{
	public void Apply(Microsoft.AspNetCore.Mvc.ApplicationModels.PageRouteModel model)
	{
		//if (model.ViewEnginePath == "/Pages/MyPage")
		//{
		//	// Modify the route template for this specific page
		//	model.Selectors.Clear(); // Clear existing routes if necessary
		//	model.Selectors.Add(new SelectorModel
		//	{
		//		AttributeRouteModel = new AttributeRouteModel
		//		{
		//			Template = "custom-prefix/my-page"
		//		}
		//	});
		//}
	}
}
