using CookStack.Shared.Enums;

namespace CookStack.Shared.ShoppingList.Dtos
{
    public class SelectableIngredientDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set;} = decimal.Zero;
        public UnitType Unit { get; set; } = UnitType.Gram;

        public bool IsSelected { get; set;}
    }
}
