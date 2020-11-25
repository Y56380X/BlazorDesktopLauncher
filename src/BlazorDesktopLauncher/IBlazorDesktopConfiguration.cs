using Microsoft.Extensions.Configuration;

namespace BlazorDesktopLauncher
{
	public interface IBlazorDesktopConfiguration : IConfigurationSource
	{
		string? BrowserExecutablePath { get; internal set; }
		string WindowTitle { get; set; }
		int WindowWidth { get; set; }
		int WindowHeight { get; set; }
	}
}