using CookStack.Api.Features.Recipes;
using CookStack.Api.Features.ShoppingList;
using CookStack.Shared.Enums;
using Microsoft.EntityFrameworkCore;



namespace CookStack.Api.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedRecipeAsync(context);
            await SeedShoppingListsAsync(context);
        }

        private static async Task SeedRecipeAsync(ApplicationDbContext context)
        {
            if (await context.Recipes.AnyAsync())
                return;

            var now = DateTime.UtcNow;

            var recipes = CreateRecipes(now);

            await context.Recipes.AddRangeAsync(recipes);
            await context.SaveChangesAsync();
        }

        private static async Task SeedShoppingListsAsync(ApplicationDbContext context)
        {
            if (await context.ShoppingLists.AnyAsync())
                return;

            var now = DateTime.UtcNow;

            var shoppingLists = CreateShoppingLists(now);

            await context.ShoppingLists.AddRangeAsync(shoppingLists);
            await context.SaveChangesAsync();
        }

        private static List<Recipe> CreateRecipes(DateTime now)
        {
            return
            [
                new Recipe
                {
                    Title = "Muffinki a la pizza",
                    SourceUrl ="https://www.przepisy.pl/przepis/muffinki-a-la-pizza",
                    ImagePath ="/images/seed/pizza-muffins.jpg",
                    CreatedAt = now.AddDays(-14),
                    LastVisitedAt = now.AddHours(-2),

                    Ingredients =
                    [
                        new RecipeIngredient
                        {
                            Name = "mąka",
                            Quantity = 2,
                            Unit = UnitType.Cup
                        },
                        new RecipeIngredient
                        {
                            Name = "suche drożdże",
                            Quantity = 7,
                            Unit = UnitType.Gram
                        },
                        new RecipeIngredient
                        {
                            Name = "majonez",
                            Quantity = 4,
                            Unit = UnitType.Tablespoon
                        },
                        new RecipeIngredient
                        {
                            Name = "ciepła woda",
                            Quantity = 0.5M,
                            Unit = UnitType.Cup
                        },
                        new RecipeIngredient
                        {
                            Name = "jajko",
                            Quantity = 1,
                            Unit = UnitType.Piece
                        },
                        new RecipeIngredient
                        {
                            Name = "sól",
                            Quantity = 1,
                            Unit = UnitType.Pinch
                        },
                        new RecipeIngredient
                        {
                            Name = "cukier",
                            Quantity = 1,
                            Unit = UnitType.Teaspoon
                        },
                        new RecipeIngredient
                        {
                            Name = "tarta mozarella",
                            Quantity = 0.5M,
                            Unit = UnitType.Cup
                        },
                        new RecipeIngredient
                        {
                            Name = "starty parmezan",
                            Quantity = 3,
                            Unit = UnitType.Tablespoon
                        },
                        new RecipeIngredient
                        {
                            Name = "kiełbasa chorizo pikantna",
                            Quantity = 200,
                            Unit = UnitType.Gram
                        },
                        new RecipeIngredient
                        {
                            Name = "oliwki zielone",
                            Quantity = 100,
                            Unit = UnitType.Gram
                        },
                        new RecipeIngredient
                        {
                            Name = "zioła prowansalskie",
                            Quantity = 100,
                            Unit = UnitType.Tablespoon
                        }
                    ],

                    Steps =
                    [
                        new RecipeStep
                        {
                            Order = 1,
                            Description =
                            "Cukier, sól i drożdże rozpuść w ciepłej wodzie. Powstały płyn połącz mąką, jajkiem i majonezem. " +
                            "Dokładnie wyrób ciasto tak, aby było elastyczne i gładkie. " +
                            "Jeśli ciasto będzie zbyt luźne, dodaj więcej mąki i podobnie jeśli będzie zbyt twarde, dodaj odrobinę więcej wody. " +
                            "Ciasto przełóż do miski, przykryj ściereczką i odstaw na około 60 minut w ciepłe miejsce, aby wyrosło. "
                        },
                        new RecipeStep
                        {
                            Order = 2,
                            Description =
                            "Kiełbasę pokrój w plasterki – podsmaż ją na patelni tak, aby się wytopiło z niej trochę tłuszczu."
                        },
                        new RecipeStep
                        {
                            Order = 3,
                            Description =
                            "Ciasto delikatnie pomieszaj z serem mozarella, kiełbasą razem z tłuszczem, pokrojonymi oliwkami i ziołami"
                        },
                        new RecipeStep
                        {
                            Order = 4,
                            Description =
                            "Powstałą w ten sposób mieszankę rozłóż do foremek na muffinki wcześniej wysmarowanych olejem."
                        },
                        new RecipeStep
                        {
                            Order = 5,
                            Description =
                            "Całość wstaw do nagrzanego do 180*C piekarnika na około 20 minut. Podawaj z dipem czosnkowym."
                        }

                    ]
                },

                new Recipe
                {
                    Title = "Kurczak curry",
                    SourceUrl = "https://aniagotuje.pl/przepis/kurczak-curry",
                    Description = "Kurczak curry to bardzo popularne danie kuchni indyjskiej. Można go przyrządzić na wiele sposobów. " +
                    "Moja wersja zawiera sporo warzyw oraz wysokoprocentowe mleczko kokosowe. Curry z kurczakiem podaję zazwyczaj z ryżem basmati. " +
                    "Bardzo dobrze smakuje również z chlebkami naan oraz z upieczonymi ziemniakami.",
                    ImagePath = "images/seed/chicken-curry.jpg",
                    CreatedAt = now.AddDays(-10),
                    LastVisitedAt = now.AddDays(-1),

                    Ingredients =
                    [
                        new RecipeIngredient
                        {
                            Name = "pierś z kurczaka",
                            Quantity = 650,
                            Unit = UnitType.Gram
                        },
                        new RecipeIngredient
                        {
                            Name = "papryka czerwona",
                            Quantity = 280,
                            Unit = UnitType.Gram
                        },
                        new RecipeIngredient
                        {
                            Name = "cebula",
                            Quantity = 250,
                            Unit = UnitType.Gram
                        },
                        new RecipeIngredient
                        {
                            Name = "marchewka",
                            Quantity = 140,
                            Unit = UnitType.Gram
                        },
                        new RecipeIngredient
                        {
                            Name = "ząbki czosnku",
                            Quantity = 3,
                            Unit = UnitType.Piece
                        },
                        new RecipeIngredient
                        {
                            Name = "kawałek imbiru",
                            Quantity = 20,
                            Unit = UnitType.Gram
                        },
                        new RecipeIngredient
                        {
                            Name = "mleko kokosowe",
                            Quantity = 250,
                            Unit = UnitType.Milliliter
                        },
                        new RecipeIngredient
                        {
                            Name = "bulion drobiowy",
                            Quantity = 125,
                            Unit = UnitType.Milliliter
                        },
                        new RecipeIngredient
                        {
                            Name = "delikatna oliwa",
                            Quantity = 4,
                            Unit = UnitType.Tablespoon
                        },
                        new RecipeIngredient
                        {
                            Name = "łagodna pasta curry",
                            Quantity = 2,
                            Unit = UnitType.Tablespoon
                        },
                        new RecipeIngredient
                        {
                            Name = "przyprawa curry",
                            Quantity = 1,
                            Unit = UnitType.Tablespoon
                        }
                    ],

                    Steps =
                    [
                        new RecipeStep
                        {
                            Order = 1,
                            Description =
                            "Dwa filety z kurczaka oczyść i pokrój w kostkę/kawałki nie większe niż 2 cm. " +
                            "Do miseczki z gotowym do smażenia mięsem dodaj łyżkę przyprawy curry w proszku lub też skomponuj własną mieszankę łącząc ze sobą: " +
                            "kolendrę młotkowaną, chilli, pieprz, cynamon, goździki mielone, gałkę muszkatołową, kardamon oraz kminek. " +
                            "Czosnku i imbiru nie musisz dodawać, ponieważ używasz ich w formie świeżej. Całość wymieszaj i odstaw. "
                        },
                        new RecipeStep
                        {
                            Order = 2,
                            Description =
                            "Zacznij nagrzewać dużą patelnię z grubym dnem i z wysokimi rantami. Wlej cztery łyżki delikatnej (nie gorzkiej) oliwy (może być też zwykły olej do smażenia). " +
                            "Po chwili wyłóż też obraną i posiekaną cebulę. Podsmażaj ją na średniej mocy palnika przez pięć minut.  "
                        },
                        new RecipeStep
                        {
                            Order = 3,
                            Description =
                            "Następnie dodaj obraną i pokrojoną w krótkie i cienkie słupki marchewkę. Zaraz potem dodaj też oczyszczoną z gniazda nasiennego i pokrojoną w kostkę czerwoną paprykę. " +
                            "Wszystko wymieszaj i podsmażaj dalej na średniej mocy palnika przez 10 minut. Cały czas bez przykrywki.  "
                        },
                        new RecipeStep
                        {
                            Order = 4,
                            Description =
                            "Po 10 minutach na patelnię wyłóż też kawałki kurczaka w przyprawie curry, obrany i pokrojony w plasterki kawałek imbiru oraz obrane i pokrojone w plasterki ząbki czosnku. " +
                            "Ponownie wymieszaj całość i smaż przez kolejne pięć minut. Ta sama moc i również bez przykrywki. "
                        },
                        new RecipeStep
                        {
                            Order = 5,
                            Description =
                            "Na koniec wlej jeszcze pół szklanki bulionu drobiowego (może być też warzywny lub w ostateczności sama woda). " +
                            "Dodaj jedną szklankę mleczka kokosowego (im gęstsze tym lepiej) oraz dwie pełne łyżki pasty curry (jeśli masz ostrą pastę, to daj tylko jedną łyżkę. " +
                            "W razie potrzeby zawsze możesz dołożyć więcej pasty przed samym podaniem dania)."
                        },
                        new RecipeStep
                        {
                            Order = 6,
                            Description =
                            "Pastę możesz zastąpić też dwiema płaskimi łyżkami przyprawy curry w proszku lub też mieszanką przypraw i ziół, które wymieniłam wyżej. " +
                            "Pasty mają często trochę inny skład niż mieszanki w proszku, dlatego też jeśli nie masz pasty, to warto dodać jeszcze po szczypcie kozieradki, kminu rzymskiego oraz tamaryndowiec lub trawę cytrynową." +
                            " \r\n\r\nWymieszaj całe danie i podgrzewaj ostatnie pięć minut.  "
                        },
                        new RecipeStep
                        {
                            Order = 7,
                            Description =
                            "Zanim podasz kurczaka curry, sprawdź smak dania. Jeśli jest dla Ciebie za ostre, to możesz dodać więcej mleczka kokosowego lub tez wlać kilka łyżek śmietanki 30 %." +
                            " \r\n\r\nJeśli danie jest za łagodne, to dodaj więcej imbiru oraz chili. Jeśli brakuje Ci kwasowości, to dodaj sok z limonki. " +
                            "Dla lekkiej słodyczy dodaj odrobinę jasnego syropu.\r\n\r\nKurczak curry wyśmienicie smakuje podany z ryżem basmati lub z ryżem jaśminowym. " +
                            "Bardzo dobry będzie też podany z chlebkiem naan a nawet z pieczonymi ziemniakami. Tutaj przepis na chleb naan. " +
                            "U mnie obowiązkowo musi się też pojawić świeża kolendra, którą dekoruję obficie całe danie. "
                        }
                        
                    ]
                }

            ];
        }

        private static List<ShoppingList> CreateShoppingLists(DateTime now)
        {
            return
            [
                new ShoppingList
                {
                    Title = "Weekendowe zakupy spożywcze",
                    Description = "Zakupy na sobotę i niedzielę",
                    CreatedAt = now.AddDays(-3),

                    Items =
                    [
                        new ShoppingItem
                        {
                            Name = "Pierś z kurczaka",
                            Quantity = 650,
                            Unit = UnitType.Gram,
                            IsChecked = false,
                            Order = 1
                        },
                        new ShoppingItem
                        {
                            Name = "Ryż",
                            Quantity = 250,
                            Unit = UnitType.Gram,
                            IsChecked = false,
                            Order = 2
                        },
                        new ShoppingItem
                        {
                            Name = "Mleko kokosowe",
                            Quantity = 400,
                            Unit = UnitType.Milliliter,
                            IsChecked = true,
                            Order = 3
                        }
                    ]
                },
                new ShoppingList
                {
                    Title = "Produkty śniadaniowe",
                    Description = "Wszystko, co potrzebne do śniadań w tym tygodniu",
                    CreatedAt = now.AddDays(-6),

                    Items =
                    [
                        new ShoppingItem
                        {
                            Name = "Jajka",
                            Quantity = 10,
                            Unit = UnitType.Piece,
                            IsChecked = true,
                            Order = 1
                        },
                        new ShoppingItem
                        {
                            Name = "Mleko",
                            Quantity = 1,
                            Unit = UnitType.Liter,
                            IsChecked = true,
                            Order = 2
                        },
                        new ShoppingItem
                        {
                            Name = "Chleb",
                            Quantity = 1,
                            Unit = UnitType.Piece,
                            IsChecked = true,
                            Order = 3
                        }
                    ]
                },
                new ShoppingList
                {
                    Title = "Szybkie zakupy",
                    CreatedAt = now.AddDays(-8),

                    Items =
                    [
                        new ShoppingItem
                        {
                            Name = "Pomidory",
                            Quantity = 4,
                            Unit = UnitType.Piece,
                            IsChecked = false,
                            Order = 1
                        },
                        new ShoppingItem
                        {
                            Name = "Mozzarella",
                            Quantity = 2,
                            Unit = UnitType.Piece,
                            IsChecked = false,
                            Order = 2
                        }

                    ]
                }
            ];
        }
    }
}
