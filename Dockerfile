# Use the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./

# Restore as distinct layers
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Use the .NET runtime for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

# Copy the output from the build stage
COPY --from=build-env /App/out .

# Copy the WorkArea directory
COPY WorkArea ./WorkArea

# Set the entry point to the application
ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]
