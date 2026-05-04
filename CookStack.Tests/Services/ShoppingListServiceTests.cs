using CookStack.Api.Data;
using CookStack.Api.Features.ShoppingList;
using CookStack.Shared.ShoppingList.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CookStack.Tests.Services
{
    public class ShoppingListServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ShoppingListServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);


        [Fact]
        public async Task GetAll_Should_ReturnAllShoppingLists()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var now = DateTime.UtcNow;

            ShoppingList[] shoppingLists =
            [
                new ShoppingList
                {
                    Title = "Test Title 1",
                    CreatedAt = now,
                },
                new ShoppingList
                {
                    Title = "Test Title 2",
                    CreatedAt = now,
                }
            ];

            await db.ShoppingLists.AddRangeAsync(shoppingLists);
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
        public async Task GetById_Should_ReturnShoppingList()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var shoppingList = new ShoppingList
            {
                Title = "Test Title",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem
                    {
                        Name = "Test Item 1",
                    },
                    new ShoppingItem
                    {
                        Name = "Test Item 2"
                    }
                },
            };

            await db.ShoppingLists.AddAsync(shoppingList);
            await db.SaveChangesAsync();

            var result = await service.GetById(shoppingList.Id);

            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
            Assert.Equal(2, result.Items.Count());
            Assert.All(result.Items, i => Assert.False(string.IsNullOrEmpty(i.Name)));
        }

        [Fact]
        public async Task GetById_Should_ReturnNull_WhenNotFound()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var result = await service.GetById(0);

            Assert.Null(result);
        }

        [Fact]
        public async Task Create_Should_CreateShoppingList()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var shoppingListDto = new CreateShoppingListDto
            {
                Title = "Test Title",
                Items = new List<ShoppingItemDto>
                {
                    new ShoppingItemDto
                    {
                        Name = "Test Item 1",
                    },
                    new ShoppingItemDto
                    {
                        Name = "Test Item 2"
                    }
                }
            };

            var success = await service.Create(shoppingListDto);

            var result = db.ShoppingLists.SingleOrDefault(s => s.Title == "Test Title");

            Assert.NotNull(result);
            Assert.True(success > 0);
            Assert.Equal(2, result.Items.Count());
            Assert.All(result.Items, i => Assert.False(string.IsNullOrEmpty(i.Name)));
        }

        [Fact]
        public async Task CreateFromRecipe_Should_CreateNewShoppingListFromIngredients()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var shoppingListDto = new AddIngredientsToShoppingListDto
            {
                Title = "Test Title",
                Items = new List<ShoppingItemDto>
                {
                    new ShoppingItemDto
                    {
                        Name = "Test Item 1",
                    },
                    new ShoppingItemDto
                    {
                        Name = "Test Item 2",
                    }
                }
            };

            var success = await service.CreateFromRecipe(shoppingListDto);

            var result = db.ShoppingLists.SingleOrDefault(s => s.Title == "Test Title");

            Assert.NotNull(result);
            Assert.True(success > 0);
            Assert.Equal(2, result.Items.Count());
            Assert.All(result.Items, i => Assert.False(string.IsNullOrEmpty(i.Name)));
        }

        [Fact]
        public async Task CreateFromRecipe_Should_CreateNewShoppingListWithUniqueTitle()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var shoppingList = new ShoppingList
            {
                Title = "Test Title",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem
                    {
                        Name = "Test Item 1",
                    },
                },
            };

            await db.ShoppingLists.AddAsync(shoppingList);
            await db.SaveChangesAsync();

            var shoppingListDto = new AddIngredientsToShoppingListDto
            {
                Title = shoppingList.Title,
                Items = shoppingList.Items.Select(i => new ShoppingItemDto
                {
                    Name = i.Name,
                }).ToList(),
            };

            var success = await service.CreateFromRecipe(shoppingListDto);

            var result = db.ShoppingLists;

            Assert.NotNull(result);
            Assert.True(success > 0);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Title == "Test Title (2)");
        }

        [Fact]
        public async Task AddToExisting_Should_AddNewIngredientsToExistingShoppingList()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var shoppingList = new ShoppingList
            {
                Title = "Test Title",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem
                    {
                        Name = "Test Item 1",
                    },
                },
            };

            await db.ShoppingLists.AddAsync(shoppingList);
            await db.SaveChangesAsync();

            var shoppingListDto = new AddIngredientsToShoppingListDto
            {
                ExistingListId = shoppingList.Id,
                Items = new List<ShoppingItemDto>
                {
                    new ShoppingItemDto
                    {
                        Name = "Test Item 2",
                    }
                }
            };

            var success = await service.AddToExisting(shoppingListDto);

            var result = db.ShoppingLists.SingleOrDefault(s => s.Title == "Test Title");

            Assert.NotNull(result);
            Assert.True(success > 0);
            Assert.Equal(2, result.Items.Count());
            Assert.All(result.Items, i => Assert.False(string.IsNullOrEmpty(i.Name)));
        }


        [Fact]
        public async Task AddToExisting_Should_ReturnNull_WhenListDoesNotExist()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var shoppingListDto = new AddIngredientsToShoppingListDto
            {
                ExistingListId = 999
            };

            var success = await service.AddToExisting(shoppingListDto);

            Assert.Null(success);
        }
                       

        [Fact]
        public async Task Update_Should_UpdateShoppingList()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var shoppingList = new ShoppingList
            {
                Title = "Test Title",
                Description = "Test Description",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem
                    {
                        Name = "Test Item 1",
                    },
                    new ShoppingItem
                    {
                        Name = "Test Item 2",
                    }
                },
            };

            await db.ShoppingLists.AddAsync(shoppingList);
            await db.SaveChangesAsync();

            var shoppingListDto = new ShoppingListUpdateDto
            {
                Title = shoppingList.Title,
                Description = "Updated Test Description",
                Items = shoppingList.Items.Select(i => new ShoppingItemDto
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit =  i.Unit
                }).ToList()
            };

            shoppingListDto.Items.Add(new ShoppingItemDto
            {
                Name = "Test Item 3"
            });

            var success = await service.Update(shoppingList.Id, shoppingListDto);

            var result = db.ShoppingLists.Find(shoppingList.Id);

            Assert.NotNull(result);
            Assert.True(success);
            Assert.Equal("Updated Test Description", result.Description);
            Assert.Equal(3, result.Items.Count());
            Assert.Contains(result.Items, i => i.Name == "Test Item 3");
        }

        [Fact]
        public async Task Update_Should_ReturnFalse_WhenListNotFound()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var result = await service.Update(0, new ShoppingListUpdateDto());

            Assert.False(result);
            Assert.Empty(db.Recipes);
        }


        [Fact]
        public async Task Delete_Should_DeleteShoppingList()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var shoppingList = new ShoppingList
            {
                Title = "Test Title",
                Description = "Test Description",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem
                    {
                        Name = "Test Item 1",
                        Quantity = 1,
                        Unit = Shared.Enums.UnitType.Gram,
                    },
                },
            };

            await db.ShoppingLists.AddAsync(shoppingList);
            await db.SaveChangesAsync();

            var success = await service.Delete(shoppingList.Id);

            var result = db.ShoppingLists.Find(shoppingList.Id);

            Assert.True(success);
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_Should_ReturnFalse_WhenListNotFound()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var result = await service.Delete(0);

            Assert.False(result);
            Assert.Empty(db.Recipes);
        }
    }
}
