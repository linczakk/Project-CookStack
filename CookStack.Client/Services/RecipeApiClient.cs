using CookStack.Client.Services.ToastMessage;
using CookStack.Shared.Recipes.Dtos;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace CookStack.Client.Services
{
    public class RecipeApiClient : BaseApiClient
    {
        public RecipeApiClient(HttpClient http, ToastService toast) : base(http, toast)
        {
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

        public async Task<int?> CreateRecipeAsync(CreateRecipeDto dto)
        {
            return await PostAndReadAsync<CreateRecipeDto, int>("api/recipe", dto);
        }

        public async Task<string?> UploadRecipeImageAsync(int recipeId, IBrowserFile file)
        {
            const long maxFileSize = 5 * 1024 * 1024;

            await using var stream = file.OpenReadStream(maxFileSize);

            using var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            using var formContent = new MultipartFormDataContent();

            formContent.Add(fileContent, "file", file.Name);

            var response = await PostContentAndReadAsync<RecipeImageResponseDto>($"api/recipe/{recipeId}/image", formContent);

            return response?.ImagePath;
        }

        public string? GetImageUrl(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return null;

            return new Uri(
                _httpClient.BaseAddress!,
                imagePath.TrimStart('/'))
                .ToString();
        }

        public async Task<bool> MarkRecipeAsVisitedAsync(int id)
        {
            return await PostAsync($"api/recipe/{id}/visit");
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
