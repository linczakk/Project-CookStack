using CookStackShared.Recipes.Dtos;
using System.ComponentModel.DataAnnotations;

namespace CookStackShared.ShoppingList.Dtos
{
    public class AddIngredientsToShoppingListDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public int? ExistingListId { get; set; }

        public List<ShoppingItemDto> Items { get; set; } = new();
    }
}
