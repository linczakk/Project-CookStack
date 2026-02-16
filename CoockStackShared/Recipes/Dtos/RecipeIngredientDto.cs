using CookStackShared.Enums;

namespace CookStackShared.Recipes.Dtos
{
    public class RecipeIngredientDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public UnitType Unit { get; set; } = UnitType.Gram;
    }
}