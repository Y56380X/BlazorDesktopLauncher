namespace BlazorDesktopLauncher
{
	internal class BlazorDesktopConfiguration : IBlazorDesktopConfiguration
	{
		string? IBlazorDesktopConfiguration.BrowserExecutablePath { get; set; }
	}
}