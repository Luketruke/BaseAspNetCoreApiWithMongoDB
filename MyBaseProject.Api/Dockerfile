FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyBaseProject.Api/MyBaseProject.Api.csproj", "MyBaseProject.Api/"]
COPY ["MyBaseProject.Application/MyBaseProject.Application.csproj", "MyBaseProject.Application/"]
COPY ["MyBaseProject.Domain/MyBaseProject.Domain.csproj", "MyBaseProject.Domain/"]
COPY ["MyBaseProject.Infrastructure/MyBaseProject.Infrastructure.csproj", "MyBaseProject.Infrastructure/"]
RUN dotnet restore "./MyBaseProject.Api/./MyBaseProject.Api.csproj"
COPY . .
WORKDIR "/src/MyBaseProject.Api"
RUN dotnet build "./MyBaseProject.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyBaseProject.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyBaseProject.Api.dll"]