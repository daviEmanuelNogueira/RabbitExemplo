# Use the official .NET 9 SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official .NET 9 runtime image as a runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Expose the port the app runs on
EXPOSE 8080

# Set the entry point for the application
ENTRYPOINT ["dotnet", "api.dll"]