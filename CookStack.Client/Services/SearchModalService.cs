namespace CookStack.Client.Services
{
    public class SearchModalService
    {
        public event Func<Task>? OnOpen;

        public async Task Open()
        {
            if(OnOpen != null)
            {
                await OnOpen.Invoke();
            }
        }
    }
}
