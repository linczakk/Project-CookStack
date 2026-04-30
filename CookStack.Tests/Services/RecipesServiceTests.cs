using CookStack.Api.Data;
using CookStack.Api.Features.Recipes;
using Microsoft.EntityFrameworkCore;

namespace CookStack.Tests.Services
{
    public class RecipesServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public RecipesServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

        [Fact]
        public async Task GetAll_Should_ReturnAllRecipes()
        {
            var db = CreateDbContext();
            var service = new RecipesService(db);

            var now = DateTime.UtcNow;

            Recipe[] recipes =
            [
                new Recipe
                {
                    Title = "Test Title 1",
                    CreatedAt = now
                },
                new Recipe
                {
                    Title = "Test Title 2",
                    CreatedAt = now
                }
            ];

            await db.Recipes.AddRangeAsync(recipes);
            await db.SaveChangesAsync();

            var result = await service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Title == "Test Title 1");
            Assert.Contains(result, r => r.Title == "Test Title 2");
            Assert.All(result, r =>
                Assert.NotEqual(default, r.CreatedAt));
        }

        [Fact]
        public async Task GetById_Should_ReturnRecipe()
        {
            var db = CreateDbContext();
            var service = new RecipesService(db);

            var recipe = new Recipe
            {
                Title = "Test Title",
                Ingredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient
                    {
                        Name = "Test Ingredient 1"
                    },
                    new RecipeIngredient
                    {
                        Name = "Test Ingredient 2"
                    }
                },
                Steps = new List<RecipeStep>
                {
                    new RecipeStep
                    {
                        Description = "Test Step"
                    }
                }
            };

            await db.Recipes.AddAsync(recipe);
            await db.SaveChangesAsync();

            var result = await service.GetById(recipe.Id);

            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
            Assert.Equal(2, result.Ingredients.Count());
            Assert.All(result.Ingredients, i => Assert.False(string.IsNullOrEmpty(i.Name)));
            Assert.Contains(result.Steps, s => s.Description == "Test Step");
        }
    }
}
