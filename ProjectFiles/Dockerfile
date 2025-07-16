
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY ProjectFiles/*.csproj ./ProjectFiles/
RUN dotnet restore

COPY ProjectFiles/. ./ProjectFiles/
WORKDIR /src/ProjectFiles
RUN dotnet publish -c Release -o /app/publish

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "AgroTechProject.dll"]