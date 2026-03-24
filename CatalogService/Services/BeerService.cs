using CatalogService.Models;
using CatalogService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatalogService.Services;

public class BeerService : IBeerService
{
    private readonly BeerDbContext _context;
    private readonly ILogger<BeerService> _logger;

    public BeerService(BeerDbContext context, ILogger<BeerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Beer> AddBeerAsync(CreateBeerRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating beer: {Name} ({Type})", request.Name, request.Type);
        
        var beer = new Beer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Type = request.Type,
            Ratings = new List<Rating>(),
            CreatedAt = DateTime.UtcNow
        };

        if (request.Rating.HasValue)
        {
             var newRating = new Rating
            {
                Id = Guid.NewGuid(),
                BeerId = beer.Id,
                Score = request.Rating.Value,
                CreatedAt = DateTime.UtcNow
            };
            beer.Ratings.Add(newRating);
        }

        _context.Beers.Add(beer);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Created beer with ID: {BeerId}", beer.Id);
        return beer;
    }

    public async Task<List<Beer>> GetAllBeersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Beers
            .Include(b => b.Ratings)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Beer>> SearchBeersByNameAsync(string query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching beers by name: {Query}", query);
        
        var results = await _context.Beers
            .Include(b => b.Ratings)
            .Where(b => EF.Functions.Like(b.Name, $"%{query}%"))
            .ToListAsync(cancellationToken);
        
        _logger.LogInformation("Found {Count} beers matching '{Query}'", results.Count, query);
        return results;
    }

    public async Task<Beer?> AddRatingAsync(Guid id, int rating, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding rating {Rating} to beer {BeerId}", rating, id);
        
        var beer = await _context.Beers
            .Include(b => b.Ratings)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        
        if (beer == null)
        {
            _logger.LogWarning("Beer not found: {BeerId}", id);
            return null;
        }

        var newRating = new Rating
        {
            Id = Guid.NewGuid(),
            BeerId = id,
            Score = rating,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Ratings.Add(newRating);
        beer.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Updated beer {BeerId} average rating to {Average}", beer.Id, beer.AverageRating);
        return beer;
    }
}
