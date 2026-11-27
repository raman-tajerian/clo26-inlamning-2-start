using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recept.Services;
using Recept.ViewModels;

namespace Recept.Controllers;

/// <summary>
/// Sidor för veckoplan och inköpslista. Logiken ligger i IMealPlanService.
/// </summary>
public class MealPlanController : AppController
{
    private readonly IMealPlanService _mealPlan;
    private readonly IRecipeRepository _repository;
    private readonly IRecipeCalculator _calculator;

    public MealPlanController(IMealPlanService mealPlan, IRecipeRepository repository, IRecipeCalculator calculator)
    {
        _mealPlan = mealPlan;
        _repository = repository;
        _calculator = calculator;
    }

    public IActionResult Index(int? year, int? week) => Run(() =>
    {
        WeekViewModel model = CreateWeek(year, week);
        model.Meals = _mealPlan.GetWeekPlan(model.Year, model.Week)
            .OrderBy(m => m.Date)
            .Select(m => (m.Date, _repository.GetRecipe(m.RecipeId)?.Name ?? "(okänt recept)"))
            .ToList();
        return View(model);
    });

    public IActionResult ShoppingList(int? year, int? week) => Run(() =>
    {
        WeekViewModel model = CreateWeek(year, week);
        model.ShoppingLines = _mealPlan.GetShoppingList(model.Year, model.Week)
            .Select(item => $"{_calculator.FormatAmount(item.Amount, item.Unit)} {item.Name}")
            .ToList();
        return View(model);
    });

    public IActionResult Plan() => Run(() =>
    {
        ViewData["Recipes"] = RecipeOptions();
        return View();
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Plan(int recipeId, DateOnly? date) => Run(() =>
    {
        if (date is null)
        {
            ModelState.AddModelError("date", "Välj ett datum.");
            ViewData["Recipes"] = RecipeOptions();
            return View();
        }

        try
        {
            List<string> warnings = _mealPlan.PlanMeal(recipeId, date.Value);
            TempData["Message"] = "Måltiden är inplanerad.";
            if (warnings.Count > 0)
            {
                TempData["Warning"] = string.Join(" ", warnings);
            }
        }
        catch (ArgumentException ex)
        {
            TempData["Error"] = ex.Message;
        }

        (int year, int week) = WeekHelper.GetYearAndWeek(date.Value);
        return RedirectToAction(nameof(Index), new { year, week });
    });

    // Utan val visas innevarande vecka. Veckan börjar på måndag (ISO 8601).
    private static WeekViewModel CreateWeek(int? year, int? week)
    {
        (int currentYear, int currentWeek) = WeekHelper.GetYearAndWeek(DateOnly.FromDateTime(DateTime.Today));
        int selectedYear = year ?? currentYear;

        return new WeekViewModel
        {
            Year = selectedYear,
            Week = Math.Clamp(week ?? currentWeek, 1 , WeekHelper.GetWeeksInYear(selectedYear))
        };
    }

    private List<SelectListItem> RecipeOptions() =>
        _repository.GetAllRecipes()
            .Select(r => new SelectListItem(r.Name, r.Id.ToString()))
            .ToList();
}
