using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace BlazorDesktopLauncher
{
    public class Program
    {
        public static Type MainPage;
        
        public static void Start(string[] args, Type mainPage)
        {
            MainPage = mainPage;
            var runTask = CreateHostBuilder(args, mainPage).Build().RunAsync();
            OpenWindow();
            runTask.Wait();
        }

        private static Task OpenWindow()
        {
            return Task.Run(async () =>
            {
                await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
				
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = false,
                    Args = new []
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

        private static IHostBuilder CreateHostBuilder(string[] args, Type mainPage) =>
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
