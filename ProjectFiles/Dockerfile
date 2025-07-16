# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install EF CLI tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy solution and project files
COPY *.sln .
COPY ProjectFiles/*.csproj ./ProjectFiles/
RUN dotnet restore

# Copy the entire project files
COPY ProjectFiles/. ./ProjectFiles/
WORKDIR /src/ProjectFiles

# Optional: Uncomment this line if you want EF migrations to auto-apply on build (not recommended in Dockerfile usually)
# RUN dotnet ef database update

RUN dotnet publish -c Release -o /app/publish

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "AgroTechProject.dll"]
