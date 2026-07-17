namespace CookStack.Shared.Recipes.Dtos
{
    public class RecipeListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastVisitedAt { get; set; }
    }
}
