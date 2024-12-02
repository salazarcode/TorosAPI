FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./src/Presentation.API/Presentation.API.csproj", "Presentation.API/"]
COPY ["./src/Domain.Core/Domain.Core.csproj", "Domain.Core/"]
COPY ["./src/Infrastructure.Hasher/Infrastructure.Hasher.csproj", "Infrastructure.Hasher/"]
COPY ["./src/Infrastructure.Mapper.AutoMapper/Infrastructure.Mapper.AutoMapper.csproj", "Infrastructure.Mapper.AutoMapper/"]
COPY ["./src/Infrastructure.Repository.EF/Infrastructure.Repository.EF.csproj", "Infrastructure.Repository.EF/"]
RUN dotnet restore "Presentation.API/Presentation.API.csproj"
COPY . .
WORKDIR "Presentation.API"
RUN dotnet build "Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/keys-protection /app/Keys \
    && chmod -R 755 /app/keys-protection \
    && chmod 755 /app/Keys
COPY Keys/* /app/Keys/
RUN chmod 644 /app/Keys/*

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DataProtection__Path=/app/keys-protection
ENTRYPOINT ["dotnet", "Presentation.API.dll"]