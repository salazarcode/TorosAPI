# Imagen base para el runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Imagen para la compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar los archivos .csproj y restaurar dependencias
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["DTOs/Infra.DTOs.csproj", "DTOs/"]
COPY ["Infra.Mappings/Infra.Mappings.csproj", "Infra.Mappings/"]
COPY ["Repositories.EF/Infra.Repositories.EF.csproj", "Repositories.EF/"]
RUN dotnet restore "./API/API.csproj"

# Copiar el resto del código
COPY . .
WORKDIR "/src/API"

# Compilar
RUN dotnet build "./API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]