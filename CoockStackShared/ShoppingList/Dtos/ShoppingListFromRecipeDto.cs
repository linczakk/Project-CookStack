using CookStackShared.Recipes.Dtos;
using System.ComponentModel.DataAnnotations;

namespace CookStackShared.ShoppingList.Dtos
{
    public class ShoppingListFromRecipeDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<RecipeIngredientDto> Items { get; set; } = new();
    }
}
