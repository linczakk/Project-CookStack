using CookStackShared.Recipes.Dtos;

namespace CookStackClient.Models.UI
{
    public class IngredientUiWrapper
    {
        public RecipeIngredientDto Ingredient { get; set; } = default!;
        public bool IsChecked { get; set; }
    }
}
