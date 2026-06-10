using CookStack.Client.Enums;

namespace CookStack.Client.Models.Sort
{
    public class RecipeSortOptions
    {
        public RecipeSortField Field { get; set; }
            = RecipeSortField.CreatedAt;

        public SortDirection Direction { get; set; }
            = SortDirection.Descending;
    }
}
