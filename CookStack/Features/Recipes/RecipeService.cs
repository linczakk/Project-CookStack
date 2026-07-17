using CookStack.Api.Data;
using CookStack.Shared.Recipes.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CookStack.Api.Features.Recipes
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public RecipeService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _dbContext = context;
            _environment = environment;
        }

        public async Task<IEnumerable<RecipeListDto>> GetAll(string? search = null)
        {
            var query = _dbContext.Recipes.AsQueryable();

            if(!string.IsNullOrWhiteSpace(search))
            {
                var normalized = search.ToLower();
                query = query.Where(r => r.Title.ToLower().Contains(normalized));
            }

            return await query
                .Select(r => new RecipeListDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    ImagePath = r.ImagePath,
                    CreatedAt = r.CreatedAt,
                    LastVisitedAt = r.LastVisitedAt,
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
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
                    ImagePath = r.ImagePath,
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
                    Quantity = Math.Max(0, i.Quantity),
                    Unit = i.Unit

                })
                .ToList(),

                Steps = dto.Steps
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
            var recipe = await _dbContext.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
                return false;

            recipe.Title = dto.Title;
            recipe.Description = dto.Description;
            recipe.SourceUrl = dto.SourceUrl;


            _dbContext.Ingredients.RemoveRange(recipe.Ingredients);
            _dbContext.Steps.RemoveRange(recipe.Steps);

            recipe.Ingredients = dto.Ingredients
                .Select(i => new RecipeIngredient
                {
                    Name = i.Name,
                    Quantity = Math.Max(0, i.Quantity),
                    Unit = i.Unit
                }).ToList();

            recipe.Steps = dto.Steps
                .Select((s, index) => new RecipeStep
                {
                    Order = index + 1,
                    Description = s.Description

                }).ToList();


            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<string> UploadImage(int recipeId, IFormFile file)
        {
            if (file is null || file.Length == 0)
                throw new ArgumentException("No image was provided.");

            const long maxFileSize = 5 * 1024 * 1024;

            if (file.Length > maxFileSize)
                throw new ArgumentException("The image cannot exceed 5 MB.");

            var extension = file.ContentType switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/webp" => ".webp",
                _ => throw new ArgumentException("Unsupported image format.")
            };

            var recipe = await _dbContext.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe is null)
                throw new KeyNotFoundException($"Recipe with ID {recipeId} was not found.");

            var directory = Path.Combine(_environment.WebRootPath, "uploads", "recipes");

            Directory.CreateDirectory(directory);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var physicalPath = Path.Combine(directory, fileName);

            await using (var stream = File.Create(physicalPath))
            {
                await file.CopyToAsync(stream);
            }

            DeletePreviousImage(recipe.ImagePath);

            recipe.ImagePath = $"/uploads/recipes/{fileName}";

            await _dbContext.SaveChangesAsync();

            return recipe.ImagePath;

        }

        private void DeletePreviousImage(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            var uploadPrefix = "/uploads/recipes/";

            if (!imagePath.StartsWith(uploadPrefix, StringComparison.OrdinalIgnoreCase))
                return;

            var relativePath = imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);

            var physicalPath = Path.Combine(_environment.WebRootPath, relativePath);

            if (File.Exists(physicalPath))
                File.Delete(physicalPath);
        }

        public async Task<bool> MarkAsVisited(int id)
        {
            var recipe = await _dbContext.Recipes.FindAsync(id);

            if(recipe == null)
                return false;

            recipe.LastVisitedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> Delete(int id)
        {
            var recipe = await _dbContext.Recipes.FindAsync(id);

            if(recipe == null)
                return false;

            _dbContext.Recipes.Remove(recipe);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
