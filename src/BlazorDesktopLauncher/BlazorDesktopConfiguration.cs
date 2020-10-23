using Microsoft.Extensions.Configuration;

namespace BlazorDesktopLauncher
{
	internal class BlazorDesktopConfiguration : IBlazorDesktopConfiguration
	{
		public IConfigurationProvider Build(IConfigurationBuilder builder) => 
			new BlazorDesktopConfigurationProvider(this);

		string? IBlazorDesktopConfiguration.BrowserExecutablePath { get; set; }

		string? IBlazorDesktopConfiguration.WindowTitle { get; set; } = "Blazor Desktop Launcher";
	}
}