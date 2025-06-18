using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Legion.ADF.Logs.Web;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Define prefixes for each RCL assembly
		var prefixMap = new Dictionary<Assembly, string>
		{
			{ typeof(Legion.ADF.Logs.UI.Components.Layouts.MainLayout).Assembly, "libraryA" },
			//{ typeof(MyLibraryB.Pages.AnotherPage).Assembly, "libraryB" }
		};
		var rclAssembly = prefixMap.Keys.First();
		// Add services to the container.
		builder.Services.AddRazorPages()
			.AddApplicationPart(rclAssembly)  // Ensure the RCL is registered
			.AddRazorPagesOptions(options =>
			{
				// Add a custom convention to apply a prefix only to pages within the RCL
				options.Conventions.Add(new CustomPageRouteConvention());
			});

		builder.Services.AddServerSideBlazor(); // Ensure Blazor services are registered

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

		app.Run();
	}
}

// Custom convention to apply route prefixes based on the page assembly
public class RclPagePrefixConvention : IPageApplicationModelConvention
{
	private readonly Dictionary<Assembly, string> _prefixMap;

	public RclPagePrefixConvention(Dictionary<Assembly, string> prefixMap)
	{
		_prefixMap = prefixMap;
	}

	public void Apply(PageApplicationModel model)
	{
		// Determine the assembly of the page using HandlerType, ModelType, or PageType
		var pageAssembly = model.HandlerType?.Assembly ?? model.ModelType?.Assembly ?? model.PageType?.Assembly;

		if (pageAssembly != null && _prefixMap.TryGetValue(pageAssembly, out var prefix))
		{
			// Register a new route with the prefix for this page
			var originalRoute = model.ActionDescriptor.ViewEnginePath.TrimStart('/');
			var prefixedRoute = $"/{prefix}/{originalRoute}";

			// Use AddPageRoute to add the prefixed route explicitly
			model.ActionDescriptor.ViewEnginePath = prefixedRoute;
			model.ActionDescriptor.RouteValues["page"] = prefixedRoute;
		}
	}
}


//// Custom convention to apply a prefix only to pages within a specific RCL assembly
//public class CustomRclRouteConvention : IPageRouteModelConvention
//{
//	private readonly Assembly _targetAssembly;
//	private readonly string _prefix;

//	public CustomRclRouteConvention(Assembly targetAssembly, string prefix)
//	{
//		_targetAssembly = targetAssembly;
//		_prefix = prefix;
//	}

//	public void Apply(PageRouteModel model)
//	{
//		// Check if the page's assembly matches the target RCL assembly
//		if (model.ActionDescriptor is PageActionDescriptor descriptor &&
//			descriptor.AssemblyName == _targetAssembly.GetName().Name)
//		{
//			foreach (var selector in model.Selectors)
//			{
//				if (selector.AttributeRouteModel != null)
//				{
//					// Prepend the prefix to the route template
//					selector.AttributeRouteModel.Template = $"{_prefix}/{selector.AttributeRouteModel.Template}";
//				}
//			}
//		}
//	}
//}

public class CustomPageRouteConvention : IPageRouteModelConvention
{
	public void Apply(PageRouteModel model)
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