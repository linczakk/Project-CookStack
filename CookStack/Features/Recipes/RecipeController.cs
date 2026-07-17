using CookStack.Shared.Recipes.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CookStack.Api.Features.Recipes
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeListDto>>> GetRecipeList([FromQuery] string? search)
        {
            var result = await _recipeService.GetAll(search);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDetailsDto>> GetRecipe(int id)
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
            return CreatedAtAction(nameof(GetRecipe), new { id }, id);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDto dto)
        {
            var result = await _recipeService.Update(id, dto);

            if (result)
                return NoContent();

            return NotFound();
        }

        [HttpPost("{id:int}/image")]
        public async Task<ActionResult<string>> UploadImage(int id, IFormFile file)
        {
            try
            {
                var imagePath = await _recipeService.UploadImage(id, file);

                return Ok(new RecipeImageResponseDto
                {
                    ImagePath = imagePath
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/visit")]
        public async Task<IActionResult> MarkRecipeAsVisited(int id)
        {
            var result = await _recipeService.MarkAsVisited(id);

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
