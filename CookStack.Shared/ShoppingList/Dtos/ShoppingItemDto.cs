using CookStack.Shared.Enums;

namespace CookStack.Shared.ShoppingList.Dtos
{
    public class ShoppingItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public UnitType Unit { get; set; } = UnitType.Gram;
        public bool IsChecked { get; set; } = false;
        public int Order { get; set; }
    }
}
