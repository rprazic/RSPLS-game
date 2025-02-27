FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy csproj and restore dependencies
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GameService.Api/GameService.Api.csproj", "GameService.Api/"]
RUN dotnet restore "GameService.Api/GameService.Api.csproj"

# Copy all files and build
COPY . .
WORKDIR "/src/GameService.Api"
RUN dotnet build "GameService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GameService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy certificate
COPY --from=build /app/build/Certificates ../cert
USER root
RUN chmod 755 /cert

ENTRYPOINT ["dotnet", "GameService.Api.dll"]