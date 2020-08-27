using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorDesktopLauncher
{
    public class Program
    {
        public static Type MainPage;
        
        public static void Start(string[] args, Type mainPage)
        {
            MainPage = mainPage;
            CreateHostBuilder(args, mainPage).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args, Type mainPage) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStaticWebAssets();
                    webBuilder.UseWebRoot(GetWebRootPath(mainPage));
                    webBuilder.UseStartup<Startup>();
                });

        private static string GetWebRootPath(Type type) => 
            Path.Combine(Path.GetDirectoryName(type.Assembly.Location)!, "wwwroot");
    }
}
