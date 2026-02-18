namespace CookStackShared.ShoppingList.Dtos
{
    public class CreateShoppingListDto
    {
        public string Description { get; set; } = string.Empty;
        public List<ShoppingItemDto> Items { get; set; } = new();
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
