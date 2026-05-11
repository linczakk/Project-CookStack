using Microsoft.JSInterop;

namespace CookStack.Client.Services
{
    public class BrowserTitleService
    {
        private readonly IJSRuntime js;

        public BrowserTitleService(IJSRuntime js)
        {
            this.js = js;
        }

        public async Task SetTitle(string title)
        {
            try
            {
                await js.InvokeVoidAsync(
               "setDocumentTitle",
               $"{title} - CookStack");
            }
            catch (JSException)
            {
                // JS may not be ready during first render
            }
        }
    }
}
