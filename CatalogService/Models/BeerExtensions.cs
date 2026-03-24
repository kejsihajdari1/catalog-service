namespace CatalogService.Models;

public static class BeerExtensions
{
    public static BeerResponse ToResponse(this Beer beer)
    {
        return new BeerResponse
        {
            Id = beer.Id,
            Name = beer.Name,
            Type = beer.Type,
            AverageRating = beer.AverageRating
        };
    }
    
    public static List<BeerResponse> ToResponse(this List<Beer> beers)
    {
        return beers.Select(b => b.ToResponse()).ToList();
    }
}
