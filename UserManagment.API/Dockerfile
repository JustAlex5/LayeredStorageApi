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

COPY ["UserManagment.API.csproj", "UserManagment/"]
RUN dotnet restore "./UserManagment/UserManagment.API.csproj"

WORKDIR "/src/UserManagment"
COPY . .
RUN dotnet build "UserManagment.API.csproj" -c Release -o /app/build

 # Publish stage
FROM build AS publish
ARG BUILD_VERSION

WORKDIR /src/UserManagment
RUN dotnet publish "UserManagment.API.csproj" -c Release -o /app/publish /p:UseAppHost=false --os linux --arch x64


# Final stage
FROM base AS final
ARG BUILD_VERSION

WORKDIR /app


# Ensure the appsettings.json files are copied to the working directory
# Copy everything from the source directory to the working directory in the final image
COPY . .

COPY --from=build /app/build .

COPY --from=publish  /app/publish .

ENTRYPOINT ["dotnet", "UserManagment.API.dll"]
ENV BUILD_VERSION=$BUILD_VERSION