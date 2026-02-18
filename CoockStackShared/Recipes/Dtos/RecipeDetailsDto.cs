namespace CookStackShared.Recipes.Dtos
{
    public class RecipeDetailsDto
    {
        public int? Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? SourceUrl { get; set; }

        public List<RecipeIngredientDto> Ingredients { get; set; } = new();
        public List<RecipeStepDto> Steps { get; set; } = new();
    }
}
