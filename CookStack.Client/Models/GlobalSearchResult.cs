using CookStack.Shared.Recipes.Dtos;
using CookStack.Shared.ShoppingList.Dtos;

namespace CookStack.Client.Models
{
    public class GlobalSearchResult
    {
        public List<RecipeListDto> Recipes { get; set; } = new();
        public List<ShoppingListsListDto> ShoppingLists { get; set; } = new();
    }
}
