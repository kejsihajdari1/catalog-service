namespace CatalogService.Models;

public class Beer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<Rating> Ratings { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public double? AverageRating => Ratings.Any() ? Ratings.Average(r => r.Score) : null;
}
