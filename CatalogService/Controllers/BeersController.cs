using CatalogService.Models;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BeersController : ControllerBase
{
    private readonly IBeerService _beerService;

    public BeersController(IBeerService beerService)
    {
        _beerService = beerService;
    }

    [HttpPost]
    public async Task<ActionResult<BeerResponse>> CreateBeer([FromBody] CreateBeerRequest request, CancellationToken cancellationToken)
    {
        var beer = await _beerService.AddBeerAsync(request, cancellationToken);
        return StatusCode(201, beer.ToResponse());
    }

    [HttpGet]
    public async Task<ActionResult<List<BeerResponse>>> GetBeers([FromQuery] string? search, CancellationToken cancellationToken)
    {
        var beers = string.IsNullOrWhiteSpace(search)
            ? await _beerService.GetAllBeersAsync(cancellationToken)
            : await _beerService.SearchBeersByNameAsync(search, cancellationToken);

        return Ok(beers.ToResponse());
    }

    [HttpPut("{id}/rating")]
    public async Task<ActionResult<BeerResponse>> AddRating(Guid id, [FromBody] UpdateRatingRequest request, CancellationToken cancellationToken)
    {
        var beer = await _beerService.AddRatingAsync(id, request.Rating, cancellationToken);

        if (beer == null)
            return NotFound(new { error = "Beer not found" });

        return Ok(beer.ToResponse());
    }
}
