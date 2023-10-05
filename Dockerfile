# Start with the .NET 7 SDK image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files to the container
COPY PlatformApi/*.csproj ./PlatformApi/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Core/*.csproj ./Core/

# Restore NuGet packages for all projects
RUN dotnet restore PlatformApi/PlatformApi.csproj

# Copy the full project code to the container
COPY . .

# Build the project
RUN dotnet build PlatformApi/PlatformApi.csproj -c Release --no-restore

# Publish the project
RUN dotnet publish PlatformApi/PlatformApi.csproj -c Release --no-restore -o /app/out

# Create a new runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out ./

# Expose the HTTP port
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "PlatformApi.dll", "--environment=Development"]
