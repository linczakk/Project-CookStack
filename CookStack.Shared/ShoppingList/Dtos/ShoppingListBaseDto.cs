using System.ComponentModel.DataAnnotations;

namespace CookStack.Shared.ShoppingList.Dtos
{
    public abstract class ShoppingListBaseDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ShoppingItemDto> Items { get; set; } = new();
        public bool IsChecked { get; set; }
        public int Order { get; set; }
    }
}
