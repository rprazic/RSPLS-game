using GameService.Domain.Constants;
using GameService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameService.Infrastructure.Configurations;

public class GameResultEntityTypeConfiguration : IEntityTypeConfiguration<GameResult>
{
    public void Configure(EntityTypeBuilder<GameResult> builder)
    {
        builder.Property(e => e.PlayedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(e => e.Result)
            .HasMaxLength(Lengths.DefaultString);

        builder.Property(e => e.PlayerChoiceName)
            .HasMaxLength(Lengths.DefaultString);

        builder.Property(e => e.ComputerChoiceName)
            .HasMaxLength(Lengths.DefaultString);

        builder.HasIndex(e => e.PlayedAt);
    }
}