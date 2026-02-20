namespace CookStackClient.Constants
{
    public static class Routes
    {
        public static class Recipes
        {
            public const string RecipesMain = "/recipes";
            public const string RecipeDetails = "/recipe/{0}";
            public const string RecipeCreate = "/recipe-create";
            public const string RecipeUpdate = "/recipe-update/{0}";
        }

        public static class  ShoppingLists
        {
            public const string ShoppingListsMain = "/shopping-list";
            public const string ShoppingListDetails = "/shopping-list/{0}";
        }
    }
}
