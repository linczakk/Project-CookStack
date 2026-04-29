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
        public async Task Create_Should_CreateShoppingList()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var shoppingListDto = new CreateShoppingListDto
            {
                Title = "Test Title",
                Description = "Test Description",
                Items = new List<ShoppingItemDto>
                {
                    new ShoppingItemDto
                    {
                        Name = "Test Item 1",
                        Quantity = 1,
                        Unit = Shared.Enums.UnitType.Gram,
                    }
                }
            };

            await service.Create(shoppingListDto);

            var result = db.ShoppingLists.SingleOrDefault(s => s.Title == "Test Title");

            Assert.NotNull(result);
            Assert.Equal("Test Description", result.Description);
            Assert.Contains(result.Items, i => i.Name == "Test Item 1");
        }

        [Fact]
        public async Task CreateFromRecipe_Should_CreateNewShoppingListFromIngredients()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var shoppingListDto = new AddIngredientsToShoppingListDto
            {
                Title = "Test Title",
                Description = "Test Description",
                Items = new List<ShoppingItemDto>
                {
                    new ShoppingItemDto
                    {
                        Name = "Test Item 1",
                        Quantity = 1,
                        Unit = Shared.Enums.UnitType.Gram,
                    }
                }
            };

            await service.CreateFromRecipe(shoppingListDto);

            var result = db.ShoppingLists.SingleOrDefault(s => s.Title == "Test Title");

            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
            Assert.Contains(result.Items, i => i.Name == "Test Item 1");
        }

        [Fact]
        public async Task CreateFromRecipe_Should_CreateNewShoppingListWithUniqueTitle()
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

            var shoppingListDto = new AddIngredientsToShoppingListDto
            {
                Title = shoppingList.Title,
                Description = shoppingList.Description,
                Items = shoppingList.Items.Select(i => new ShoppingItemDto
                {
                    Name = i.Name,
                    Quantity= i.Quantity,
                    Unit = i.Unit,
                }).ToList(),
            };

            await service.CreateFromRecipe(shoppingListDto);

            var result = db.ShoppingLists;

            Assert.NotNull(result);
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

            var shoppingListDto = new AddIngredientsToShoppingListDto
            {
                ExistingListId = shoppingList.Id,
                Items = new List<ShoppingItemDto>
                {
                    new ShoppingItemDto
                    {
                        Name = "Test Item 2",
                        Quantity = 1,
                        Unit = Shared.Enums.UnitType.Gram,
                    }
                }
            };

            await service.AddToExisting(shoppingListDto);

            var result = db.ShoppingLists.SingleOrDefault(s => s.Title == "Test Title");

            Assert.NotNull(result);
            Assert.Contains(result.Items, i => i.Name == "Test Item 1");
            Assert.Contains(result.Items, i => i.Name == "Test Item 2");
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

            var result = await service.AddToExisting(shoppingListDto);

            Assert.Null(result);
        }


        [Fact]
        public async Task GetById_Should_ReturnShoppingList()
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

            var result = await service.GetById(shoppingList.Id);

            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
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
        public async Task GetAll_Should_ReturnAllShoppingLists()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            ShoppingList[] shoppingLists = 
            [ 
                new ShoppingList
                {
                    Title = "Test Title 1",
                    Description = "Test Description 1",
                    Items = new List<ShoppingItem>
                    {
                        new ShoppingItem
                        {
                            Name = "Test Item 1",
                            Quantity = 1,
                            Unit = Shared.Enums.UnitType.Gram,
                        },
                    },
                },
                new ShoppingList
                {
                    Title = "Test Title 2",
                    Description = "Test Description 2",
                    Items = new List<ShoppingItem>
                    {
                        new ShoppingItem
                        {
                            Name = "Test Item 2",
                            Quantity = 1,
                            Unit = Shared.Enums.UnitType.Kilogram,
                        },
                    },
                }
            ];

            await db.ShoppingLists.AddRangeAsync(shoppingLists);
            await db.SaveChangesAsync();

            var result = await service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Title == "Test Title 1");
            Assert.Contains(result, r => r.Title == "Test Title 2");
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
                        Quantity = 1,
                        Unit = Shared.Enums.UnitType.Gram,
                    },
                },
            };

            await db.ShoppingLists.AddAsync(shoppingList);
            await db.SaveChangesAsync();

            var shoppingListDto = new ShoppingListUpdateDto
            {
                Title = shoppingList.Title,
                Description = "Updated Description",
                Items = shoppingList.Items.Select(i => new ShoppingItemDto
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit =  i.Unit
                }).ToList()
            };
            var success = await service.Update(shoppingList.Id, shoppingListDto);

            var result = db.ShoppingLists.Find(shoppingList.Id);

            Assert.NotNull(result);
            Assert.True(success);
            Assert.Equal("Updated Description", result.Description);
        }

        [Fact]
        public async Task Update_Should_ReturnFalse_WhenListNotFound()
        {
            var db = CreateDbContext();
            var service = new ShoppingListService(db);

            var result = await service.Update(0, new ShoppingListUpdateDto());

            Assert.False(result);
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
        }
    }
}
