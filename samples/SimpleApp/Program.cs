using Microsoft.Extensions.DependencyInjection;

namespace SimpleApp
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			BlazorDesktopLauncher.Program.Start<Component1>(args, collection =>
			{
				collection.AddSingleton<ExampleJsInterop>();
			});
		}
	}
}