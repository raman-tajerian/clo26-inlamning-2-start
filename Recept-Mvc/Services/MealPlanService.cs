using Recept.Models;

namespace Recept.Services;

/// <summary>
/// Ska byggas av er. GetWeekPlan och GetShoppingList visar demodata så att sidorna går att titta på.
/// PlanMeal kastar NotImplementedException, och sidan visar då "Inte implementerat ännu".
/// </summary>
public class MealPlanService : IMealPlanService
{
    // Du behöver bygga ut detta mer: spara måltiden, och returnera varningar vid 80 % och 100 %
    // av kategorins gräns. Samma 80 %-varning får inte visas igen förrän veckan byts (önskemål 4).
    // Här passar till exempel mönstret Observer.
    public List<string> PlanMeal(int recipeId, DateOnly date) => throw new NotImplementedException();

    // DEMO: samma påhittade måltider varje vecka. Ersätt med veckans måltider från databasen.
    // ISOWeek räknar veckonummer med måndag som första dag (önskemål 2).
    public List<PlannedMeal> GetWeekPlan(int year, int weekNumber)
    {
        DateOnly monday = WeekHelper.GetMonday(year, weekNumber);
        return
        [
            new PlannedMeal { Id = 1, Date = monday, RecipeId = 2 },
            new PlannedMeal { Id = 2, Date = monday.AddDays(3), RecipeId = 1 },
            new PlannedMeal { Id = 3, Date = monday.AddDays(6), RecipeId = 1 },
        ];
    }

    // DEMO: en fast lista som visar hur inköpslistan ska se ut för demoveckan ovan
    // (Tomatsoppa en gång och Pannkakor två gånger). Ersätt med en lista som räknas fram
    // ur veckoplanen, där samma ingrediens med samma enhet summeras till en rad.
    public List<ShoppingListItem> GetShoppingList(int year, int weekNumber) =>
    [
        new ShoppingListItem { Name = "vetemjöl", Amount = 5m, Unit = "dl" },
        new ShoppingListItem { Name = "mjölk", Amount = 12m, Unit = "dl" },
        new ShoppingListItem { Name = "ägg", Amount = 6m, Unit = "st" },
        new ShoppingListItem { Name = "salt", Amount = 0.5m, Unit = "tsk" },
        new ShoppingListItem { Name = "krossade tomater", Amount = 400m, Unit = "g" },
        new ShoppingListItem { Name = "gul lök", Amount = 1m, Unit = "st" },
        new ShoppingListItem { Name = "grönsaksbuljong", Amount = 3m, Unit = "dl" },
    ];
}
