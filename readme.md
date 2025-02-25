# Rock, Paper, Scissors, Lizard, Spock Game Service
![Build & Tests](https://github.com/rprazic/RSPLS-game/actions/workflows/ci.yml/badge.svg)

A modern .NET 9 game service that implements the "Rock, Paper, Scissors, Lizard, Spock" game popularized by the TV show "The Big Bang Theory". The service provides a resilient, well-structured API with comprehensive game statistics and monitoring capabilities.

## Features

- Play Rock, Paper, Scissors, Lizard, Spock against a computer opponent
- View game history and latest results
- Access detailed game statistics with time range filtering (requires authentication)
- CQRS architecture with MediatR
- Resilient external service integration with Polly
- Efficient object mapping with Mapster
- Structured logging with Serilog
- PostgreSQL database for data persistence
- Dockerized deployment
- Comprehensive test coverage
- CI/CD pipeline with GitHub Actions

## Tech Stack

- .NET 9 Web API
- Entity Framework Core with PostgreSQL
- MediatR for CQRS implementation
- Mapster for object mapping
- Polly for HTTP resilience
- FluentValidation for request validation
- Serilog for structured logging
- Docker and Docker Compose
- xUnit for testing
- GitHub Actions for CI/CD

## API Endpoints

### Game Endpoints (Public)

- `GET /game/choices` - Get all available choices
- `GET /game/choice` - Get a random choice
- `POST /game/play` - Play a round against the computer
- `GET /game/results/latest?count=10` - Get the latest game results

### Statistics Endpoints (Authenticated)

- `GET /statistics?from={date}&to={date}` - Get overall game statistics
- `GET /statistics/choices?from={date}&to={date}` - Get statistics for each choice
- `GET /statistics/winrates?from={date}&to={date}` - Get win rates by choice

## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET 9 SDK (for development)
- PostgreSQL (if running locally)

### Running with Docker Compose

1. Clone the repository:
   ```bash
   git clone https://github.com/rprazic/RSPLS-game.git
   cd RPSLS-game/GameService
   ```

2. Start the application and database:
   ```bash
   docker-compose up -d
   ```

3. Access the API at [https://localhost:6001/swagger](https://localhost:6001/swagger)

## Authentication

Statistics endpoints require API key authentication:

1. Set your API key in `appsettings.json` or environment variables
2. Include the API key in requests to statistics endpoints:
   ```http
   GET /statistics
   X-API-Key: 2!pf629B(Nmf
   ```

## Testing

Run unit tests:
```bash
dotnet test
```

The test suite includes:
- Unit tests for all components
- Validation tests
- Time range filtering tests

## Architecture

### CQRS Pattern
The application implements CQRS using MediatR:
- Commands: Game actions (playing a game)
- Queries: Data retrieval (statistics, history)

### Resilience
- Polly retry policies for external service calls
- Circuit breaker pattern implementation
- Timeout policies

### Validation
- FluentValidation for request validation
- Validation behavior pipeline
- Comprehensive error handling

### Mapping
- Mapster for efficient object mapping
- Type-safe mapping configurations
- Custom mapping profiles

## Game Rules

- Rock crushes Scissors and Lizard
- Paper covers Rock and disproves Spock
- Scissors cuts Paper and decapitates Lizard
- Lizard eats Paper and poisons Spock
- Spock vaporizes Rock and smashes Scissors

## Docker Deployment

The application includes Docker configurations for both development and production environments:

Development:
```bash
docker-compose up -d
```

Production:
```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

## Monitoring

The application includes:
- Health check endpoints
- Structured logging with Serilog
- Performance monitoring through MediatR behaviors
- Docker health checks

## Configuration

Key configuration options in appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=db;Database=gamedb;Username=gameuser;Password=gamepass"
  },
  "AuthSettings": {
    "ApiKey": "YourSecureApiKey123456"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    }
  }
}
```

## Local Development

### Required Tools
- Visual Studio 2022 or Rider
- Docker Desktop
- PostgreSQL (optional if using Docker)

### Development Setup

1. Install .NET 9 SDK

2. Install required global tools:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Update the connection string in `appsettings.Development.json`

5. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

6. Run the application:
   ```bash
   dotnet run
   ```

## Credits

- [Setup migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli)
- [.NET developer HTTPS certificate config](https://stackoverflow.com/questions/69282468/using-dotnet-dev-certs-with-aspnet-docker-image#comment138114599_76165591)

