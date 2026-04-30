using CookStack.Api.Data;
using CookStack.Shared.Recipes.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookStack.Api.Features.Recipes
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipesService _recipeService;

        public RecipesController(IRecipesService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeListDto>>> GetRecipesList()
        {
            var result = await _recipeService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDetailsDto>> GetRecipeDetails(int id)
        {
            var result = await _recipeService.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDto dto)
        {
            var id = await _recipeService.Create(dto);
            return CreatedAtAction(nameof(GetRecipeDetails), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDto dto)
        {
            var result = await _recipeService.Update(id, dto);

            if (result)
                return NoContent();

            return NotFound();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var result = await _recipeService.Delete(id);

            if (result)
                return NoContent();

            return NotFound();
        }
    }
}
