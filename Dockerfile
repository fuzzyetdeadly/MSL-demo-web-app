# Stage 1 - Build container
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy and restore only the app project
COPY MyWebApp/MyWebApp.csproj MyWebApp/
RUN dotnet restore MyWebApp/MyWebApp.csproj

# Copy the rest of the app source and publish it
COPY MyWebApp/ MyWebApp/
RUN dotnet publish MyWebApp/MyWebApp.csproj -c Release -o /app/publish

# Stage 2 - Run container
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy the built app into the container
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MyWebApp.dll"]