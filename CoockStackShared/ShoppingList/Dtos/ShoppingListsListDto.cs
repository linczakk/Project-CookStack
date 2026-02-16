namespace CookStackShared.Features.ShoppingList.Dtos
{
    public class ShoppingListsListDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
