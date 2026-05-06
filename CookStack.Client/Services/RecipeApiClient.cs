using CookStack.Client.Services.ToastMessage;
using CookStack.Shared.Recipes.Dtos;

namespace CookStack.Client.Services
{
    public class RecipeApiClient : BaseApiClient
    {
        public RecipeApiClient(HttpClient http, ToastService toast) : base(http, toast)
        {
        }

        public async Task<List<RecipeListDto>> GetRecipesAsync()
        {
            return await GetAsync<List<RecipeListDto>>("api/recipe")
                ?? new List<RecipeListDto>();
        }

        public async Task<List<RecipeListDto>> GetRecipesAsync(string? searchTerm = null)
        {
            var url = $"api/recipe?search={Uri.EscapeDataString(searchTerm ?? "")}";
            return await GetAsync<List<RecipeListDto>>(url)
                ?? new List<RecipeListDto>();
        }

        public async Task<RecipeDetailsDto?> GetRecipeByIdAsync(int id)
        {
            return await GetAsync<RecipeDetailsDto>($"api/recipe/{id}");
        }

        public async Task<bool> CreateRecipeAsync(CreateRecipeDto dto)
        {
            return await PostAsync("api/recipe", dto);
        }

        public async Task<bool> UpdateRecipeAsync(int id, RecipeUpdateDto dto)
        {
            return await PutAsync($"api/recipe/{id}", dto);
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            return await DeleteAsync($"api/recipe/{id}");
        }
    }
}
