using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models;

public class UpdateRatingRequest
{
    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }
}
