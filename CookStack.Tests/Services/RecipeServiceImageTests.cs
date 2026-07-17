using CookStack.Api.Data;
using CookStack.Api.Features.Recipes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;

namespace CookStack.Tests.Services
{
    public sealed class RecipeServiceImageTests : IDisposable
    {

        private readonly SqliteConnection _connection;
        private readonly ApplicationDbContext _dbContext;
        private readonly string _temporaryWebRoot;
        private readonly RecipeService _service;

        public RecipeServiceImageTests()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();

            _temporaryWebRoot = Path.Combine(Path.GetTempPath(), $"cookstack-tests-{Guid.NewGuid():N}");

            Directory.CreateDirectory(_temporaryWebRoot);

            var environmentMock = new Mock<IWebHostEnvironment>();

            environmentMock
                .SetupGet(environment => environment.WebRootPath)
                .Returns(_temporaryWebRoot);

            _service = new RecipeService(_dbContext, environmentMock.Object);
        }

        [Fact]
        public async Task UploadImageAsync_ValidJpeg_SavesFileandUpdatesImagePath()
        {
            var recipe = await CreateRecipeAsync();
            var file = CreateFile();


            var imagePath = await _service.UploadImage(recipe.Id, file);


            Assert.StartsWith("/uploads/recipes/", imagePath);

            Assert.EndsWith(".jpg", imagePath, StringComparison.OrdinalIgnoreCase);

            var updateRecipe = await _dbContext.Recipes
                .AsNoTracking()
                .SingleAsync(r => r.Id == recipe.Id);

            Assert.Equal(imagePath, updateRecipe.ImagePath);

            var physicalPath = GetPhysicalPath(imagePath);
            Assert.True(File.Exists(physicalPath));
        }

        [Fact]
        public async Task UploadImageAsync_ValidPng_SavesFileWithPngExtension()
        {
            var recipe = await CreateRecipeAsync();

            var file = CreateFile(fileName: "recipe.png", contentType: "image/png");


            var imagePath = await _service.UploadImage(recipe.Id, file);


            Assert.EndsWith(".png", imagePath, StringComparison.OrdinalIgnoreCase);

            Assert.True(File.Exists(GetPhysicalPath(imagePath)));
        }

        [Fact]
        public async Task UploadImageAsync_ValidWebp_SavesFileWithWebpExtension()
        {
            var recipe = await CreateRecipeAsync();

            var file = CreateFile(fileName: "recipe.webp", contentType: "image/webp");


            var imagePath = await _service.UploadImage(recipe.Id, file);


            Assert.EndsWith(".webp", imagePath, StringComparison.OrdinalIgnoreCase);

            Assert.True(File.Exists(GetPhysicalPath(imagePath)));
        }

        [Fact]
        public async Task UploadImageAsync_RecipeDoesNotExist_ThrowsKeyNotFoundException()
        {
            var file = CreateFile();

            var uploadsDirectory = Path.Combine(_temporaryWebRoot, "uploads", "recipes");

            
            var action = () => _service.UploadImage(recipeId: 999, file);


            await Assert.ThrowsAsync<KeyNotFoundException>(action);

            Assert.False(Directory.Exists(uploadsDirectory));
        }

        [Fact]
        public async Task UploadImageAsync_EmptyFile_ThrowsArgumentException()
        {
            var recipe = await CreateRecipeAsync();

            var file = new FormFile(Stream.Null, 0, 0, "file", "empty.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var uploadsDirectory = Path.Combine(_temporaryWebRoot, "uploads", "recipes");


            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.UploadImage(recipe.Id, file));


            Assert.Contains("No image", exception.Message, StringComparison.OrdinalIgnoreCase);

            Assert.False(Directory.Exists(uploadsDirectory));
        }

        [Fact]
        public async Task UploadImageAsync_UnsupportedContentType_ThrowsArgumentException()
        {
            var recipe = await CreateRecipeAsync();

            var file = CreateFile(fileName: "recipe.gif", contentType: "image/gif");

            var uploadsDirectory = Path.Combine(_temporaryWebRoot, "uploads", "recipes");


            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.UploadImage(recipe.Id, file));


            Assert.Contains("Unsupported", exception.Message, StringComparison.OrdinalIgnoreCase);

