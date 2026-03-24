namespace CatalogService.Models;

public class BeerResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public double? AverageRating { get; set; }
}
