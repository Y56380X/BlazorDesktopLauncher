using Microsoft.Extensions.DependencyInjection;

namespace BlazorDesktopLauncher.Extensions
{
	public class JsScriptSource
	{
		private readonly string _path;

		public string[] ScriptPaths => new [] { _path };

		public JsScriptSource(string path) => _path = path;
	}
	
	public static class ServiceCollectionExtensions
	{
		public static void AddJsLoader(this IServiceCollection services, string path) => 
			services.AddSingleton(new JsScriptSource(path));
	}
}