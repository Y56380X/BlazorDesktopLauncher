using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
		private static int ApplicationPort { get; } = ReserveApplicationPort();
		
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
			var bdConfig = new BlazorDesktopConfiguration();
			_appConfiguration?.Invoke(bdConfig);
			
			var runTask = CreateHostBuilder(Environment.GetCommandLineArgs())
				.ConfigureAppConfiguration(builder => builder.Add(bdConfig))
				.ConfigureServices(sv =>
				{
					_serviceConfiguration?.Invoke(sv);
					sv.AddSingleton<MainComponentProvider>(this);
				})
				.Build().RunAsync();
			
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
						$"--app=http://localhost:{ApplicationPort}/",
						$"--window-size={bdc.WindowWidth},{bdc.WindowHeight}",
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
					webBuilder.UseUrls($"http://localhost:{ApplicationPort}/");
					webBuilder.UseStartup<Startup>();
				});

		private static string GetWebRootPath() =>
			Path.Combine(Environment.CurrentDirectory, "wwwroot");

		private static int ReserveApplicationPort()
		{
			var port = 5000;
			var success = false;
			do
			{
				var listener = new TcpListener(IPAddress.Loopback, port) { ExclusiveAddressUse = true };
				try
				{
					listener.Start();
					listener.Stop();
					success = true;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					port++;
				}
			} while (!success);

			return port;
		}
	}
}