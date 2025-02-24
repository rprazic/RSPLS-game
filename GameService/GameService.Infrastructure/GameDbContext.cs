using GameService.Domain.Entities;
using GameService.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GameService.Infrastructure;

public class GameDbContext(DbContextOptions<GameDbContext> options)
    : DbContext(options)
{
    public GameDbContext() : this(new DbContextOptions<GameDbContext>())
    {
    }

    public virtual DbSet<GameResult> GameResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new GameResultEntityTypeConfiguration()
            .Configure(modelBuilder.Entity<GameResult>());
    }
}