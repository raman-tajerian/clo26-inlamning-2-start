namespace Recept.Models;

/// <summary>
/// Ett recept med ingredienser, kategori, kock och valfri anteckning.
/// </summary>
public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Servings { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new();

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int ChefId { get; set; }
    public Chef? Chef { get; set; }

    public string? Note { get; set; }
}
