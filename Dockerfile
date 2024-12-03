
RUN echo "------------------------------------------------------------------"
RUN echo "1. Etapa base, construye el contenerdor principal"
RUN echo "------------------------------------------------------------------"

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN echo "------------------------------------------------------------------"
RUN echo "2. Crea un contenedor para compilar el proyecto posicionándose en la carpeta /src"
RUN echo "------------------------------------------------------------------"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN echo "------------------------------------------------------------------"
RUN echo "3. Copia los archivos del proyecto para restaurar las dependencias"
RUN echo "------------------------------------------------------------------"

COPY ["src/Presentation.API/Presentation.API.csproj", "Presentation.API/"]
COPY ["src/Domain.Core/Domain.Core.csproj", "Domain.Core/"]
COPY ["src/Infrastructure.Hasher/Infrastructure.Hasher.csproj", "Infrastructure.Hasher/"]
COPY ["src/Infrastructure.Mapper.AutoMapper/Infrastructure.Mapper.AutoMapper.csproj", "Infrastructure.Mapper.AutoMapper/"]
COPY ["src/Infrastructure.Repository.EF/Infrastructure.Repository.EF.csproj", "Infrastructure.Repository.EF/"]
RUN dotnet restore "Presentation.API/Presentation.API.csproj"

RUN echo "------------------------------------------------------------------"
RUN echo "4. Ahora copia todo el codigo y compila"
RUN echo "------------------------------------------------------------------"

COPY . /src
WORKDIR "/src/Presentation.API"
RUN dotnet build "Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN echo "------------------------------------------------------------------"
RUN echo "5. Luego de compilar exitosamente, publica"
RUN echo "------------------------------------------------------------------"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN echo "------------------------------------------------------------------"
RUN echo "6. Toma el publicado y lo mete en el contenedor final que se creó al principio"
RUN echo "------------------------------------------------------------------"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN echo "------------------------------------------------------------------"
RUN echo "7. configura la carpeta de claves y permisos"
RUN echo "------------------------------------------------------------------"

RUN mkdir -p /app/keys-protection /app/Keys \
    && chmod -R 755 /app/keys-protection \
    && chmod 755 /app/Keys
COPY Keys/* /app/Keys/
RUN chmod 644 /app/Keys/*   

RUN echo "------------------------------------------------------------------"
RUN echo "8. Define variables de entorno y arranca la aplicación"
RUN echo "------------------------------------------------------------------"

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DataProtection__Path=/app/keys-protection
ENTRYPOINT ["dotnet", "Presentation.API.dll"]
