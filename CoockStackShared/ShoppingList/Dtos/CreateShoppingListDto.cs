using System.ComponentModel.DataAnnotations;

namespace CookStackShared.ShoppingList.Dtos
{
    public class CreateShoppingListDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ShoppingItemDto> Items { get; set; } = new();
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
