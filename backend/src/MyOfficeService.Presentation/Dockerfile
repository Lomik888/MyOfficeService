FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MyOfficeService.Presentation/MyOfficeService.Presentation.csproj", "src/MyOfficeService.Presentation/"]
COPY ["src/MyOfficeService.Infrastructure.Postgres/MyOfficeService.Infrastructure.Postgres.csproj", "src/MyOfficeService.Infrastructure.Postgres/"]
COPY ["src/MyOfficeService.Application/MyOfficeService.Application.csproj", "src/MyOfficeService.Application/"]
COPY ["src/MyOfficeService.Domain/MyOfficeService.Domain.csproj", "src/MyOfficeService.Domain/"]
RUN dotnet restore "src/MyOfficeService.Presentation/MyOfficeService.Presentation.csproj"
COPY . .
WORKDIR "/src/src/MyOfficeService.Presentation"
RUN dotnet build "MyOfficeService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyOfficeService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyOfficeService.Presentation.dll"]
