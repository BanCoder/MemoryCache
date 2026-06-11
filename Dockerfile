FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем все .csproj файлы
COPY ["MemoryCache.Web/MemoryCache.Web.csproj", "MemoryCache.Web/"]
COPY ["BusinessLogic/BusinessLogic.csproj", "BusinessLogic/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]

# Восстанавливаем зависимости
RUN dotnet restore "MemoryCache.Web/MemoryCache.Web.csproj"

# Копируем весь код
COPY . .

# Собираем веб-проект
WORKDIR "/src/MemoryCache.Web"
RUN dotnet build "MemoryCache.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MemoryCache.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MemoryCache.Web.dll"]