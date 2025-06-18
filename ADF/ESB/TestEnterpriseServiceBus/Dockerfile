#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR src
COPY ["/src/Directory.Build.targets", "src/"]
COPY ["/ADF/ESB/TestEnterpriseServiceBus/TestEnterpriseServiceBus.csproj", "ESB/TestEnterpriseServiceBus/"]
COPY ["/src/Legion.Logging.PostgreSQL/Legion.Logging.PostgreSQL.csproj", "src/Legion.Logging.PostgreSQL/"]
COPY ["/src/Legion.EntityFrameworkCore.PostgreSQL/Legion.EntityFrameworkCore.PostgreSQL.csproj", "src/Legion.EntityFrameworkCore.PostgreSQL/"]
COPY ["/src/Legion.EntityFrameworkCore/Legion.EntityFrameworkCore.csproj", "src/Legion.EntityFrameworkCore/"]
COPY ["/src/Legion/Legion.csproj", "src/Legion/"]
COPY ["/ADF/ESB/Legion.ADF.ESB.ServiceBus.PostgreSQL/Legion.ADF.ESB.ServiceBus.PostgreSQL.csproj", "ESB/Legion.ADF.ESB.ServiceBus.PostgreSQL/"]
COPY ["/ADF/ESB/Legion.ADF.ESB.Components.PostgreSQL/Legion.ADF.ESB.Components.PostgreSQL.csproj", "ESB/Legion.ADF.ESB.Components.PostgreSQL/"]
COPY ["/ADF/ESB/Legion.ADF.ESB.Components/Legion.ADF.ESB.Components.csproj", "ESB/Legion.ADF.ESB.Components/"]
COPY ["/ADF/ESB/Legion.ADF.ESB.Abstractions/Legion.ADF.ESB.Abstractions.csproj", "ESB/Legion.ADF.ESB.Abstractions/"]
COPY ["/ADF/ESB/Legion.ADF.ESB.MBox.PostgreSQL/Legion.ADF.ESB.MBox.PostgreSQL.csproj", "ESB/Legion.ADF.ESB.MBox.PostgreSQL/"]
COPY ["/ADF/ESB/Legion.ADF.ESB.MBox/Legion.ADF.ESB.MBox.csproj", "ESB/Legion.ADF.ESB.MBox/"]
COPY ["/ADF/ESB/Legion.ADF.ESB.Orchestrations.PostgreSQL/Legion.ADF.ESB.Orchestrations.PostgreSQL.csproj", "ESB/Legion.ADF.ESB.Orchestrations.PostgreSQL/"]
COPY ["/ADF/ESB/Legion.ADF.ESB.Orchestrations/Legion.ADF.ESB.Orchestrations.csproj", "ESB/Legion.ADF.ESB.Orchestrations/"]
COPY ["/ADF/ESB/Legion.ADF.ESB.ServiceBus/Legion.ADF.ESB.ServiceBus.csproj", "ESB/Legion.ADF.ESB.ServiceBus/"]
RUN dotnet restore "./ESB/TestEnterpriseServiceBus/TestEnterpriseServiceBus.csproj"
COPY . .
WORKDIR "/src/ESB/TestEnterpriseServiceBus"
RUN dotnet build "./TestEnterpriseServiceBus.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TestEnterpriseServiceBus.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestEnterpriseServiceBus.dll"]