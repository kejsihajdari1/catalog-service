using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models;

public class CreateBeerRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Type is required")]
    public string Type { get; set; } = string.Empty;
    
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int? Rating { get; set; }
}
