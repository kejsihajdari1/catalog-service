using CatalogService.Models;

namespace CatalogService.Services;

public interface IBeerService
{
    Task<Beer> AddBeerAsync(CreateBeerRequest request, CancellationToken cancellationToken = default);
    Task<List<Beer>> GetAllBeersAsync(CancellationToken cancellationToken = default);
    Task<List<Beer>> SearchBeersByNameAsync(string query, CancellationToken cancellationToken = default);
    Task<Beer?> AddRatingAsync(Guid id, int rating, CancellationToken cancellationToken = default);
}
