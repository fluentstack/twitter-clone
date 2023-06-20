#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["gateway/Gateway.csproj", "gateway/"]
COPY ["framework/Core/FSH.Framework.Core.csproj", "framework/Core/"]
COPY ["framework/Infrastructure/FSH.Framework.Infrastructure.csproj", "framework/Infrastructure/"]
RUN dotnet restore "gateway/Gateway.csproj"
COPY . .
WORKDIR "/src/gateway"
RUN dotnet build "Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Twitter.Gateway.dll"]