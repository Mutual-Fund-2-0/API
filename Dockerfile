# 1. Build Stage
# Using Alpine for the build stage speeds up the image pull slightly
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Copy all configuration and solution files first to optimize layer caching
COPY ["global.json", "Directory.Build.props", "Directory.Build.targets", "Directory.Packages.props", "nuget.config", "MF.sln", "dotnet-tools.json", ".editorconfig", "./"]

# Copy project files individually
COPY ["API/API.csproj", "API/"]
COPY ["IT/IT.csproj", "IT/"]
COPY ["UT/UT.csproj", "UT/"]

# This ensures NuGet packages are cached as a Docker layer unless project files change.
RUN dotnet restore "MF.sln"

# Copy EVERYTHING else (Ensure you have a .dockerignore file!)
COPY . .

# Build and Publish
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-resolute-chiseled AS runtime
WORKDIR /app

# Modern .NET port configuration
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Run as a non-root user (built into Microsoft's modern images) for security compliance
USER $APP_UID

ENTRYPOINT ["dotnet", "API.dll"]
