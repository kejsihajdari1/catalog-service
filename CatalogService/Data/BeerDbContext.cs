using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data;

public class BeerDbContext : DbContext
{
    public BeerDbContext(DbContextOptions<BeerDbContext> options) : base(options)
    {
    }

    public DbSet<Beer> Beers { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Beer)
            .WithMany(b => b.Ratings)
            .HasForeignKey(r => r.BeerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
