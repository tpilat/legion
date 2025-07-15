# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Directory.Build.targets", "src/"]
COPY ["ADF/Cache/Legion.ADF.Cache.RestApi/Legion.ADF.Cache.RestApi.csproj", "ADF/Cache/Legion.ADF.Cache.RestApi/"]
COPY ["src/Legion.AspNetCore/Legion.AspNetCore.csproj", "src/Legion.AspNetCore/"]
COPY ["src/Legion/Legion.csproj", "src/Legion/"]
COPY ["ADF/Logs/Legion.ADF.Logs.PostgreSQL/Legion.ADF.Logs.PostgreSQL.csproj", "ADF/Logs/Legion.ADF.Logs.PostgreSQL/"]
COPY ["src/Legion.Database.PostgreSQL/Legion.Database.PostgreSQL.csproj", "src/Legion.Database.PostgreSQL/"]
COPY ["src/Legion.EntityFrameworkCore.PostgreSQL/Legion.EntityFrameworkCore.PostgreSQL.csproj", "src/Legion.EntityFrameworkCore.PostgreSQL/"]
COPY ["src/Legion.EntityFrameworkCore/Legion.EntityFrameworkCore.csproj", "src/Legion.EntityFrameworkCore/"]
COPY ["ADF/Logs/Legion.ADF.Logs/Legion.ADF.Logs.csproj", "ADF/Logs/Legion.ADF.Logs/"]
COPY ["ADF/Logs/Legion.ADF.Logs.Abstractions/Legion.ADF.Logs.Abstractions.csproj", "ADF/Logs/Legion.ADF.Logs.Abstractions/"]
COPY ["ADF/Cache/Legion.ADF.Cache.PostgreSQL/Legion.ADF.Cache.PostgreSQL.csproj", "ADF/Cache/Legion.ADF.Cache.PostgreSQL/"]
COPY ["ADF/Cache/Legion.ADF.Cache/Legion.ADF.Cache.csproj", "ADF/Cache/Legion.ADF.Cache/"]
COPY ["ADF/Cache/Legion.ADF.Cache.Abstractions/Legion.ADF.Cache.Abstractions.csproj", "ADF/Cache/Legion.ADF.Cache.Abstractions/"]
RUN dotnet restore "./ADF/Cache/Legion.ADF.Cache.RestApi/Legion.ADF.Cache.RestApi.csproj"
COPY . .
WORKDIR "/src/ADF/Cache/Legion.ADF.Cache.RestApi"
RUN dotnet build "./Legion.ADF.Cache.RestApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Legion.ADF.Cache.RestApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Legion.ADF.Cache.RestApi.dll"]