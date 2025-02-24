using GameService.Domain.Commands;
using GameService.Domain.Dtos;
using GameService.Domain.Entities;
using Mapster;

namespace GameService.Aplication.Mapping;

public class GameMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GameResult, GameResultDto>()
            .Map(dest => dest.PlayerChoice, src => src.PlayerChoiceName)
            .Map(dest => dest.ComputerChoice, src => src.ComputerChoiceName);

        config.NewConfig<PlayGameCommand, GameResult>()
            .Map(dest => dest.PlayerChoice, src => src.PlayerChoice)
            .Map(dest => dest.PlayedAt, src => DateTime.UtcNow)
            .Ignore(dest => dest.Id);
    }
}