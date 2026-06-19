namespace CookStack.Client.Services
{
    public class AddShoppingListModalService
    {
        public event Func<Task>? OnOpen;

        public async Task Open()
        {
            if(OnOpen != null)
                await OnOpen.Invoke();
        }
    }
}
