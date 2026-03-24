using CatalogService.Data;
using CatalogService.Models;
using CatalogService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CatalogService.Tests;

public class BeerServiceTests
{
    private BeerDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<BeerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new BeerDbContext(options);
    }

    private ILogger<BeerService> CreateLogger()
    {
        return NullLogger<BeerService>.Instance;
    }

    [Fact]
    public async Task AddBeerAsync_CreatesBeerWithAndWithoutRating()
    {
        using var context = CreateInMemoryContext();
        var service = new BeerService(context, CreateLogger());
        
        var beerWithRating = await service.AddBeerAsync(new CreateBeerRequest 
        { 
            Name = "IPA", 
            Type = "Pale Ale", 
            Rating = 4 
        });
        
        var beerWithoutRating = await service.AddBeerAsync(new CreateBeerRequest 
        { 
            Name = "Guinness", 
            Type = "Stout" 
        });

        Assert.Equal("IPA", beerWithRating.Name);
        Assert.Equal(4.0, beerWithRating.AverageRating);
        Assert.Equal("Guinness", beerWithoutRating.Name);
        Assert.Null(beerWithoutRating.AverageRating);
    }

    [Fact]
    public async Task GetAllBeersAsync_ReturnsAllBeers()
    {
        using var context = CreateInMemoryContext();
        var service = new BeerService(context, CreateLogger());
        
        await service.AddBeerAsync(new CreateBeerRequest { Name = "Beer1", Type = "Lager" });
        await service.AddBeerAsync(new CreateBeerRequest { Name = "Beer2", Type = "Ale" });

        var result = await service.GetAllBeersAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task SearchBeersByNameAsync_FindsPartialMatches()
    {
        using var context = CreateInMemoryContext();
        var service = new BeerService(context, CreateLogger());
        
        await service.AddBeerAsync(new CreateBeerRequest { Name = "Pale Ale IPA", Type = "Ale" });
        await service.AddBeerAsync(new CreateBeerRequest { Name = "Dark Stout", Type = "Stout" });

        var result = await service.SearchBeersByNameAsync("pale");

        Assert.Single(result);
        Assert.Equal("Pale Ale IPA", result[0].Name);
    }

    [Fact]
    public async Task AddRatingAsync_AveragesMultipleRatings()
    {
        using var context = CreateInMemoryContext();
        var service = new BeerService(context, CreateLogger());
        
        var beer = await service.AddBeerAsync(new CreateBeerRequest { Name = "Test Beer", Type = "Lager", Rating = 5 });
        await service.AddRatingAsync(beer.Id, 3);
        var result = await service.AddRatingAsync(beer.Id, 4);

        Assert.Equal(3, result!.Ratings.Count);
        Assert.Equal(4.0, result.AverageRating);
    }
}
