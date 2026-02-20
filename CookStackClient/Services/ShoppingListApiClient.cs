using CookStackShared.ShoppingList.Dtos;
using System.Net.Http.Json;


namespace CookStackClient.Services
{
    public class ShoppingListApiClient
    {
        private readonly HttpClient _httpClient;

        public ShoppingListApiClient(HttpClient http)
        {
            _httpClient = http;
        }

        public async Task<List<ShoppingListsListDto>> GetShoppingListsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ShoppingListsListDto>>("api/ShoppingList")
                ?? new List<ShoppingListsListDto>();
        }

        public async Task<ShoppingListDetailsDto?> GetShoppingListByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ShoppingListDetailsDto>($"api/ShoppingList/{id}");
        }

        public async Task CreateShoppingListAsync(CreateShoppingListDto dto)
        {
            await _httpClient.PostAsJsonAsync("api/ShoppingList", dto);
        }

        public async Task UpdateShoppingListAsync(int id, ShoppingListUpdateDto dto)
        {
            await _httpClient.PutAsJsonAsync($"api/ShoppingList/{id}", dto);
        }

        public async Task DeleteShoppingListAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/ShoppingList/{id}");
        }
    }
}
