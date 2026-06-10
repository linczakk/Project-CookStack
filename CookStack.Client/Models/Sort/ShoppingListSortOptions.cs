using CookStack.Client.Enums;

namespace CookStack.Client.Models.Sort
{
    public class ShoppingListSortOptions
    {
        public ShoppingListSortField Field { get; set; }
            = ShoppingListSortField.CreatedAt;

        public SortDirection Direction { get; set; }
            = SortDirection.Descending;
    }
}
