#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Masivian.Roulette.Api/Masivian.Roulette.Api.csproj", "Masivian.Roulette.Api/"]
COPY ["Masivian.Roulette.Domain/Masivian.Roulette.Domain.csproj", "Masivian.Roulette.Domain/"]
COPY ["Masivian.Roulette.Core/Masivian.Roulette.Core.csproj", "Masivian.Roulette.Core/"]
COPY ["Masivian.Roulette.Infrastructure/Masivian.Roulette.Infrastructure.csproj", "Masivian.Roulette.Infrastructure/"]
RUN dotnet restore "Masivian.Roulette.Api/Masivian.Roulette.Api.csproj"
COPY . .
WORKDIR "/src/Masivian.Roulette.Api"
RUN dotnet build "Masivian.Roulette.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Masivian.Roulette.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Masivian.Roulette.Api.dll"]