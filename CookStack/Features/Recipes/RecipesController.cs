using CookStackApi.Data;
using CookStackShared.Recipes.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookStackApi.Features.Recipes
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public RecipesController(ApplicationDbContext context)
        {
            _dbContext = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeListDto>>> GetRecipesList()
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

            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDetailsDto>> GetRecipeDetails(int id)
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


            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDto dto)
        {
            var recipe = new Recipe
            {
                Id = 0,
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
                Steps = dto.Steps.Select((s, index) => new RecipeStep
                {
                    Order = index + 1,
                    Description = s.Description
                })
                .ToList()
            };

            _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecipeDetails), new { id = recipe.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDto dto)
        {
            var recipe = await _dbContext.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

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

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _dbContext.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _dbContext.Recipes.Remove(recipe);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
