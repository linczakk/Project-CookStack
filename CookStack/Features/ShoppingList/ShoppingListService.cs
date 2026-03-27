using CoockStackShared.ShoppingList.Dtos;
using CookStackApi.Data;
using CookStackShared.ShoppingList.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CookStackApi.Features.ShoppingList
{
    public class ShoppingListService
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingListService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<ShoppingListsListDto>> GetAll()
        {
            var shoppingList = await _dbContext.ShoppingLists
                .Select(s => new ShoppingListsListDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    CreatedAt = s.CreatedAt,
                    TotalItems = s.Items.Count(),
                    CompletedItems = s.Items.Count(i => i.IsChecked)
                })
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return shoppingList;
        }

        public async Task<ShoppingListDetailsDto?> GetById(int id)
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
                       Id = si.Id,
                       Name = si.Name,
                       Quantity = si.Quantity,
                       Unit = si.Unit,
                       IsChecked = si.IsChecked,
                       Order = si.Order
                   })
                   .ToList()
               })
               .FirstOrDefaultAsync();

            return shoppingList;
        }

        public async Task<ShoppingList> Create(CreateShoppingListDto dto)
        {
            var shoppingList = new ShoppingList
            {
                Title = await GenerateUniqueTitle(dto.Title),
                Description = dto.Description,
                Items = dto.Items.Select(si => new ShoppingItem
                {
                    Name = si.Name,
                    Quantity = si.Quantity,
                    Unit = si.Unit,
                    IsChecked = si.IsChecked,
                    Order = si.Order
                }).ToList()

            };

            _dbContext.ShoppingLists.Add(shoppingList);
            await _dbContext.SaveChangesAsync();

            return shoppingList;
        }

        public async Task<ShoppingList> CreateFromRecipe(ShoppingListFromRecipeDto dto)
        {
            var shoppingList = new ShoppingList
            {
                Title = await GenerateUniqueTitle(dto.Title),
                Description = dto.Description,
                Items = dto.Items.Select((si, index) => new ShoppingItem
                {
                    Name = si.Name,
                    Quantity = si.Quantity,
                    Unit = si.Unit,
                    IsChecked = false,
                    Order = index

                }).ToList()
            };

            _dbContext.ShoppingLists.Add(shoppingList);
            await _dbContext.SaveChangesAsync();
            return shoppingList;
        }

        private async Task<string> GenerateUniqueTitle(string baseTitle)
        {
            var existingTitles = await _dbContext.ShoppingLists
                .Where(s => s.Title.StartsWith(baseTitle))
                .Select(s => s.Title)
                .ToListAsync();

            if (!existingTitles.Contains(baseTitle))
                return baseTitle;

            int counter = 2;
            string newTitle;

            do
            {
                newTitle = $"{baseTitle} ({counter})";
                counter++;
            }
            while (existingTitles.Contains(newTitle));

            return newTitle;
        }


        public async Task<bool> Update(int id, ShoppingListUpdateDto dto)
        {
            var itemFound = false;
            var shoppingList = await _dbContext.ShoppingLists
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);

            if(shoppingList != null)
            {
                itemFound = true;
                shoppingList.Title = dto.Title;
                shoppingList.Description = dto.Description;

                _dbContext.ShoppingItems.RemoveRange(shoppingList.Items);

                shoppingList.Items = dto.Items
                    .Select(si => new ShoppingItem
                    {
                        Name = si.Name,
                        Quantity = si.Quantity,
                        Unit = si.Unit,
                        IsChecked = si.IsChecked,
                        Order = si.Order
                    }).ToList();

                await _dbContext.SaveChangesAsync();
            }
            return itemFound;
        }

        public async Task<bool> Delete(int id)
        {
            var itemFound = false;
            var shoppingList = await _dbContext.ShoppingLists.FindAsync(id);

            if(shoppingList != null)
            {
                itemFound = true;

                _dbContext.ShoppingLists.Remove(shoppingList);
                await _dbContext.SaveChangesAsync();
            }
            return itemFound;
        }
    }
}
