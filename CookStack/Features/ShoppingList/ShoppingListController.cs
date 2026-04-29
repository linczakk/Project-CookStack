using CookStack.Shared.ShoppingList.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CookStack.Api.Features.ShoppingList
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingListsListDto>>> GetShoppingLists()
        {
            var result = await _shoppingListService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingListDetailsDto>> GetShoppingList(int id)
        {
            var result = await _shoppingListService.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShoppingList([FromBody] CreateShoppingListDto dto)
        {
            var id = await _shoppingListService.Create(dto);
            return CreatedAtAction(nameof(GetShoppingList), new { id }, null);
        }

        [HttpPost("add-ingredients")]
        public async Task<IActionResult> AddIngredients([FromBody] AddIngredientsToShoppingListDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                return BadRequest("No items provided");

            if (dto.ExistingListId.HasValue)
            {
               var id = await _shoppingListService.AddToExisting(dto);
                if (id == null)
                    return NotFound();

                return Ok(id);
            }

            var createdId = await _shoppingListService.CreateFromRecipe(dto);

            return CreatedAtAction(nameof(GetShoppingList), new { id = createdId }, null);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShoppingList(int id, [FromBody] ShoppingListUpdateDto dto)
        {
            var result = await _shoppingListService.Update(id, dto);

            if (result)
                return NoContent();
            
            return NotFound();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingList(int id)
        {
            var result = await _shoppingListService.Delete(id);
            
            if (result)
                return NoContent();

            return NotFound();
        }
    }
}
