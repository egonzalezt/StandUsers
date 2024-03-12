#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StandUsers.WebApi/StandUsers.WebApi.csproj", "StandUsers.WebApi/"]
COPY ["StandUsers.Application/StandUsers.Application.csproj", "StandUsers.Application/"]
COPY ["StandUsers.Domain/StandUsers.Domain.csproj", "StandUsers.Domain/"]
COPY ["StandUsers.Infrastructure/StandUsers.Infrastructure.csproj", "StandUsers.Infrastructure/"]
COPY ["StandUsers.Workers/StandUsers.Workers.csproj", "StandUsers.Workers/"]
RUN dotnet restore "./StandUsers.WebApi/./StandUsers.WebApi.csproj"
COPY . .
WORKDIR "/src/StandUsers.WebApi"
RUN dotnet build "./StandUsers.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StandUsers.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StandUsers.WebApi.dll"]