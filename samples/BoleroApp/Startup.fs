namespace BoleroApp.Client

open System
open BlazorDesktopLauncher
open BlazorDesktopLauncher.Extensions

module Program =

    [<EntryPoint>]
    let Main _ =
        let configure (appConfig: IBlazorDesktopConfiguration) =
            appConfig.UseLocalBrowser()
            appConfig.WindowTitle <- "Bolero Sample App"
        
        let app = DesktopApplication<Main.MyApp>(null, Action<IBlazorDesktopConfiguration>(configure))
        app.RunAsync().Wait()
        0