            Assert.False(Directory.Exists(uploadsDirectory));
        }


        [Fact]
        public async Task UploadImageasync_FileExceedsLimit_ThrowsArgumentException()
        {
            var recipe = await CreateRecipeAsync();

            var content = new byte[5 * 1024 * 1024 + 1];

            var uploadsDirectory = Path.Combine(_temporaryWebRoot, "uploads", "recipes");

            var file = CreateFile(fileName: "large.jpg", contentType: "image/jpeg", content: content);


            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.UploadImage(recipe.Id, file));


            Assert.Contains("5 MB", exception.Message, StringComparison.OrdinalIgnoreCase);

            Assert.False(Directory.Exists(uploadsDirectory));
        }

        [Fact]
        public async Task UploadImageAsync_FileExactlyAtLimit_Succeeds()
        {
            var recipe = await CreateRecipeAsync();

            var content = new byte[5 * 1024 * 1024];

            var file = CreateFile(fileName: "limit.jpg", contentType: "image/jpeg", content: content);


            var imagePath = await _service.UploadImage(recipe.Id, file);


            Assert.True(File.Exists(GetPhysicalPath(imagePath)));
        }

        [Fact]
        public async Task UploadImageAsync_RecipeHasPreviousUpload_DeletesPreviousFile()
        {
            var uploadsDirectory = Path.Combine(_temporaryWebRoot, "uploads", "recipes");

            Directory.CreateDirectory(uploadsDirectory);

            const string previousFileName = "previous.jpg";

            var previousPhysicalPath = Path.Combine(uploadsDirectory, previousFileName);

            await File.WriteAllTextAsync(previousPhysicalPath, "old image");

            const string previousImagePath = "/uploads/recipes/previous.jpg";

            var recipe = await CreateRecipeAsync(previousImagePath);

            var newFile = CreateFile(fileName: "new.png", contentType: "image/png");


            var newImagePath = await _service.UploadImage(recipe.Id, newFile);


            Assert.False(File.Exists(previousPhysicalPath));
            Assert.NotEqual(previousImagePath, newImagePath);
            Assert.True(File.Exists(GetPhysicalPath(newImagePath)));

            var updatedRecipe = await _dbContext.Recipes
                .AsNoTracking()
                .SingleAsync(r => r.Id == recipe.Id);

            Assert.Equal(newImagePath, updatedRecipe.ImagePath);
        }

        [Fact]
        public async Task UploadImageAsync_PreviousFileDoesNotExist_StillUploadsNewImage()
        {
            var recipe = await CreateRecipeAsync("/uploads/recipes/missing.jpg");

            var newFile = CreateFile();

            var newImagePath = await _service.UploadImage(recipe.Id, newFile);

            Assert.True(File.Exists(GetPhysicalPath(newImagePath)));

            var updateRecipe = await _dbContext.Recipes
                .AsNoTracking()
                .SingleAsync(r => r.Id == recipe.Id);

            Assert.Equal(newImagePath, updateRecipe.ImagePath);
        }

        [Fact]
        public async Task UploadImageAsync_TwoUploads_generateDifferentFileNames()
        {
            var firstRecipe = await CreateRecipeAsync();
            var secondRecipe = await CreateRecipeAsync();

            var firstPath = await _service.UploadImage(firstRecipe.Id, CreateFile());
            var secondPath = await _service.UploadImage(secondRecipe.Id, CreateFile());

            Assert.NotEqual(firstPath, secondPath);
            Assert.True(File.Exists(GetPhysicalPath(firstPath)));
            Assert.True(File.Exists(GetPhysicalPath(secondPath)));
        }

        private static IFormFile CreateFile(
            string fileName = "recipe.jpg", 
            string contentType = "image/jpeg", 
            byte[]? content = null)
        {
            content ??= Encoding.UTF8.GetBytes("fake image content");

            var stream = new MemoryStream(content);

            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType            
            };
        }

        private async Task<Recipe> CreateRecipeAsync(string? imagePath = null)
        {
            var recipe = new Recipe
            {
                Title = "Test recipe",
                Description = "Test description",
                ImagePath = imagePath
            };

            _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();

            return recipe;
        }

        private string GetPhysicalPath(string imagePath)
        {
            var relativePath = imagePath
                .TrimStart('/')
                .Replace('/', Path.DirectorySeparatorChar);

            return Path.Combine(_temporaryWebRoot, relativePath);
        }



        public void Dispose()
        {
            _dbContext.Dispose();
            _connection.Dispose();

            if(Directory.Exists(_temporaryWebRoot))
            {
                Directory.Delete(_temporaryWebRoot, recursive: true);
            }
        }
    }
}
