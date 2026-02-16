namespace CookStackShared.Features.ShoppingList.Dtos
{
    public class ShoppingListDto
    {
        public string Description { get; set; } = string.Empty;
        public List<ShoppingItemDto> Items { get; set; } = new();
    }
}
