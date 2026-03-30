using CookStackClient.Services.ToastMessage;
using CookStackShared.ShoppingList.Dtos;
using System.Net.Http.Json;


namespace CookStackClient.Services
{
    public class ShoppingListApiClient : BaseApiClient
    {
        public ShoppingListApiClient(HttpClient http, ToastService toast) : base(http, toast)
        {
        }

        public async Task<List<ShoppingListsListDto>> GetShoppingListsAsync()
        {
            return await GetAsync<List<ShoppingListsListDto>>("api/ShoppingList") ?? new List<ShoppingListsListDto>();
        }

        public async Task<ShoppingListDetailsDto?> GetShoppingListByIdAsync(int id)
        {
            return await GetAsync<ShoppingListDetailsDto>($"api/ShoppingList/{id}");
        }

        public async Task<bool> CreateShoppingListAsync(CreateShoppingListDto dto)
        {
            return await PostAsync("api/ShoppingList", dto);
        }

        public async Task<bool> CreateShoppingListFromRecipeAsync(ShoppingListFromRecipeDto dto)
        {
            return await PostAsync("api/ShoppingList/from-recipe", dto);
        }

        public async Task<bool> UpdateShoppingListAsync(int id, ShoppingListUpdateDto dto)
        {
            return await PutAsync($"api/ShoppingList/{id}", dto);
        }

        public async Task<bool> DeleteShoppingListAsync(int id)
        {
            return await DeleteAsync($"api/ShoppingList/{id}");
        }
    }
}
