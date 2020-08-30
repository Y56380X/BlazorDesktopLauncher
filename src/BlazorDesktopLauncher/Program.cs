using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PuppeteerSharp;

namespace BlazorDesktopLauncher
{
	public interface IBlazorDesktopConfiguration
	{
		string? BrowserExecutablePath { get; internal set; }
	}

	internal class BlazorDesktopConfiguration : IBlazorDesktopConfiguration
	{
		string? IBlazorDesktopConfiguration.BrowserExecutablePath { get; set; }
	}

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

	public class Program
	{
		public static Type MainPage { get; private set; }

		public static void Start<T>(string[] args,
			Action<IServiceCollection>? cnf = null,
			Action<IBlazorDesktopConfiguration>? bdc = null) where T : ComponentBase
		{
			MainPage = typeof(T);
			var runTask = CreateHostBuilder(args)
				.ConfigureServices(sv => cnf?.Invoke(sv))
				.Build().RunAsync();
			var bdConfig = new BlazorDesktopConfiguration();
			bdc?.Invoke(bdConfig);
			OpenWindow(bdConfig);
			runTask.Wait();
		}

		private static Task OpenWindow(IBlazorDesktopConfiguration bdc)
		{
			return Task.Run(async () =>
			{
				if (string.IsNullOrWhiteSpace(bdc.BrowserExecutablePath) || !File.Exists(bdc.BrowserExecutablePath))
					await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

				var browser = await Puppeteer.LaunchAsync(new LaunchOptions
				{
					ExecutablePath = bdc.BrowserExecutablePath,
					Headless = false,
					Args = new[]
					{
						"--app=http://localhost:5005/",
						"--window-size=900,650",
						"--allow-insecure-localhost",
						"--disable-extensions"
					}
				});
				browser.Closed += (sender, eventArgs) => Environment.Exit(0);
			});
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStaticWebAssets();
					webBuilder.UseWebRoot(GetWebRootPath());
					webBuilder.UseUrls("http://localhost:5005/");
					webBuilder.UseStartup<Startup>();
				});

		private static string GetWebRootPath() =>
			Path.Combine(Environment.CurrentDirectory, "wwwroot");
	}
}