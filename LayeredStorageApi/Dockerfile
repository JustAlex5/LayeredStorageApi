ARG BUILD_VERSION=0.0.0

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG BUILD_VERSION

WORKDIR /app

EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_HTTP_PORTS=80
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_VERSION

WORKDIR /src

COPY ["LayeredStorageApi.csproj", "LayeredStorageApi/"]
RUN dotnet restore "./LayeredStorageApi/LayeredStorageApi.csproj"

WORKDIR "/src/LayeredStorageApi"
COPY . .
RUN dotnet build "LayeredStorageApi.csproj" -c Release -o /app/build

 # Publish stage
FROM build AS publish
ARG BUILD_VERSION

WORKDIR /src/LayeredStorageApi
RUN dotnet publish "LayeredStorageApi.csproj" -c Release -o /app/publish /p:UseAppHost=false --os linux --arch x64


# Final stage
FROM base AS final
ARG BUILD_VERSION

WORKDIR /app


# Ensure the appsettings.json files are copied to the working directory
# Copy everything from the source directory to the working directory in the final image
COPY . .

COPY --from=build /app/build .

COPY --from=publish  /app/publish .

ENTRYPOINT ["dotnet", "LayeredStorageApi.dll"]
ENV BUILD_VERSION=$BUILD_VERSION