# Building the app from source
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as builder

WORKDIR /source

COPY ./Migrator.csproj .

# Restore here so the packages are cached
RUN dotnet restore

COPY ./Program.cs ./Program.cs

RUN dotnet publish -o out

# Image should contain as little as possible
FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime

WORKDIR /app

COPY --from=builder /source/out ./

# Copy the default settings over
COPY ./appsettings.json ./appsettings.json

ENTRYPOINT ["dotnet", "Migrator.dll"]