# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Install Node.js and Yarn Classic for SPA build
RUN --mount=type=cache,target=/var/cache/apt,sharing=locked \
    --mount=type=cache,target=/var/lib/apt,sharing=locked \
    apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    curl -fsSL https://deb.nodesource.com/setup_22.x | bash - && \
    apt-get install -y --no-install-recommends nodejs && \
    npm install -g yarn && \
    rm -rf /var/lib/apt/lists/*

COPY ["Directory.Build.props", "."]
COPY ["AutoTest.Web/AutoTest.Web.csproj", "AutoTest.Web/"]
COPY ["AutoTest.Domain/AutoTest.Domain.csproj", "AutoTest.Domain/"]
COPY ["AutoTest.Service/AutoTest.Service.csproj", "AutoTest.Service/"]
COPY ["AutoTest.Persistence/AutoTest.Persistence.csproj", "AutoTest.Persistence/"]
RUN dotnet restore "./AutoTest.Web/AutoTest.Web.csproj"

# Install SPA dependencies first (cached unless package files change)
WORKDIR "/src/ClientApp"
COPY ["ClientApp/package.json", "ClientApp/yarn.lock", "./"]
COPY ["ClientApp/src/types/fake-empty", "src/types/fake-empty/"]
RUN --mount=type=cache,target=/root/.cache/yarn,sharing=locked yarn install --frozen-lockfile --network-timeout 1000000

WORKDIR /src
COPY . .
WORKDIR "/src/AutoTest.Web"
RUN dotnet build "./AutoTest.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AutoTest.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoTest.Web.dll"]