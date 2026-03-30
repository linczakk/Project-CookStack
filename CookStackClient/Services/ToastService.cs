using CookStackClient.Models;

namespace CookStackClient.Services.ToastMessage
{
    public class ToastService
    {
        public event Action<ToastModel>? OnShow;
        public event Action? OnHide;

        public void ShowToast(string message, string type="info", int dismissAfter = 3)
        {
            var toast = new ToastModel
            {
                Message = message,
                Type = type,
                Duration = dismissAfter,
            };

            OnShow?.Invoke(toast);
        }

        public void HideToast()
        {
            OnHide?.Invoke();
        }


        public void ShowSuccess(string message, int dismissAfter = 3) => ShowToast(message, "success", dismissAfter);
        public void ShowError(string message, int dismissAfter = 3) => ShowToast(message, "failure", dismissAfter);
        public void ShowInfo(string message, int dismissAfter = 3) => ShowToast(message, "info", dismissAfter);
    }
}
