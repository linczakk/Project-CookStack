namespace CookStack.Client.Services
{
    public class KeyboardShortcutService
    {
        public event Action? OnEscapePressed;
        public event Action? OnSearchShortcutPressed;

        public void EscapePressed()
        {
            OnEscapePressed?.Invoke();
        }

        public void SearchShortcutPressed()
        {
            OnSearchShortcutPressed?.Invoke();
        }
    }
}
