namespace CookStack.Shared.ShoppingList.Dtos
{
    public class ShoppingListsListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public int TotalItems { get; set; }
        public int CompletedItems { get; set; }

        public bool IsCompleted => TotalItems > 0 && TotalItems == CompletedItems;
    }
}
