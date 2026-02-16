using CookStackShared.Recipes.Dtos;

namespace CoockStackShared.Recipes.Dtos
{
    public abstract class RecipeBaseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? SourceUrl { get; set; }

        public List<RecipeIngredientDto> Ingredients { get; set; } = new();
        public List<RecipeStepDto> Steps { get; set; } = new();
    }
}
