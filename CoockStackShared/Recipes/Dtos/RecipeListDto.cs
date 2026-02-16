namespace CookStackShared.Recipes.Dtos
{
    public class RecipeListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
