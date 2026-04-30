using CookStack.Shared.Recipes.Dtos;

namespace CookStack.Api.Features.Recipes
{
    public interface IRecipesService
    {
        Task<IEnumerable<RecipeListDto>> GetAll();
        Task<RecipeDetailsDto?> GetById(int id);
        Task<int> Create(CreateRecipeDto dto);
        Task<bool> Update(int id, RecipeUpdateDto dto);
        Task<bool> Delete(int id);
    }
}
