using System.ComponentModel.DataAnnotations;


namespace CookStackApi.Features.Recipes
{
    public class Recipe
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<RecipeIngredient> Ingredients { get; set; } = new();
        public List<RecipeStep> Steps { get; set; } = new();
        public string? SourceUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
