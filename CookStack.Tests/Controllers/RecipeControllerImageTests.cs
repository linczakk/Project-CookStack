using CookStack.Api.Features.Recipes;
using CookStack.Shared.Recipes.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookStack.Tests.Controllers
{
    public sealed class RecipeControllerImageTests
    {
        private readonly Mock<IRecipeService> _recipeServiceMock;
        private readonly RecipeController _controller;

        public RecipeControllerImageTests()
        {
            _recipeServiceMock = new Mock<IRecipeService>();

            _controller = new RecipeController(_recipeServiceMock.Object);
        }

        [Fact]
        public async Task UploadImage_ServiceSucceds_ReturnsOkwithImagePath()
        {
            const int recipeId = 15;
            const string expectedImagePath = "/uploads/recipes/test.jpg";

            var file = CreateFile();

            _recipeServiceMock
                .Setup(service => service.UploadImage(recipeId, file))
                .ReturnsAsync(expectedImagePath);

            var response = await _controller.UploadImage(recipeId, file);

            var result = Assert.IsType<OkObjectResult>(response.Result);

            var dto = Assert.IsType<RecipeImageResponseDto>(result.Value);

            Assert.Equal(expectedImagePath, dto.ImagePath);

            _recipeServiceMock.Verify(service => service.UploadImage(recipeId, file), Times.Once);
        }

        [Fact]
        public async Task UploadImage_RecipeDoesNotExist_ReturnsNotFound()
        {
            const int recipeId = 999;
            var file = CreateFile();

            _recipeServiceMock
                .Setup(service => service.UploadImage(recipeId, file))
                .ThrowsAsync(new KeyNotFoundException());

            var response = await _controller.UploadImage(recipeId, file);

            Assert.IsType<NotFoundResult>(response.Result);

            _recipeServiceMock.Verify(service => service.UploadImage(recipeId, file), Times.Once);
        }

        [Fact]
        public async Task UploadImage_InvalidFile_ReturnsBadRequest()
        {
            const int recipeId = 15;
            const string expectedMessage = "Unsupported image format.";

            var file = CreateFile();

            _recipeServiceMock
                .Setup(service => service.UploadImage(recipeId, file))
                .ThrowsAsync(new ArgumentException(expectedMessage));

            var response = await _controller.UploadImage(recipeId, file);

            var result = Assert.IsType<BadRequestObjectResult>(response.Result);

            Assert.Equal(expectedMessage, result.Value);

            _recipeServiceMock.Verify(service => service.UploadImage(recipeId, file), Times.Once);
        }

        [Fact]
        public async Task UploadImage_UnexpectedException_PropagatesException()
        {
            const int recipeId = 15;
            var file = CreateFile();

            _recipeServiceMock
                .Setup(service => service.UploadImage(recipeId, file))
                .ThrowsAsync(new InvalidOperationException("Unexpected failure"));

            var action = () => _controller.UploadImage(recipeId, file);

            await Assert.ThrowsAsync<InvalidOperationException>(action);

            _recipeServiceMock.Verify(service => service.UploadImage(recipeId, file), Times.Once);
        }

        private static IFormFile CreateFile()
        {
            var stream = new MemoryStream([1, 2, 3]);

            return new FormFile(stream, 0, stream.Length, "file", "recipe.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
        }
    }
}
