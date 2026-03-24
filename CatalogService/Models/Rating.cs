namespace CatalogService.Models;

public class Rating
{
    public Guid Id { get; set; }
    public Guid BeerId { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Beer Beer { get; set; } = null!;
}
