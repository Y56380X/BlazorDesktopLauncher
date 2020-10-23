using Microsoft.Extensions.Configuration;

namespace BlazorDesktopLauncher
{
	internal class BlazorDesktopConfigurationProvider : ConfigurationProvider
	{
		private readonly IBlazorDesktopConfiguration _configuration;

		public BlazorDesktopConfigurationProvider(IBlazorDesktopConfiguration blazorDesktopConfiguration) => 
			_configuration = blazorDesktopConfiguration;

		public override void Load()
		{
			Set(nameof(_configuration.BrowserExecutablePath), _configuration.BrowserExecutablePath);
			Set(nameof(_configuration.WindowTitle), _configuration.WindowTitle);
		}
	}
}