# Stage 1: Runtime Base
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build Environment
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Copy all project dependency definitions using paths relative to the solution root
COPY ["Gymly.Web/Gymly.Web.csproj", "Gymly.Web/"]
COPY ["Gymly.Application/Gymly.Application.csproj", "Gymly.Application/"]
COPY ["Gymly.Domain/Gymly.Domain.csproj", "Gymly.Domain/"]
COPY ["Gymly.Infrastructure/Gymly.Infrastructure.csproj", "Gymly.Infrastructure/"]

# 2. Run the restore process over the entire solution dependency tree
RUN dotnet restore "Gymly.Web/Gymly.Web.csproj"

# 3. Copy everything else and navigate to the Web project context
COPY . .
WORKDIR "/src/Gymly.Web"
RUN dotnet build "Gymly.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Gymly.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final Image Entry
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gymly.Web.dll"]