using CookStack.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace CookStack.Api.Features.Recipes
{
    public class RecipeIngredient
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        [Required]
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public UnitType Unit { get; set; }
    }
}
