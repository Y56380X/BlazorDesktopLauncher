namespace BoleroApp.Client

open System
open BlazorDesktopLauncher
open BlazorDesktopLauncher.Extensions

module Program =

    [<EntryPoint>]
    let Main _ =
        let appConfig = BlazorDesktopConfigurationExtensions.UseLocalBrowser
        
        let app = DesktopApplication<Main.MyApp>(null, Action<IBlazorDesktopConfiguration>(appConfig))
        app.RunAsync().Wait()
        0
