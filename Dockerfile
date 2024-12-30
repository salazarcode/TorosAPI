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
COPY ["src/Presentation.API/Presentation.API.csproj", "Presentation.API/"]
COPY ["src/Domain.Core/Domain.Core.csproj", "Domain.Core/"]
COPY ["src/Infrastructure.Hasher/Infrastructure.Hasher.csproj", "Infrastructure.Hasher/"]
COPY ["src/Infrastructure.Mapper.AutoMapper/Infrastructure.Mapper.AutoMapper.csproj", "Infrastructure.Mapper.AutoMapper/"]
COPY ["src/Insfrastructure.Email.AwsSES/Insfrastructure.Email.AwsSES.csproj", "Insfrastructure.Email.AwsSES/"]
COPY ["src/Infrastructure.Repository.MongoDB/Infrastructure.Repository.MongoDB.csproj", "Infrastructure.Repository.MongoDB/"]
RUN dotnet restore "Presentation.API/Presentation.API.csproj"

# Copiar el resto del código fuente y construir
COPY ./src /src
WORKDIR "/src/Presentation.API"
RUN dotnet build "Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicación
FROM build AS publish
WORKDIR "/src/Presentation.API"
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish

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
