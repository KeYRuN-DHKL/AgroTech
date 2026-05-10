# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore
COPY AgroTechProject.sln .
COPY ProjectFiles/*.csproj ./ProjectFiles/
RUN dotnet restore

# Copy everything and publish
COPY ProjectFiles/. ./ProjectFiles/
WORKDIR /src/ProjectFiles
RUN dotnet publish -c Release -o /app/publish

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AgroTechProject.dll"]