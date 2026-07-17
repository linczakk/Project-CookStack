using CookStack.Api.Data;
using CookStack.Api.Features.Recipes;
using CookStack.Shared.Recipes.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CookStack.Tests.Services
{
    public class RecipeServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public RecipeServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

        private static RecipeService CreateService(ApplicationDbContext dbContext)
        {
            var environmentMock = new Mock<IWebHostEnvironment>();

            environmentMock
                .SetupGet(e => e.WebRootPath)
                .Returns(Path.GetTempPath());

            return new RecipeService(dbContext, environmentMock.Object);
        }

        [Fact]
        public async Task GetAll_Should_ReturnAllRecipes_WhenSearchIsNull()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var now = DateTime.UtcNow;

            Recipe[] recipes =
            [
                new Recipe
                {
                    Title = "Test Title 1",
                    CreatedAt = now,
                    LastVisitedAt = now
                },
                new Recipe
                {
                    Title = "Test Title 2",
                    CreatedAt = now,
                    LastVisitedAt = now
                }
            ];

            await db.Recipes.AddRangeAsync(recipes);
            await db.SaveChangesAsync();

            var result = await service.GetAll(null);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Title == "Test Title 1");
            Assert.Contains(result, r => r.Title == "Test Title 2");
            Assert.All(result, r => Assert.NotEqual(default, r.CreatedAt));
            Assert.All(result, r => Assert.NotEqual(default, r.LastVisitedAt));
        }
        

        [Fact]
        public async Task GetAll_Should_ReturnFilteredRecipes_WhenSearchMatchesTitle()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var now = DateTime.UtcNow;

            Recipe[] recipes =
            [
                new Recipe
                {
                    Title = "Test Title 1",
                    CreatedAt = now,
                    LastVisitedAt = now,
                },
                new Recipe
                {
                    Title = "Test Title 2",
                    CreatedAt = now,
                    LastVisitedAt = now,
                },
            ];

            await db.Recipes.AddRangeAsync(recipes);
            await db.SaveChangesAsync();

            var result = await service.GetAll("Test Title 2");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(result, r => r.Title == "Test Title 2");
            Assert.DoesNotContain(result, r => r.Title == "Test Title 1");
            Assert.All(result, r => Assert.NotEqual(default, r.CreatedAt));
            Assert.All(result, r => Assert.NotEqual(default, r.LastVisitedAt));
        }

        [Fact]
        public async Task GetAll_Should_ReturnEmptyList_WhenNoMatchFound()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var recipe = new Recipe
            {
                Title = "Test Title 1",
            };

            await db.Recipes.AddAsync(recipe);
            await db.SaveChangesAsync();

            var result = await service.GetAll("Test Title 2");

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetById_Should_ReturnRecipe()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

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

        [Fact]
        public async Task GetById_Should_ReturnNull_WhenRecipeNotFound()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var result = await service.GetById(0);

            Assert.Null(result);
        }

        [Fact]
        public async Task Create_Should_CreateRecipe()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var recipeDto = new CreateRecipeDto
            {
                Title = "Test Title",
                Ingredients = new List<RecipeIngredientDto>
                {
                    new RecipeIngredientDto
                    {
                        Name = "Test Ingredient 1"
                    },
                    new RecipeIngredientDto
                    {
                        Name = "Test Ingredient 2"
                    }
                },
                Steps = new List<RecipeStepDto>
                {
                    new RecipeStepDto
                    {
                        Description = "Test Step"
                    }
                }
            };

            var success = await service.Create(recipeDto);

            var result = db.Recipes.SingleOrDefault(r => r.Title == "Test Title");

            Assert.NotNull(result);
            Assert.True(success > 0);
            Assert.Equal(2, result.Ingredients.Count());
            Assert.All(result.Ingredients, i => Assert.False(string.IsNullOrEmpty(i.Name)));
            Assert.Contains(result.Steps, s => s.Description == "Test Step");
            Assert.Equal(1, result.Steps.First().Order);
        }

        [Fact]
        public async Task Update_Should_UpdateRecipe()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var recipe = new Recipe
            {
                Title = "Test Title",
                Description = "Test Description",
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
                        Description = "Test Step 1"
                    }
                }
            };

            await db.Recipes.AddAsync(recipe);
            await db.SaveChangesAsync();

            var recipeDto = new RecipeUpdateDto
            {
                Title = recipe.Title,
                Description = "Updated Test Description",
                Ingredients = recipe.Ingredients.Select(i => new RecipeIngredientDto
                {
                    Name = i.Name
                }).ToList(),
                Steps = recipe.Steps.Select(s => new RecipeStepDto
                {
                    Description = s.Description
                }).ToList()
            };

            recipeDto.Ingredients.Add(new RecipeIngredientDto
            {
                Name = "Test Ingredient 3"
            });
            recipeDto.Steps.Add(new RecipeStepDto
            {
                Description = "Test Step 2"
            });

            var success = await service.Update(recipe.Id, recipeDto);

            var result = db.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .First(r => r.Id == recipe.Id);

            Assert.NotNull(result);
            Assert.True(success);
            Assert.Equal("Updated Test Description", result.Description);
            Assert.Equal(3, result.Ingredients.Count());
            Assert.Contains(result.Ingredients, i => i.Name == "Test Ingredient 3");
            Assert.Equal(2, result.Steps.Count());
            Assert.Equal(2, result.Steps[1].Order);
        }

        [Fact]
        public async Task Update_Should_ReturnFalse_WhenRecipeNotFound()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var recipeDto = new RecipeUpdateDto
            {
                Title = "Test Title"
            };

            var success = await service.Update(0, recipeDto);

            Assert.False(success);
            Assert.Empty(db.Recipes);
        }

        [Fact]
        public async Task MarkAsVisited_Should_MarkRecipeAsVisited()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var recipe = new Recipe
            {
                Title = "Test Title",
            };

            await db.AddAsync(recipe);
            await db.SaveChangesAsync();

            var success = await service.MarkAsVisited(recipe.Id);

            var result = db.Recipes.First(r => r.Id == recipe.Id);

            Assert.NotNull(result);
            Assert.True(success);
            Assert.NotNull(result.LastVisitedAt);
        }

        [Fact]
        public async Task MarkAsVisited_Should_ReturnFalse_WhenRecipeNotFound()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var success = await service.MarkAsVisited(0);

            Assert.False(success);
        }

        [Fact]
        public async Task Delete_Should_DeleteRecipe()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var recipe = new Recipe
            {
                Title = "Test Title"
            };

            await db.Recipes.AddAsync(recipe);
            await db.SaveChangesAsync();

            var success = await service.Delete(recipe.Id);

            var result = db.Recipes.Find(recipe.Id);

            Assert.True(success);
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_Should_ReturnFalse_WhenRecipeNotFound()
        {
            await using var db = CreateDbContext();
            var service = CreateService(db);

            var success = await service.Delete(0);

            Assert.False(success);
            Assert.Empty(db.Recipes);
        }
    }
}
