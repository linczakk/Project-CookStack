namespace CookStack.Client.Services
{
    public class KeyboardShortcutService
    {
        public event Action? OnEscapePressed;
        public event Action? OnSearchShortcutPressed;
        public event Action? OnArrowDown;
        public event Action? OnArrowUp;
        public event Action? OnEnter;
        public event Action? OnMouseDown;

        public bool IsModalOpen { get; set; }

        public void EscapePressed()
        {
            OnEscapePressed?.Invoke();
        }

        public void SearchShortcutPressed()
        {
            OnSearchShortcutPressed?.Invoke();
        }

        public void ArrowDownPressed()
        {
            OnArrowDown?.Invoke();
        }

        public void ArrowUpPressed()
        {
            OnArrowUp?.Invoke();
        }

        public void EnterPressed()
        {
            OnEnter?.Invoke();
        }

        public void MouseUsed()
        {
            OnMouseDown?.Invoke();
        }
    }
}
