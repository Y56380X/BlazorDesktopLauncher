using System.Threading.Tasks;
using BlazorDesktopLauncher;
using BlazorDesktopLauncher.Extensions;

namespace SimpleApp
{
	public static class Program
	{
		public static async Task Main()
		{
			var application = new DesktopApplication<Component1>(
				appConfiguration: app => app.UseLocalBrowser());
			await application.RunAsync();
		}
	}
}