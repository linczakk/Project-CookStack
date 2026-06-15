using CookStack.Client.Enums;
using CookStack.Client.Models.Sort;

namespace CookStack.Client.Services
{
    public class SortStateService
    {
        public SortOptions<RecipeSortField> RecipeSort { get; set; } = new()
        {
            Field = RecipeSortField.CreatedAt,
            Direction = SortDirection.Descending
        };

        public SortOptions<ShoppingListSortField> ShoppingListSort { get; set; } = new()
        {
            Field = ShoppingListSortField.CreatedAt,
            Direction = SortDirection.Descending
        };
    }
}
