using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace SimpleApp
{
    public class ExampleJsInterop
    {
        private readonly IJSRuntime _jsRuntime;

        public ExampleJsInterop(IJSRuntime jsRuntime) => _jsRuntime = jsRuntime;

        public ValueTask<string> Prompt(string message)
        {
            // Implemented in exampleJsInterop.js
            return _jsRuntime.InvokeAsync<string>(
                "exampleJsFunctions.showPrompt",
                message);
        }
        
        public async Task Alert(string message)
        {
            // Implemented in exampleJsInterop.js
            await _jsRuntime.InvokeVoidAsync(
                "exampleJsFunctions.showAlert", message);
        }
    }
}
