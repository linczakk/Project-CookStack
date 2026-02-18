using CookStackShared.Recipes.Dtos;
using System.Net.Http.Json;

namespace CookStackClient.Services
{
    public class RecipesApiClient
    {
        private readonly HttpClient _httpClient;

        public RecipesApiClient(HttpClient http)
        {
            _httpClient = http;
        }

        public async Task<List<RecipeListDto>> GetRecipesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RecipeListDto>>("api/recipes")
                ?? new List<RecipeListDto>();
        }

        public async Task<RecipeDetailsDto?> GetRecipeByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<RecipeDetailsDto>($"api/recipes/{id}");
        }

        public async Task CreateRecipeAsync(CreateRecipeDto dto)
        {
            await _httpClient.PostAsJsonAsync("api/recipes", dto);
        }

        public async Task UpdateRecipeAsync(int id, RecipeUpdateDto dto)
        {
            await _httpClient.PutAsJsonAsync($"api/recipes/{id}", dto);
        }

        public async Task DeleteRecipeAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/recipes/{id}");
        }
    }
}
