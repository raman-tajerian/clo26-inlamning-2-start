using System.ComponentModel.DataAnnotations;
using Recept.Models;

namespace Recept.ViewModels;

/// <summary>
/// Formuläret "Lägg till recept". Har ett fast antal ingrediensrader; tomma rader ignoreras.
/// </summary>
public class RecipeFormModel : IValidatableObject
{
    public const int IngredientRows = 8;

    [Display(Name = "Namn")]
    [Required(ErrorMessage = "Receptet måste ha ett namn.")]
    public string Name { get; set; } = "";

    [Display(Name = "Antal portioner")]
    [Range(1, 100, ErrorMessage = "Antal portioner måste vara mellan 1 och 100.")]
    public int Servings { get; set; } = 4;

    [Display(Name = "Kategori")]
    [Range(1, int.MaxValue, ErrorMessage = "Välj en kategori.")]
    public int CategoryId { get; set; }

    [Display(Name = "Kock")]
    [Range(1, int.MaxValue, ErrorMessage = "Välj en kock.")]
    public int ChefId { get; set; }

    [Display(Name = "Anteckning")]
    public string? Note { get; set; }
    public List<IngredientRow> Ingredients { get; set; } =
        Enumerable.Range(0, IngredientRows).Select(_ => new IngredientRow()).ToList();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        for (int i = 0; i < Ingredients.Count; i++)
        {
            IngredientRow row = Ingredients[i];
            if (row.IsEmpty)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(row.Name))
            {
                yield return new ValidationResult($"Rad {i + 1}: ingrediensen saknar namn.");
            }
            if (row.Amount is null or < 0)
            {
                yield return new ValidationResult($"Rad {i + 1}: skriv en mängd, till exempel 2,5.");
            }
            if (string.IsNullOrWhiteSpace(row.Unit))
            {
                yield return new ValidationResult($"Rad {i + 1}: skriv en enhet, till exempel dl.");
            }
        }
    }

    public Recipe ToRecipe() => new()
    {
        Name = Name.Trim(),
        Servings = Servings,
        CategoryId = CategoryId,
        ChefId = ChefId,
        Note = string.IsNullOrWhiteSpace(Note) ? null : Note.Trim(),
        Ingredients = Ingredients
            .Where(row => !row.IsEmpty)
            .Select(row => new Ingredient { Name = row.Name!.Trim(), Amount = row.Amount!.Value, Unit = row.Unit!.Trim() })
            .ToList(),
    };
}

public class IngredientRow
{
    [Display(Name = "ingrediens")]
    public string? Name { get; set; }

    [Display(Name = "mängd")]
    public decimal? Amount { get; set; }

    [Display(Name = "enhet")]
    public string? Unit { get; set; }

    public bool IsEmpty => string.IsNullOrWhiteSpace(Name) && Amount is null && string.IsNullOrWhiteSpace(Unit);
}
