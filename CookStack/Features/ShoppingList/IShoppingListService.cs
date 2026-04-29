using CookStack.Shared.ShoppingList.Dtos;

namespace CookStack.Api.Features.ShoppingList
{
    public interface IShoppingListService
    {
        Task<IEnumerable<ShoppingListsListDto>> GetAll();
        Task<ShoppingListDetailsDto?> GetById(int id);
        Task<int> Create(CreateShoppingListDto dto);
        Task<int> CreateFromRecipe(AddIngredientsToShoppingListDto dto);
        Task<int?> AddToExisting(AddIngredientsToShoppingListDto dto);
        Task<bool> Update(int id, ShoppingListUpdateDto dto);
        Task<bool> Delete(int id);
    }
}
