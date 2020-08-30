using System.Threading.Tasks;
using BlazorDesktopLauncher;
using BlazorDesktopLauncher.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleApp
{
	public static class Program
	{
		public static async Task Main()
		{
			var application = new DesktopApplication<Component1>(services =>
			{
				services.AddSingleton<ExampleJsInterop>();
				services.AddJsLoader("exampleJsInterop.js");
			}, blazorDesktop => blazorDesktop.UseLocalBrowser());
			
			await application.RunAsync();
		}
	}
}