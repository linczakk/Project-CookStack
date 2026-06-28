namespace CookStack.Client.Services
{
    public class SearchModalService
    {
        public event Func<string?, Task>? OnOpen;

        public async Task Open(string? searchTerm = null)
        {
            if(OnOpen != null)
                await OnOpen.Invoke(searchTerm);
        }
    }
}
