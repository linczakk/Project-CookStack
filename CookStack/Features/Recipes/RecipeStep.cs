using System.ComponentModel.DataAnnotations;

namespace CookStackApi.Features.Recipes
{
    public class RecipeStep
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        public int Order { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
