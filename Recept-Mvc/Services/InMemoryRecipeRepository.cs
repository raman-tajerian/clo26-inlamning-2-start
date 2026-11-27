using Recept.Models;

namespace Recept.Services;

/// <summary>
/// Låtsasdatabas: allt ligger i listor i minnet och försvinner när programmet stängs.
/// Den finns bara för att sidorna ska gå att köra från dag ett.
/// Ersätt den med en egen klass som implementerar IRecipeRepository mot er databas,
/// och byt sedan en enda rad i Program.cs (registreringen av IRecipeRepository).
/// </summary>
public class InMemoryRecipeRepository : IRecipeRepository
{
    private readonly List<Recipe> _recipes = new();
    private readonly List<Chef> _chefs = new();
    private int _nextRecipeId = 1;
    private int _nextChefId = 1;

    public InMemoryRecipeRepository()
    {
        Seed();
    }

    public List<Recipe> GetAllRecipes() => _recipes.ToList();

    public Recipe? GetRecipe(int id) => _recipes.FirstOrDefault(r => r.Id == id);

    public void AddRecipe(Recipe recipe)
    {
        recipe.Id = _nextRecipeId++;
        _recipes.Add(recipe);
    }

    public void DeleteRecipe(int id) => _recipes.RemoveAll(r => r.Id == id);

    public List<Recipe> SearchByIngredient(string ingredientName) =>
        _recipes
            .Where(r => r.Ingredients.Any(i => i.Name.Contains(ingredientName, StringComparison.OrdinalIgnoreCase)))
            .ToList();

    public List<Chef> GetAllChefs() => _chefs.ToList();

    public void AddChef(Chef chef)
    {
        // Du behöver bygga ut detta mer: beställningen säger att programmet ska klara 1–5 kockar.
        chef.Id = _nextChefId++;
        _chefs.Add(chef);
    }

    // Exempeldata, så att det finns något att titta på direkt.
    // Er riktiga databas ska seeda sig själv på samma sätt vid första start, om den är tom.
    // Här passar till exempel mönstret Builder: ett recept med många ingredienser blir
    // lättare att läsa om det byggs steg för steg, till exempel .WithIngredient("mjölk", 6, "dl").
    private void Seed()
    {
        AddChef(new Chef { FirstName = "Kim"});

        AddRecipe(new Recipe
        {
            Name = "Pannkakor",
            Servings = 4,
            Ingredients =
            {
                new Ingredient { Name = "vetemjöl", Amount = 2.5m, Unit = "dl" },
                new Ingredient { Name = "mjölk", Amount = 6m, Unit = "dl" },
                new Ingredient { Name = "ägg", Amount = 3m, Unit = "st" },
                new Ingredient { Name = "salt", Amount = 0.25m, Unit = "tsk" },
            }
        });

        AddRecipe(new Recipe
        {
            Name = "Tomatsoppa",
            Servings = 2,
            Ingredients =
            {
                new Ingredient { Name = "krossade tomater", Amount = 400m, Unit = "g" },
                new Ingredient { Name = "gul lök", Amount = 1m, Unit = "st" },
                new Ingredient { Name = "grönsaksbuljong", Amount = 3m, Unit = "dl" },
            }
        });
    }
}
