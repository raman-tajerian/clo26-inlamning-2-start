using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Recept.Models;

/// <summary>
/// En ingrediens i ett recept, till exempel "2,5 dl vetemjöl".
/// Denna ligger inbäddad i receptet och har därför inget eget Id.
/// </summary>
public class Ingredient
{
    public string Name { get; set; } = "";

    // Decimal gör att mängden sparas som tal och inte som text i MongdoDb
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Amount { get; set; }

    public string Unit { get; set; } = "";
}
