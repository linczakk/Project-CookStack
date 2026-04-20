using CookStack.Shared.Recipes.Dtos;

namespace CookStack.Client.Models.UI
{
    public class IngredientUiWrapper
    {
        public RecipeIngredientDto Ingredient { get; set; } = default!;
        public bool IsChecked { get; set; }
    }
}
