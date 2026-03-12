using CookStackShared.ShoppingList.Dtos;

namespace CookStackShared.ShoppingList.Mappings
{
    public static class ShoppingListMappings
    {
        public static ShoppingListUpdateDto ToUpdateDto(this ShoppingListDetailsDto dto)
        {
            return new ShoppingListUpdateDto
            {
                Title = dto.Title,
                Description = dto.Description,
                Items = dto.Items
            };
        }
    }
}
