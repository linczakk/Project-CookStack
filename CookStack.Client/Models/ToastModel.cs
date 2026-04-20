namespace CookStack.Client.Models
{
    public class ToastModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "info";
        public int Duration { get; set; } = 3;
    }
}
