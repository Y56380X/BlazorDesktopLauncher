using Microsoft.Extensions.Configuration;

namespace BlazorDesktopLauncher
{
	internal class BlazorDesktopConfiguration : IBlazorDesktopConfiguration
	{
		public IConfigurationProvider Build(IConfigurationBuilder builder) => 
			new BlazorDesktopConfigurationProvider(this);

		string? IBlazorDesktopConfiguration.BrowserExecutablePath { get; set; }

		public string WindowTitle { get; set; } = "Blazor Desktop Launcher";

		public int WindowWidth { get; set; } = 900;
		
		public int WindowHeight { get; set; } = 650;
	}
}