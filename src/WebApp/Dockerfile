#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WebApp/WebApp.csproj", "src/WebApp/"]
COPY ["src/CShop.Infras/CShop.Infras.csproj", "src/CShop.Infras/"]
COPY ["src/CShop.UseCases/CShop.UseCases.csproj", "src/CShop.UseCases/"]
RUN dotnet restore "./src/WebApp/./WebApp.csproj"
COPY . .
WORKDIR "/src/src/WebApp"
RUN dotnet build "./WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.dll"]