namespace CookStackShared.ShoppingList.Dtos
{
    public class ShoppingListDetailsDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ShoppingItemDto> Items { get; set; } = new();
    }
}
