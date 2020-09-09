# BlazorDesktopLauncher

Launches a razor class library as blazor server application 
 and starts a navigation less browser frame.

!THIS IS VERY EXPERIMENTAL!

## Getting started

### SimpleApp example

1. Start with a razor class library
2. Add BlazorDesktopLauncher nuget package
3. Add a Program.cs file with a main method as entry point
4. Instantiate a new DesktopApplication, you can use the Component1 
 from the template for your applications main component (```DesktopApplication<Component1>```).
5. Call RunAsync Method with await or .Wait()
6. Change .csproj OutputType to Exe (```<OutputType>Exe</OutputType>```)

#### Setup static content

Add following code to your .csproj:

```
<ItemGroup>
      <Content Update="wwwroot\**">
        <ExcludeFromSingleFile>true</ExcludeFromSingleFile>
        <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      </Content>
    </ItemGroup>
```

#### Load stylesheet in your Component1

Add following code to your Component1:

```
<link rel="stylesheet" href="styles.css" />
```

#### Run application in a locally installed browser

By default the launcher will download a chromium browser. 
 On many systems a chromium browser is already installed, so the launcher is able to try using it.

For this you have to modify the instantiation of the DesktopApplication. Add an argument:
```appConfiguration: app => app.UseLocalBrowser()```.
