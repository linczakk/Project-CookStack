using CookStackApi.Data;
using CookStackShared.ShoppingList.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookStackApi.Features.ShoppingList
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingListController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingListController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingListsListDto>>> GetShoppingLists()
        {
            var shoppingList = await _dbContext.ShoppingLists
                .Select(s => new ShoppingListsListDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    CreatedAt = s.CreatedAt
                })
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return Ok(shoppingList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingListDetailsDto>> GetShoppingList(int id)
        {
            var shoppingList = await _dbContext.ShoppingLists
                .Where(s => s.Id == id)
                .Select(s => new ShoppingListDetailsDto
                {
                    Title = s.Title,
                    Description = s.Description,
                    Items = s.Items
                    .Select(si => new ShoppingItemDto
                    {
                        Name = si.Name,
                        Quantity = si.Quantity,
                        Unit = si.Unit
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            if (shoppingList == null)
            {
                return NotFound();
            }

            return Ok(shoppingList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShoppingList([FromBody] CreateShoppingListDto dto)
        {
            var shoppingList = new ShoppingList
            {
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = dto.CreateAt,
                Items = dto.Items.Select(i => new ShoppingItem
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit
                }).ToList()
            };

            _dbContext.ShoppingLists.Add(shoppingList);

            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetShoppingList), new { id = shoppingList.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShoppingList(int id, [FromBody] ShoppingListUpdateDto dto)
        {
            var shoppingList = await _dbContext.ShoppingLists
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shoppingList == null)
            {
                return NotFound();
            }

            shoppingList.Id = dto.Id;
            shoppingList.Title = dto.Title;
            shoppingList.Description = dto.Description;
            shoppingList.CreatedAt = dto.CreateAt;

            _dbContext.ShoppingItems.RemoveRange(shoppingList.Items);

            shoppingList.Items = dto.Items
                .Select(i => new ShoppingItem
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit
                }).ToList();

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingList(int id)
        {
            var shoppingList = await _dbContext.ShoppingLists.FindAsync(id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            _dbContext.ShoppingLists.Remove(shoppingList);

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
