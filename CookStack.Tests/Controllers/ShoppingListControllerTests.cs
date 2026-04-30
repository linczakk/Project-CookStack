using CookStack.Api.Features.ShoppingList;
using CookStack.Shared.ShoppingList.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CookStack.Tests.Controllers
{

    public class ShoppingListControllerTests
    {
        [Fact]
        public async Task GetShoppingLists_Should_ReturnOk()
        {
            var mockService = new Mock<IShoppingListService>();

            mockService
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<ShoppingListsListDto>
                {
                    new() { Title = "Test Title 1"},
                    new() { Title = "Test Title 2"}
                });

            var controller = new ShoppingListController(mockService.Object);

            var result = await controller.GetShoppingLists();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<ShoppingListsListDto>>(okResult.Value);

            Assert.Equal(2, value.Count());
            Assert.Contains(value, v => v.Title == "Test Title 1");
            Assert.Contains(value, v => v.Title == "Test Title 2");
        }


        [Fact]
        public async Task GetShoppingList_Should_ReturnOk_WhenListExist()
        {
            var mockService = new Mock<IShoppingListService>();

            mockService
                .Setup(s => s.GetById(1))
                .ReturnsAsync(new ShoppingListDetailsDto { Title = "Test Title" });

            var controller = new ShoppingListController(mockService.Object);

            var result = await controller.GetShoppingList(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<ShoppingListDetailsDto>(okResult.Value);

            Assert.Equal("Test Title", value.Title);
        }

        [Fact]
        public async Task GetShoppingList_Should_ReturnNotFound_WhenListDoesNotExist()
        {
            var mockService = new Mock<IShoppingListService>();

            mockService
                .Setup(s => s.GetById(1))
                .ReturnsAsync((ShoppingListDetailsDto?)null);

            var controller = new ShoppingListController(mockService.Object);

            var result = await controller.GetShoppingList(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task CreateShoppingList_Should_ReturnCreatedAtAction()
        {
            var mockService = new Mock<IShoppingListService>();

            mockService
                .Setup(s => s.Create(It.Is<CreateShoppingListDto>(d => d.Title == "Test Title")))
                .ReturnsAsync(123);

            var controller = new ShoppingListController(mockService.Object);

            var dto = new CreateShoppingListDto
            {
                Title = "Test Title"
            };

            var result = await controller.CreateShoppingList(dto);

            var createdResult = 
                Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ShoppingListController.GetShoppingList), createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.Equal(123, createdResult.RouteValues["id"]);
        }

        [Fact]
        public async Task AddIngredients_Should_ReturnCreatedAtAction_WhenCreatingNewList()
        {
            var mockService = new Mock<IShoppingListService>();
            mockService
                .Setup(s => s.CreateFromRecipe(It.IsAny<AddIngredientsToShoppingListDto>()))
                .ReturnsAsync(456);

            var controller = new ShoppingListController(mockService.Object);

            var dto = new AddIngredientsToShoppingListDto
            {
                ExistingListId = null,
                Items = new List<ShoppingItemDto>
                {
                    new ShoppingItemDto()
                },
            };

            var result = await controller.AddIngredients(dto);

            var createdResult = 
                Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ShoppingListController.GetShoppingList), createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.Equal(456, createdResult.RouteValues["id"]);

            mockService.Verify(
                s => s.AddToExisting(It.IsAny<AddIngredientsToShoppingListDto>()),
                Times.Never);
        }

        [Fact]
        public async Task AddIngredients_Should_ReturnBadRequest_WhenItemsIsNull()
        {
            var mockService = new Mock<IShoppingListService>();
            var controller = new ShoppingListController(mockService.Object);
            var dto = new AddIngredientsToShoppingListDto();

            var result = await controller.AddIngredients(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No items provided", badRequestResult.Value);

            mockService.Verify(
                s => s.CreateFromRecipe(It.IsAny<AddIngredientsToShoppingListDto>()),
                Times.Never);
        }

        [Fact]
        public async Task AddIngredients_Should_ReturnBadRequest_WhenItemsIsEmpty()
        {
            var mockService = new Mock<IShoppingListService>();
            var controller = new ShoppingListController(mockService.Object);
            var dto = new AddIngredientsToShoppingListDto()
            {
                Items = new List<ShoppingItemDto>()
            };

            var result = await controller.AddIngredients(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No items provided", badRequestResult.Value);

            mockService.Verify(
                s => s.CreateFromRecipe(It.IsAny<AddIngredientsToShoppingListDto>()),
                Times.Never);
        }

        [Fact]
        public async Task AddIngredients_Should_ReturnOk_WhenAddingToExistingList()
        {
            var mockService = new Mock<IShoppingListService>();
            mockService
                .Setup(s => s.AddToExisting(It.IsAny<AddIngredientsToShoppingListDto>()))
                .ReturnsAsync(123);

            var controller = new ShoppingListController(mockService.Object);

            var dto = new AddIngredientsToShoppingListDto
            {
                ExistingListId = 1,
                Items = new List<ShoppingItemDto>
                {
                    new ShoppingItemDto()
                },
            };

            var result = await controller.AddIngredients(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(123, okResult.Value);

            mockService.Verify(
                s => s.CreateFromRecipe(It.IsAny<AddIngredientsToShoppingListDto>()),
                Times.Never);
        }

        [Fact]
        public async Task AddIngredients_Should_ReturnNotFound_WhenListDoesNotExist()
        {
            var mockService = new Mock<IShoppingListService>();
            mockService
                .Setup(s => s.AddToExisting(It.IsAny<AddIngredientsToShoppingListDto>()))
                .ReturnsAsync((int?)null);

            var controller = new ShoppingListController(mockService.Object);

            var dto = new AddIngredientsToShoppingListDto
            {
                ExistingListId = 1,
                Items = new List<ShoppingItemDto>
                {
                    new ShoppingItemDto()
                },
            };

            var result = await controller.AddIngredients(dto);

            Assert.IsType<NotFoundResult>(result);

            mockService.Verify(
                s => s.AddToExisting(It.IsAny<AddIngredientsToShoppingListDto>()),
                Times.Once);

            mockService.Verify(
                s => s.CreateFromRecipe(It.IsAny<AddIngredientsToShoppingListDto>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateShoppingList_Should_ReturnNoContent_WhenUpdateIsSuccessful()
        {
            var mockService = new Mock<IShoppingListService>();
            mockService
                .Setup(s => s.Update(1, It.Is<ShoppingListUpdateDto>(d => d.Title == "Test Title")))
                .ReturnsAsync(true);

            var controller = new ShoppingListController(mockService.Object);

            var dto = new ShoppingListUpdateDto()
            {
                Title = "Test Title"
            };

            var result = await controller.UpdateShoppingList(1, dto);

            Assert.IsType<NoContentResult>(result);

            mockService.Verify(
                s => s.Update(1, It.Is<ShoppingListUpdateDto>(d => d.Title == "Test Title")),
                Times.Once);
        }

        [Fact]
        public async Task UpdateShoppingList_Should_ReturnNotFound_WhenUpdateIsUnsuccessful()
        {
            var mockService = new Mock<IShoppingListService>();
            mockService
                .Setup(s => s.Update(1, It.Is<ShoppingListUpdateDto>(d => d.Title == "Test Title")))
                .ReturnsAsync(false);

            var controller = new ShoppingListController(mockService.Object);

            var dto = new ShoppingListUpdateDto()
            {
                Title = "Test Title"
            };

            var result = await controller.UpdateShoppingList(1, dto);

            Assert.IsType<NotFoundResult>(result);

            mockService.Verify(
                s => s.Update(1, It.Is<ShoppingListUpdateDto>(d => d.Title == "Test Title")),
                Times.Once);
        }

        [Fact]
        public async Task DeleteShoppingList_Should_ReturnNoContent_WhenDeleteIsSuccessful()
        {
            var mockService = new Mock<IShoppingListService>();
            mockService
                .Setup(s => s.Delete(1))
                .ReturnsAsync(true);

            var controller = new ShoppingListController(mockService.Object);

            var result = await controller.DeleteShoppingList(1);

            Assert.IsType<NoContentResult>(result);

            mockService.Verify(
                s => s.Delete(1), 
                Times.Once);
        }

        [Fact]
        public async Task DeleteShoppingList_Should_ReturnNotFound_WhenDeleteIsUnsuccessful()
        {
            var mockService = new Mock<IShoppingListService>();
            mockService
                .Setup(s => s.Delete(1))
                .ReturnsAsync(false);

            var controller = new ShoppingListController(mockService.Object);

            var result = await controller.DeleteShoppingList(1);

            Assert.IsType<NotFoundResult>(result);

            mockService.Verify(
                s => s.Delete(1),
                Times.Once);
        }
    }
}
