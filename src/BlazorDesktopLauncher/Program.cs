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
	public abstract class MainComponentProvider
	{
		public abstract Type MainComponent { get; }
	}
	
	public class DesktopApplication<T> : MainComponentProvider where T : ComponentBase
	{
		public override Type MainComponent { get; }
		
		private readonly Action<IServiceCollection>? _serviceConfiguration;
		private readonly Action<IBlazorDesktopConfiguration>? _appConfiguration;

		public DesktopApplication(
			Action<IServiceCollection>? serviceConfiguration = null, 
			Action<IBlazorDesktopConfiguration>? appConfiguration = null)
		{
			_serviceConfiguration = serviceConfiguration;
			_appConfiguration = appConfiguration;
			MainComponent = typeof(T);
		}

		public async Task RunAsync()
		{
			var runTask = CreateHostBuilder(Environment.GetCommandLineArgs())
				.ConfigureServices(sv =>
				{
					_serviceConfiguration?.Invoke(sv);
					sv.AddSingleton<MainComponentProvider>(this);
				})
				.Build().RunAsync();
			
			var bdConfig = new BlazorDesktopConfiguration();
			_appConfiguration?.Invoke(bdConfig);
			var browserTask = OpenWindow(bdConfig);
			
			await Task.WhenAll(browserTask, runTask);
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