namespace CookStackClient.Services
{
    public class LoadingService
    {
        public event Action? OnChange;

        public bool IsLoading { get; private set; }
        public string? StatusText { get; private set; }

        public void Show(string? text = null)
        {
            IsLoading = true;
            StatusText = text;
            NotifyStateChanged();
        }

        public void Hide()
        {
            IsLoading = false;
            StatusText = null;
            NotifyStateChanged();
        }

        public async Task RunWithLoading(Func<Task> action, string text)
        {
            Show(text);
            try
            {
                //DELETE LATER
                //await Task.Delay(2000);
                //
                await action();
            }
            finally
            {
                Hide();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
