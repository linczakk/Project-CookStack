using CookStack.Api.Data;
using CookStack.Shared.Recipes.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CookStack.Api.Features.Recipes
{
    public class RecipesService : IRecipesService
    {
        private readonly ApplicationDbContext _dbContext;

        public RecipesService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<RecipeListDto>> GetAll()
        {
            var recipes = await _dbContext.Recipes
                .Select(r => new RecipeListDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    CreatedAt = r.CreatedAt
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return recipes;
        }

        public async Task<RecipeDetailsDto?> GetById(int id)
        {
            var recipe = await _dbContext.Recipes
                .Where(r => r.Id == id)
                .Select(r => new RecipeDetailsDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    SourceUrl = r.SourceUrl,
                    Ingredients = r.Ingredients
                    .Select(i => new RecipeIngredientDto
                    {
                        Name = i.Name,
                        Quantity = i.Quantity,
                        Unit = i.Unit

                    })
                    .ToList(),
                    Steps = r.Steps
                    .Select(s => new RecipeStepDto
                    {
                        Order = s.Order,
                        Description = s.Description
                    })
                    .ToList(),

                }).FirstOrDefaultAsync();

            return recipe;
        }

        public async Task<int> Create(CreateRecipeDto dto)
        {
            var recipe = new Recipe
            {
                Title = dto.Title,
                Description = dto.Description ?? string.Empty,
                SourceUrl = dto.SourceUrl,
                Ingredients = dto.Ingredients.Select(i => new RecipeIngredient
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit

                })
                .ToList(),

                Steps = dto.Steps
                .OrderBy(s => s.Order)
                .Select((s, index) => new RecipeStep
                {
                    Order = index + 1,
                    Description = s.Description
                })
                .ToList()
            };

            _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();

            return recipe.Id;
        }
        public async Task<bool> Update(int id, RecipeUpdateDto dto)
        {
            var recipeFound = false;
            var recipe = await _dbContext.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe != null)
            {
                recipeFound = true;
                recipe.Title = dto.Title;
                recipe.Description = dto.Description;
                recipe.SourceUrl = dto.SourceUrl;


                _dbContext.Ingredients.RemoveRange(recipe.Ingredients);
                _dbContext.Steps.RemoveRange(recipe.Steps);

                recipe.Ingredients = dto.Ingredients
                    .Select(i => new RecipeIngredient
                    {
                        Name = i.Name,
                        Quantity = i.Quantity,
                        Unit = i.Unit
                    }).ToList();

                recipe.Steps = dto.Steps
                    .OrderBy(s => s.Order)
                    .Select(s => new RecipeStep
                    {
                        Order = s.Order,
                        Description = s.Description

                    }).ToList();


                await _dbContext.SaveChangesAsync();
            }
            return recipeFound;
        }

        public async Task<bool> Delete(int id)
        {
            var recipeFound = false;
            var recipe = await _dbContext.Recipes.FindAsync(id);

            if(recipe != null)
            {
                recipeFound = true;

                _dbContext.Recipes.Remove(recipe);
                await _dbContext.SaveChangesAsync();
            }
            return recipeFound;
        }

    }
}
