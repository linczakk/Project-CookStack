using CookStackShared.Enums;
using System.ComponentModel.DataAnnotations;

namespace CookStackApi.Features.ShoppingList
{
    public class ShoppingItem
    {
        public int Id { get; set; }

        public int ShoppingListId { get; set; }
        public ShoppingList ShoppingList { get; set; } = null!;

        [Required]
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public UnitType Unit { get; set; }
        public bool IsChecked { get; set; }
    }
}
