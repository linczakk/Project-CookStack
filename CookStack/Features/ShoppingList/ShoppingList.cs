namespace CookStackApi.Features.ShoppingList
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<ShoppingItem> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
