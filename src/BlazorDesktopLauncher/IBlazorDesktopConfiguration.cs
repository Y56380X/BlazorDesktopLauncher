namespace BlazorDesktopLauncher
{
	public interface IBlazorDesktopConfiguration
	{
		string? BrowserExecutablePath { get; internal set; }
	}
}