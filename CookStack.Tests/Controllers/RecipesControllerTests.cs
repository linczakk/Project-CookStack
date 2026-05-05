using CookStack.Api.Features.Recipes;
using CookStack.Shared.Recipes.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CookStack.Tests.Controllers
{
    public class RecipesControllerTests
    {

        [Fact]
        public async Task GetRecipesList_Should_ReturnOk()
        {
            var mockService = new Mock<IRecipeService>();

            mockService
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<RecipeListDto>
                {
                    new() { Title = "Test Title 1"},
                    new() { Title = "Test Title 2"}
                });

            var controller = new RecipeController (mockService.Object);

            var result = await controller.GetRecipesList();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<RecipeListDto>>(okResult.Value);

            Assert.Equal(2, value.Count());
            Assert.Contains(value, v => v.Title == "Test Title 1");
            Assert.Contains(value, v => v.Title == "Test Title 2");
        }

        [Fact]
        public async Task GetRecipe_Should_ReturnOk_WhenRecipeExist()
        {
            var mockService = new Mock<IRecipeService>();

            mockService
                .Setup(s => s.GetById(1))
                .ReturnsAsync(new RecipeDetailsDto { Title = "Test Title" });

            var controller = new RecipeController(mockService.Object);

            var result = await controller.GetRecipe(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<RecipeDetailsDto>(okResult.Value);

            Assert.Equal("Test Title", value.Title);
        }

        [Fact]
        public async Task GetRecipe_Should_ReturnNotFound_WhenRecipeDoesNotExist()
        {
            var mockService = new Mock<IRecipeService>();

            mockService
                .Setup(s => s.GetById(1))
                .ReturnsAsync((RecipeDetailsDto?)null);

            var controller = new RecipeController(mockService.Object);

            var result = await controller.GetRecipe(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateRecipe_Should_ReturnCreatedAtAction()
        {
            var mockService = new Mock<IRecipeService>();

            mockService
                .Setup(s => s.Create(It.Is<CreateRecipeDto>(d => d.Title == "Test Title")))
                .ReturnsAsync(123);

            var controller = new RecipeController(mockService.Object);

            var dto = new CreateRecipeDto
            {
                Title = "Test Title"
            };

            var result = await controller.CreateRecipe(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(RecipeController.GetRecipe), createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.Equal(123, createdResult.RouteValues["id"]);

        }

        [Fact]
        public async Task UpdateRecipe_Should_ReturnNoContent_WhenUpdateIsSuccessful()
        {
            var mockService = new Mock<IRecipeService>();

            mockService
                .Setup(s => s.Update(1, It.Is<RecipeUpdateDto>(d => d.Title == "Test Title")))
                .ReturnsAsync(true);

            var controller = new RecipeController(mockService.Object);

            var dto = new RecipeUpdateDto
            {
                Title = "Test Title"
            };

            var result = await controller.UpdateRecipe(1, dto);

            Assert.IsType<NoContentResult>(result);

            mockService.Verify(
                s => s.Update(1, It.Is<RecipeUpdateDto>(d => d.Title == "Test Title")),
                Times.Once);
        }

        [Fact]
        public async Task UpdateRecipe_Should_ReturnNotFound_WhenUpdateIsUnsuccessful()
        {
            var mockService = new Mock<IRecipeService>();

            mockService
                .Setup(s => s.Update(1, It.Is<RecipeUpdateDto>(d => d.Title == "Test Title")))
                .ReturnsAsync(false);

            var controller = new RecipeController(mockService.Object);

            var dto = new RecipeUpdateDto
            {
                Title = "Test Title"
            };

            var result = await controller.UpdateRecipe(1, dto);

            Assert.IsType<NotFoundResult>(result);

            mockService.Verify(
                s => s.Update(1, It.Is<RecipeUpdateDto>(d => d.Title == "Test Title")),
                Times.Once);
        }

        [Fact]
        public async Task DeleteRecipe_Should_ReturnNoContent_WhenDeleteIsSuccessful()
        {
            var mockService = new Mock<IRecipeService>();

            mockService
                .Setup(s => s.Delete(1))
                .ReturnsAsync(true);

            var controller = new RecipeController(mockService.Object);

            var result = await controller.DeleteRecipe(1);

            Assert.IsType<NoContentResult>(result);

            mockService.Verify(
                s => s.Delete(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteRecipe_Should_ReturnNotFound_WhenDeleteIsUnsuccessful()
        {
            var mockService = new Mock<IRecipeService>();

            mockService
                .Setup(s => s.Delete(1))
                .ReturnsAsync(false);

            var controller = new RecipeController(mockService.Object);

            var result = await controller.DeleteRecipe(1);

            Assert.IsType<NotFoundResult>(result);

            mockService.Verify(
                s => s.Delete(1),
                Times.Once);
        }
    }
}
