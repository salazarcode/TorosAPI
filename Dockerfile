FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copiar los archivos de proyecto y restaurar dependencias
COPY ["API/API.csproj", "API/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Repository/Repository.csproj", "Repository/"]
RUN dotnet restore "API/API.csproj"

# Copiar todo el código fuente
COPY . .
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copiar y configurar permisos de las llaves
COPY ["API/Keys/", "/app/API/Keys/"]
RUN mkdir -p /app/keys-protection && \
    chmod -R 755 /app/API && \
    chmod 644 /app/API/Keys/private.key && \
    chmod 644 /app/API/Keys/public.key && \
    chmod 755 /app/API/Keys

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DataProtection__Path=/app/keys-protection

ENTRYPOINT ["dotnet", "API.dll"]