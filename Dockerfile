FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ubicanimal.Api/Ubicanimal.Api.csproj", "Ubicanimal.Api/"]
COPY ["Ubicanimal.Application/Ubicanimal.Application.csproj", "Ubicanimal.Application/"]
COPY ["Ubicanimal.Domain/Ubicanimal.Domain.csproj", "Ubicanimal.Domain/"]
COPY ["Ubicanimal.Infrastructure/Ubicanimal.Infrastructure.csproj", "Ubicanimal.Infrastructure/"]
RUN dotnet restore "./Ubicanimal.Api/./Ubicanimal.Api.csproj"
COPY . .
WORKDIR "/src/Ubicanimal.Api"
RUN dotnet build "./Ubicanimal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ubicanimal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ubicanimal.Api.dll"]
