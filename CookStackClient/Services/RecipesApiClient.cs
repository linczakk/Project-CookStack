using CookStackClient.Services.ToastMessage;
using CookStackShared.Recipes.Dtos;
using System.Net.Http.Json;

namespace CookStackClient.Services
{
    public class RecipesApiClient : BaseApiClient
    {
        public RecipesApiClient(HttpClient http, ToastService toast) : base(http, toast)
        {
        }

        public async Task<List<RecipeListDto>> GetRecipesAsync()
        {
            return await GetAsync<List<RecipeListDto>>("api/recipes")
                ?? new List<RecipeListDto>();
        }

        public async Task<RecipeDetailsDto?> GetRecipeByIdAsync(int id)
        {
            return await GetAsync<RecipeDetailsDto>($"api/recipes/{id}");
        }

        public async Task<bool> CreateRecipeAsync(CreateRecipeDto dto)
        {
            return await PostAsync("api/recipes", dto);
        }

        public async Task<bool> UpdateRecipeAsync(int id, RecipeUpdateDto dto)
        {
            return await PutAsync($"api/recipes/{id}", dto);
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            return await DeleteAsync($"api/recipes/{id}");
        }
    }
}
