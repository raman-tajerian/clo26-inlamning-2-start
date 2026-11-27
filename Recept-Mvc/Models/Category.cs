namespace Recept.Models;

/// <summary>
/// En receptkategori, till exempel "Middag", "Efterrätt" eller "Övrigt"
/// </summary>
public class Category
{
    public const string DefaultOtherName = "Övrigt";
    public int Id { get; set; }
    public string Name { get; set; } = "";

    // Hur många gånger per vecka kategorin får förekomma i veckoplan (null = ingen gräns satt)
    public int? WeeklyLimit { get; set; }

    public bool IsDefaultOther =>
        string.Equals(Name?.Trim(), DefaultOtherName, StringComparison.OrdinalIgnoreCase);

    public bool CanBeDeleted() => !IsDefaultOther;
}
