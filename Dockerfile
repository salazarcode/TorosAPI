# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["src/Presentation.API/Presentation.API.csproj", "src/Presentation.API/"]
COPY ["src/Domain.Core/Domain.Core.csproj", "src/Domain.Core/"]
COPY ["src/Infrastructure.Hasher/Infrastructure.Hasher.csproj", "src/Infrastructure.Hasher/"]
COPY ["src/Infrastructure.Mapper.AutoMapper/Infrastructure.Mapper.AutoMapper.csproj", "src/Infrastructure.Mapper.AutoMapper/"]
COPY ["src/Infrastructure.Repository.EF/Infrastructure.Repository.EF.csproj", "src/Infrastructure.Repository.EF/"]
RUN dotnet restore "src/Presentation.API/Presentation.API.csproj"

# Copiar el resto del código fuente y construir
COPY . /src
WORKDIR "/src/Presentation.API"
RUN dotnet build "Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configuración de claves y permisos
RUN mkdir -p /app/keys-protection /app/Keys \
    && chmod -R 755 /app/keys-protection \
    && chmod 755 /app/Keys
COPY Keys/* /app/Keys/
RUN chmod 644 /app/Keys/*

# Variables de entorno y entrada
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DataProtection__Path=/app/keys-protection
ENTRYPOINT ["dotnet", "Presentation.API.dll"]
