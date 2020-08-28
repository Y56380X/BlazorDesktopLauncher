using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace SimpleApp
{
    public class ExampleJsInterop
    {
        public ValueTask<string> Prompt(IJSRuntime jsRuntime, string message)
        {
            // Implemented in exampleJsInterop.js
            return jsRuntime.InvokeAsync<string>(
                "exampleJsFunctions.showPrompt",
                message);
        }
        
        public async Task Alert(IJSRuntime jsRuntime, string message)
        {
            // Implemented in exampleJsInterop.js
            await jsRuntime.InvokeVoidAsync(
                "exampleJsFunctions.showAlert", message);
        }
    }
}
