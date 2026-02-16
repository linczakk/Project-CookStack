namespace CookStackShared.Features.ShoppingList.Dtos
{
    public class ShoppingListUpdateDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<ShoppingItemDto> Items { get; set; } = new();
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
