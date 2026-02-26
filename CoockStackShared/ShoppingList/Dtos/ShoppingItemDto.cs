using CookStackShared.Enums;

namespace CookStackShared.ShoppingList.Dtos
{
    public class ShoppingItemDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public UnitType Unit { get; set; } = UnitType.Gram;
    }
}
