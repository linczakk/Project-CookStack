using CookStack.Shared.Recipes.Dtos;

namespace CookStack.Client.Models.UI
{
    public class StepsUiWrapper
    {
        public RecipeStepDto Step { get; set; } = default!;
        public bool IsChecked { get; set; }
    }
}
