#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Triast/Suap.Triast.WebApi/Suap.Triast.WebApi.csproj", "Triast/Suap.Triast.WebApi/"]
COPY ["Common/Suap.Common.Api/Suap.Common.Api.csproj", "Common/Suap.Common.Api/"]
COPY ["Common/Suap.Common.Errors/Suap.Common.Errors.csproj", "Common/Suap.Common.Errors/"]
COPY ["Common/Suap.Common.Logging/Suap.Common.Logging.csproj", "Common/Suap.Common.Logging/"]
COPY ["Common/Suap.Common.Jwt/Suap.Common.Jwt.csproj", "Common/Suap.Common.Jwt/"]
RUN dotnet restore "./Triast/Suap.Triast.WebApi/Suap.Triast.WebApi.csproj"
COPY . .
WORKDIR "/src/Triast/Suap.Triast.WebApi"
RUN dotnet build "./Suap.Triast.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Suap.Triast.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Suap.Triast.WebApi.dll"]