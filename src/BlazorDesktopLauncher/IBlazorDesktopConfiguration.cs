using Microsoft.Extensions.Configuration;

namespace BlazorDesktopLauncher
{
	public interface IBlazorDesktopConfiguration : IConfigurationSource
	{
		string? BrowserExecutablePath { get; internal set; }
		string? WindowTitle { get; internal set; }
	}
}