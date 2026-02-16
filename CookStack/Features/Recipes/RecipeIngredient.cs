using CookStackShared.Enums;

namespace CookStackApi.Features.Recipes
{
    public class RecipeIngredient
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public UnitType Unit { get; set; }
    }
}
