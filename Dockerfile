FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copiar los archivos de proyecto y restaurar dependencias
COPY ["./src/Presentation.API/Presentation.API.csproj", "Presentation.API/"]
COPY ["./src/Domain.Core/Domain.Core.csproj", "Domain.Core/"]
COPY ["./src/Infrastructure.Hasher/Infrastructure.Hasher.csproj", "Infrastructure.Hasher/"]
COPY ["./src/Infrastructure.Mapper.AutoMapper/Infrastructure.Mapper.AutoMapper.csproj", "Infrastructure.Mapper.AutoMapper/"]
COPY ["./src/Infrastructure.Repository.EF/Infrastructure.Repository.EF.csproj", "Infrastructure.Repository.EF/"]
RUN dotnet restore "./src/Presentation.API/Presentation.API.csproj"

# Copiar todo el código fuente
COPY . .
WORKDIR "Presentation.API"
RUN dotnet build "Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copiar y configurar permisos de las llaves
COPY ["Keys/", "/app/Keys/"]
RUN mkdir -p /app/keys-protection && \
    chmod -R 755 /app/Presentation.API && \
    chmod 644 /app/Keys/private.key && \
    chmod 644 /app/Keys/public.key && \
    chmod 755 /app/Keys

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DataProtection__Path=/app/keys-protection

ENTRYPOINT ["dotnet", "Presentation.API.dll"]