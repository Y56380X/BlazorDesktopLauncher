using System;
using System.IO;

namespace BlazorDesktopLauncher.Extensions
{
	public static class BlazorDesktopConfigurationExtensions
	{
		public static void UseLocalBrowser(this IBlazorDesktopConfiguration config)
		{
			static bool IsEdgeInstalled() => 
				File.Exists(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
		
			static bool IsChromiumInstalled() => File.Exists("/usr/bin/chromium");
			
			static string GetExecutablePath() => Environment.OSVersion.Platform switch
			{
				// Check for edge (win)
				PlatformID.Win32NT when IsEdgeInstalled() => @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
				// Check for chromium (linux)
				PlatformID.Unix when IsChromiumInstalled() => "/usr/bin/chromium",
				_ => string.Empty
			};

			config.BrowserExecutablePath = GetExecutablePath();
		}
	}
}