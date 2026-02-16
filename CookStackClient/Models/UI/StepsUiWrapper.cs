using CookStackShared.Recipes.Dtos;

namespace CookStackClient.Models.UI
{
    public class StepsUiWrapper
    {
        public RecipeStepDto Step { get; set; } = default!;
        public bool IsChecked { get; set; }
    }
}
