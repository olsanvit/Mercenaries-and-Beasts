using MercenariesAndBeasts.Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace MercenariesAndBeasts.Infrastructure;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.HasIndex(l => l.Code).IsUnique();
            entity.Property(l => l.Code).IsRequired().HasMaxLength(64);
            entity.Property(l => l.Type).IsRequired().HasMaxLength(32);
            entity.Property(l => l.NameEn).IsRequired().HasMaxLength(128);
        });
    }
}